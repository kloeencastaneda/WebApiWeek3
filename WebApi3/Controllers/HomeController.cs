﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace WebApi3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<Contact> contacts = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:11094/api/");
                //HTTP GET
                var responseTask = client.GetAsync("Contacts");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Contact>>();
                    readTask.Wait();

                    contacts = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    contacts = Enumerable.Empty<Contact>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(contacts);
        }
    }
}