using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GamePlan.Models;
using Newtonsoft.Json;

namespace GamePlan.Controllers
{
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Events
        public ActionResult Index()
        {

            return View(db.Events.ToList());

            
        }

        public async Task<ActionResult> AllEvents()
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

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
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
        public async Task<ActionResult> Create(Event @event)
        {
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(@event), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:49757/api/Events", stringContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("AllEvents");
                }
                else
                {
                    return RedirectToAction("Create");
                }

            }

            //if (ModelState.IsValid)
            //{
            //    db.Events.Add(@event);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(@event);
            //public ActionResult Create([Bind(Include = "Id,Description,Lat,Lng,Date")] Event @event)
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Lat,Lng,Date")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
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
