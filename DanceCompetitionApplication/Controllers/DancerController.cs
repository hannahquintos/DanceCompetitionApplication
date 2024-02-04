using DanceCompetitionApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace DanceCompetitionApplication.Controllers
{
    public class DancerController : Controller
    {
        // code factoring to reduce redundancy
        // instantiate HttpClient once which can then be re-used through the lifetime of the application
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static DancerController()
        {
            client = new HttpClient();
            //set base part of path for accessing information in dancer controller
            client.BaseAddress = new Uri("https://localhost:44355/api/");
        }

        // GET: Dancer/List
        public ActionResult List()
        {
            // get list of dancers in the system through an HTTP request
            // GET {resource}/api/dancerdata/listdancers
            // curl https://localhost:44355/api/dancerdata/listdancers

            // set the url
            string url = "dancerdata/listdancers";

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<DancerDto> Dancers = response.Content.ReadAsAsync<IEnumerable<DancerDto>>().Result;

            /*foreach (DancerDto Dancer in Dancers)
            {
                Debug.WriteLine("Recieved dancer: " + Dancer.FirstName);
            }*/

            //Views/Dancer/List.cshtml
            return View(Dancers);
        }

        // GET: Dancer/Details/5
        public ActionResult Details(int id)
        {
            // get one dancer in the system through an HTTP request
            // GET {resource}/api/dancerdata/finddancer/{id}
            // curl https://localhost:44355/api/dancerdata/finddancer/{id}

            // set the url
            string url = "dancerdata/finddancer/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            DancerDto SelectedDancer = response.Content.ReadAsAsync<DancerDto>().Result;


            //Views/Dancer/List.cshtml
            return View(SelectedDancer);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Dancer/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Dancer/Create
        [HttpPost]
        public ActionResult Create(Dancer dancer)
        {
            string url = "dancerdata/adddancer";

            string jsonpayload = jss.Serialize(dancer);

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

        // GET: Dancer/Edit/5
        public ActionResult Edit(int id)
        {
            // get the dancer information

            string url = "dancerdata/finddancer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DancerDto SelectedDancer = response.Content.ReadAsAsync<DancerDto>().Result;

            return View(SelectedDancer);
        }

        // POST: Dancer/Update/5
        [HttpPost]
        public ActionResult Update(int id, Dancer dancer)
        {
            try
            {
                /*Debug.WriteLine("The new dancer info is:");
                Debug.WriteLine(dancer.FirstName);
                Debug.WriteLine(dancer.LastName);
                Debug.WriteLine(dancer.DateOfBirth);*/

                //send the request to the API
                string url = "dancerdata/updatedancer/" + id;

                // serialize into JSON
                string jsonpayload = jss.Serialize(dancer);
                /*Debug.WriteLine(jsonpayload);*/

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                // POST: api/DancerData/UpdateDancer/{id}
                // Header : Content-Type: application/json

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }

        // GET: Dancer/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            // get the dancer information

            string url = "dancerdata/finddancer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DancerDto SelectedDancer = response.Content.ReadAsAsync<DancerDto>().Result;

            return View(SelectedDancer);
        }

        // POST: Dancer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //send the request to the API
            string url = "dancerdata/deletedancer/" + id;

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
