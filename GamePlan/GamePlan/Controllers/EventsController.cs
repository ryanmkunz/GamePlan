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

namespace GamePlan.Controllers
{
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Events
        public async Task<ActionResult> Index()
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

                    return View("Index", jsonResults);
                }
                catch (Exception e)
                {
                    return View("Home");
                }
            }
        }

        public async Task<ActionResult> Map(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //---------------------------------------------------------------
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
                    var singleEvent = jsonResults.Where(e => e.Id == id).SingleOrDefault();
                    return View("Map", singleEvent);
                }
                catch (Exception e)
                {
                    return View("Home");
                }
            }
            //---------------------------------------------------------------
            //Event @event = db.Events.Find(id);
            //if (@event == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(@event);
        }

        public async Task<ActionResult> Invites()
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
                    var invites = jsonResults.Where(e => e.Invite == User.Identity.Name).ToList();

                    return View("Invites", invites);
                }
                catch (Exception e)
                {
                    return View("Home");
                }
            }
        }

        public async Task<ActionResult> Recommended(double? currentTemp)
        {
            var allEvents = await AllEvents();
            var filteredByWeekDay = allEvents.Where(e => e.Date.Value.DayOfWeek == DateTime.Today.DayOfWeek).ToList();
            var filteredByWeather = allEvents.Where(e => e.Temp >= currentTemp).ToList();
            var recommended = filteredByWeather;
            recommended.AddRange(filteredByWeekDay);
            return View(recommended);
        }

        public async Task<IEnumerable<Event>> AllEvents()
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

        // GET: Events/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Category,Description,Lat,Lng,EmailNotification,Date")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
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
            //Event @event = await db.Events.FindAsync(id);
            //if (@event == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(@event);
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
                    var selectedEvent = jsonResults.Where(e => e.Id == id).SingleOrDefault();
                    return View(selectedEvent);
                }
                catch (Exception e)
                {
                    return View("Home");
                }
            }
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Category,Description,Lat,Lng,EmailNotification,Date")] Event @event)
        {
            if (ModelState.IsValid)
            {

                //db.Entry(@event).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                //return RedirectToAction("Index");

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:49757/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //var response = await client.PutAsync("api/Events", @event).Result;
                }
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Event @event = await db.Events.FindAsync(id);
            //if (@event == null)
            //{
            //    return HttpNotFound();
            //}

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49757/");
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = await client.DeleteAsync("api/Events/" + id);
                    response.EnsureSuccessStatusCode();
                    //string data = await response.Content.ReadAsStringAsync();
                    //var jsonResults = JsonConvert.DeserializeObject<IEnumerable<Event>>(data).ToList();
                    //var selectedEvent = jsonResults.Where(e => e.Id == id).SingleOrDefault();                    
                }
                catch (Exception e)
                {
                    return View("Index");
                }
            }
            return View("Index");
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Event @event = await db.Events.FindAsync(id);
            db.Events.Remove(@event);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
