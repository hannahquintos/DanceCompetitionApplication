using DanceCompetitionApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Web.Script.Serialization;

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
            client.BaseAddress = new Uri("https://localhost:44355/api/performancedata/");
        }

        // GET: Performance/List
        public ActionResult List()
        {
            // get list of performances in the system through an HTTP request
            // GET {resource}/api/performancedata/listperformances
            // curl https://localhost:44355/api/performancedata/listperformances

            // set the url
            string url = "listperformances";

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
            // get one performance in the system through an HTTP request
            // GET {resource}/api/performancedata/findperformance{id}
            // curl https://localhost:44355/api/performancedata/findperformance/{id}

            // set the url
            string url = "findperformance/"+id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            PerformanceDto SelectedPerformance = response.Content.ReadAsAsync<PerformanceDto>().Result;


            //Views/Performance/List.cshtml
            return View(SelectedPerformance);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Performance/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Performance/Create
        [HttpPost]
        public ActionResult Create(Performance performance)
        {
            string url = "addperformance";

            string jsonpayload = jss.Serialize(performance);

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
            // get the performance information

            string url = "findperformance/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PerformanceDto selectedperformance = response.Content.ReadAsAsync<PerformanceDto>().Result;

            return View(selectedperformance);
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
                string url = "updateperformance/" + id;

                // serialize into JSON
                string jsonpayload = jss.Serialize(performance);
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

            string url = "findperformance/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PerformanceDto selectedperformance = response.Content.ReadAsAsync<PerformanceDto>().Result;

            return View(selectedperformance);
        }

        // POST: Performance/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Performance performance)
        {
            //send the request to the API
            string url = "deleteperformance/" + id;

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
