﻿using DanceCompetitionApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using DanceCompetitionApplication.Models.ViewModels;

namespace DanceCompetitionApplication.Controllers
{
    public class PerformanceController : Controller
    {
        // code factoring to reduce redundancy
        // instantiate HttpClient once which can then be re-used through the lifetime of the application
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static PerformanceController()
        {
            client = new HttpClient();
            //set base part of path for accessing information in performance controller
            client.BaseAddress = new Uri("https://localhost:44355/api/");
        }

        // GET: Performance/List
        public ActionResult List(string SearchKey = null)
        {
            // get list of performances in the system through an HTTP request
            // GET {resource}/api/performancedata/listperformances
            // curl https://localhost:44355/api/performancedata/listperformances

            // set the url
            string url = "performancedata/listperformances/" + SearchKey;

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<PerformanceDto> Performances = response.Content.ReadAsAsync<IEnumerable<PerformanceDto>>().Result;

            /*foreach (PerformanceDto Performance in Performances)
            {
                Debug.WriteLine("Recieved performance: " + Performance.RoutineName);
            }*/

            //Views/Performance/List.cshtml
            return View(Performances);
        }

        // GET: Performance/Details/5
        public ActionResult Details(int id)
        {
            DetailsPerformance ViewModel = new DetailsPerformance();

            // get one performance in the system through an HTTP request
            // GET {resource}/api/performancedata/findperformance/{id}
            // curl https://localhost:44355/api/performancedata/findperformance/{id}

            // set the url
            string url = "performancedata/findperformance/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            PerformanceDto SelectedPerformance = response.Content.ReadAsAsync<PerformanceDto>().Result;

            ViewModel.SelectedPerformance = SelectedPerformance;

            // show associated dancers with this performance
            url = "dancerdata/listdancersforperformance/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DancerDto> DancersInPerformance = response.Content.ReadAsAsync<IEnumerable<DancerDto>>().Result;

            ViewModel.DancersInPerformance = DancersInPerformance;

            url = "dancerdata/listdancersnotinperformance/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DancerDto> AvailableDancers = response.Content.ReadAsAsync<IEnumerable<DancerDto>>().Result;

            ViewModel.AvailableDancers = AvailableDancers;

            //Views/Performance/List.cshtml
            return View(ViewModel);
        }

        //POST: Performance/Associate/{performanceid}
        [HttpPost]
        public ActionResult Associate(int id, int DancerId)
        {
            /*Debug.WriteLine("Attempting to associate performance: " + id + " with dancer: " + DancerId);*/

            // call api to add dancer to performance
            string url = "performancedata/adddancertoperformance/" + id + "/" + DancerId;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        //GET: Performance/UnAssociate/{id}?DancerId={dancerid}
        [HttpGet]
        public ActionResult UnAssociate(int id, int DancerId)
        {
            /*Debug.WriteLine("Attempting to unassociate performance: " + id + " with dancer: " + DancerId);*/

            // call api to add dancer to performance
            string url = "performancedata/removedancerfromperformance/" + id + "/" + DancerId;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Performance/New
        public ActionResult New()
        {
            // access information all categories in the system to choose from when creating a new performance
            // GET: api/categorydata/listcategories

            string url = "categorydata/listcategories";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CategoryDto> CategoryOptions = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;

            return View(CategoryOptions);
        }

        // POST: Performance/Create
        [HttpPost]
        public ActionResult Create(Performance performance)
        {
            string url = "performancedata/addperformance";

            string jsonpayload = JsonConvert.SerializeObject(performance);

            /*Debug.WriteLine(jsonpayload);*/

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Performance/Edit/5
        public ActionResult Edit(int id)
        {
            UpdatePerformance ViewModel = new UpdatePerformance();

            // existing performance information
            string url = "performancedata/findperformance/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PerformanceDto SelectedPerformance = response.Content.ReadAsAsync<PerformanceDto>().Result;
            ViewModel.SelectedPerformance = SelectedPerformance;

            // all categories in the system to choose from when updating a performance
            url = "categorydata/listcategories/";
            response = client.GetAsync(url).Result;
            IEnumerable<CategoryDto> CategoryOptions = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            ViewModel.CategoryOptions = CategoryOptions;

            return View(ViewModel);
        }

        // POST: Performance/Update/5
        [HttpPost]
        public ActionResult Update(int id, Performance performance)
        {
            try
            {
                /*Debug.WriteLine("The new performance info is:");
                Debug.WriteLine(performance.RoutineName);
                Debug.WriteLine(performance.PerformanceTime);
                Debug.WriteLine(performance.Studio);
                Debug.WriteLine(performance.CategoryId);*/

                //send the request to the API
                string url = "performancedata/updateperformance/" + id;

                // serialize into JSON
                string jsonpayload = JsonConvert.SerializeObject(performance);
                /*Debug.WriteLine(jsonpayload);*/

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                // POST: api/PerformanceData/UpdatePerformance/{id}
                // Header : Content-Type: application/json

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                return RedirectToAction("Details/"+id);
            }
            catch
            {
                return View();
            }
        }

        // GET: Performance/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            // get the performance information

            string url = "performancedata/findperformance/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PerformanceDto SelectedPerformance = response.Content.ReadAsAsync<PerformanceDto>().Result;

            return View(SelectedPerformance);
        }

        // POST: Performance/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //send the request to the API
            string url = "performancedata/deleteperformance/" + id;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
