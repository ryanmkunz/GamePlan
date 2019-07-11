using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GamePlan.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace GamePlan.Controllers
{
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<IEnumerable<Event>> GetAllEvents()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49757/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/Events");
                    response.EnsureSuccessStatusCode();
                    string data = await response.Content.ReadAsStringAsync();
                    var jsonResults = JsonConvert.DeserializeObject<IEnumerable<Event>>(data).ToList();

                    return jsonResults;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public async Task<Event> GetEventById(int? id)
        {
            if (id == null)
            {
                return null;
            }
            var allEvents = await GetAllEvents();
            return allEvents.Where(e => e.Id == id).SingleOrDefault();
        }

        public async Task<ActionResult> Index()
        {
            return View("Index", await GetAllEvents());
        }
        
        public async Task<ActionResult> Map(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View("Map", await GetEventById(id));
        }

        public async Task<ActionResult> Invites()
        {
            var allEvents = await GetAllEvents();
            var invites = allEvents.Where(e => e.Invite == User.Identity.Name).ToList();

            return View("Invites", invites);
        }

        public async Task<ActionResult> Recommended(double? currentTemp)
        {
            var allEvents = await GetAllEvents();
            var filteredByWeekDay = allEvents.Where(e => e.Date.Value.DayOfWeek == DateTime.Today.DayOfWeek).ToList();
            var filteredByWeather = allEvents.Where(e => e.Temp >= currentTemp).ToList();
            var recommended = filteredByWeather;
            //recommended.AddRange(filteredByWeekDay);
            return View(recommended);
        }

        // GET: Events/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await GetEventById(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var selectedEvent = await GetEventById(id);
            return View(selectedEvent);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Category,Description,Lat,Lng,EmailNotification,Date")] Event @event)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49757/");

                //HTTP POST
                var jsonString = JsonConvert.SerializeObject(@event);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var putTask = client.PutAsync("api/Events", content);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return View(@event);
                }
            }
            return RedirectToAction("Index", "ToDoLists");
        }

        // GET: Events/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49757/");
                client.DefaultRequestHeaders.Accept.Clear();
                try
                {
                    HttpResponseMessage response = await client.DeleteAsync("api/Events/" + id);
                    response.EnsureSuccessStatusCode();                   
                }
                catch (Exception e)
                {
                    return RedirectToAction("Index", "ToDoLists");
                }
            }
            return RedirectToAction("Index", "ToDoLists");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
