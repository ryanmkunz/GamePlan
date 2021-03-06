﻿using System;
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
    public class ToDoListsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            var lists = await GetAllLists();
            var events = await GetAllEvents();

            foreach (var item in lists)
            {
                var listEvents = events.Where(e => e.ToDoListId == item.Id).ToList();
                item.Events = listEvents;
            }
            return View("NewToDoList", lists);
        }

        public async Task<List<ToDoList>> GetAllLists()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49757/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/ToDoLists");
                    response.EnsureSuccessStatusCode();
                    string data = await response.Content.ReadAsStringAsync();
                    var jsonResults = JsonConvert.DeserializeObject<IEnumerable<ToDoList>>(data).ToList();

                    return jsonResults;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public async Task<ToDoList> GetListById(int? id)
        {
            if (id == null)
            {
                return null;
            }
            var allLists = await GetAllLists();
            return allLists.Where(L => L.Id == id).FirstOrDefault();            
        }

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

        // GET: ToDoLists/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoList toDoList = await db.ToDoLists.FindAsync(id);
            if (toDoList == null)
            {
                return HttpNotFound();
            }
            return View(toDoList);
        }

        // GET: ToDoLists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToDoLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Category")] ToDoList toDoList)
        {
            if (ModelState.IsValid)
            {
                db.ToDoLists.Add(toDoList);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(toDoList);
        }

        // GET: ToDoLists/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var toDoList = await GetListById(id);
            if (toDoList == null)
            {
                return HttpNotFound();
            }
            return View(toDoList);
        }

        // POST: ToDoLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title")] ToDoList toDoList)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49757/");

                //HTTP POST
                var jsonString = JsonConvert.SerializeObject(toDoList);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var putTask = client.PutAsync("api/ToDoLists", content);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return View(toDoList);
                }
            }
            return RedirectToAction("Index", "ToDoLists");
        }

        // GET: ToDoLists/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var allLists = await GetAllLists();
            var currentList = allLists.Where(L => L.Id == id).FirstOrDefault();
            var allEvents = await GetAllEvents();
            var relatedEvents = allEvents.Where(e => e.ToDoListId == currentList.Id).ToList();

            foreach (var item in relatedEvents)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:49757/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    try
                    {
                        HttpResponseMessage response = await client.DeleteAsync("api/Events/" + item.Id);
                        response.EnsureSuccessStatusCode();
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("Index", "ToDoLists");
                    }
                }
            }

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49757/");
                client.DefaultRequestHeaders.Accept.Clear();
                try
                {
                    HttpResponseMessage response = await client.DeleteAsync("api/ToDoLists/" + id);
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
