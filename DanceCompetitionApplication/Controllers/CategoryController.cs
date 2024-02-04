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
    public class CategoryController : Controller
    {
        // code factoring to reduce redundancy
        // instantiate HttpClient once which can then be re-used through the lifetime of the application
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static CategoryController()
        {
            client = new HttpClient();
            //set base part of path for accessing information in category controller
            client.BaseAddress = new Uri("https://localhost:44355/api/");
        }

        // GET: Category/List
        public ActionResult List()
        {
            // get list of categories in the system through an HTTP request
            // GET {resource}/api/categorydata/listcategories
            // curl https://localhost:44355/api/categorydata/listcategories

            // set the url
            string url = "categorydata/listcategories";

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CategoryDto> Categories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;

            /*foreach (CategoryDto Category in Categories)
            {
                Debug.WriteLine("Recieved category: " + Category.CategoryName);
            }*/

            //Views/Category/List.cshtml
            return View(Categories);
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            // get one category in the system through an HTTP request
            // GET {resource}/api/categorydata/findcategory/{id}
            // curl https://localhost:44355/api/categorydata/findcategory/{id}

            // set the url
            string url = "categorydata/findcategory/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;


            //Views/Category/List.cshtml
            return View(SelectedCategory);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Category/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(Category category)
        {
            string url = "categorydata/addcategory";

            string jsonpayload = jss.Serialize(category);

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

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            // get the category information

            string url = "categorydata/findcategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;

            return View(SelectedCategory);
        }

        // POST: Category/Update/5
        [HttpPost]
        public ActionResult Update(int id, Category category)
        {
            try
            {
                /*Debug.WriteLine("The new category info is:");
                Debug.WriteLine(category.CategoryName);*/

                //send the request to the API
                string url = "categorydata/updatecategory/" + id;

                // serialize into JSON
                string jsonpayload = jss.Serialize(category);
                /*Debug.WriteLine(jsonpayload);*/

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                // POST: api/CategoryData/UpdateCategory/{id}
                // Header : Content-Type: application/json

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }

        // GET: Category/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            // get the category information

            string url = "categorydata/findcategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;

            return View(SelectedCategory);
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //send the request to the API
            string url = "categorydata/deletecategory/" + id;

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
