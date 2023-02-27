using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using passionProject1.Models;
using System.Web.Script.Serialization;
using System.Web.Optimization;
using passionProject1.Models.ViewModels;
namespace passionProject1.Controllers
{
    public class CategoryController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static CategoryController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44378/api/Categoriesdata/");

         

        }
        // GET: Category/list
        public ActionResult List()
        {
            string url = "listCategories";

            HttpResponseMessage response = client.GetAsync(url).Result;
            Debug.WriteLine("The response code is ");//9-
            Debug.WriteLine(response.StatusCode);//7- statusCode allows us to see info if the request sucssesfully handeled.

            IEnumerable<CategoryDto> Categories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;

            //Debug.WriteLine("Number of category recieve : ");//12-
            //Debug.WriteLine(places.Count());//13-
            //Debug.WriteLine(Categories.Count());//13-

            return View(Categories);
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            DetailsCategories ViewModel = new DetailsCategories();

            string url = "findCategory/" + id;// 16 -establishing URL communicating end point.
            HttpResponseMessage response = client.GetAsync(url).Result;//-accessing to api from client .Result() to get the result

            CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;

            ViewModel.SelectedCategory = SelectedCategory;


            //showcase information about places related to this categories
            //send a request to gather information about places related to a particular category ID
            HttpClient client2 = new HttpClient();
            client2.BaseAddress = new Uri("https://localhost:44378/api/");

            string url2 = "placesdata/listplacesforcategories/" + id;
            HttpResponseMessage PlaceResponse = client2.GetAsync(url2).Result;
            IEnumerable<placeDto> RelatedPlaces = PlaceResponse.Content.ReadAsAsync<IEnumerable<placeDto>>().Result;

            ViewModel.RelatedPlaces = RelatedPlaces;
            

            return View(ViewModel);
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
        public ActionResult Create(Category Category)
        {
            string url = "addCategory";

            string jsonpayload = jss.Serialize(Category); // 20-


            HttpContent content = new StringContent(jsonpayload); // 21-

            content.Headers.ContentType.MediaType = "application/json"; // 22- to send the new place to database

            HttpResponseMessage response = client.PostAsync(url, content).Result;// 21- && 23
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
            Updatecategory ViewModel = new Updatecategory();
            string url = "findCategories/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
            ViewModel.SelectedCategory = SelectedCategory;

            //url = "categoriesdata/listcategories/";
            //response = client.GetAsync(url).Result;
            //IEnumerable<categoryDto> CategoriesOptions = response.Content.ReadAsAsync<IEnumerable<categoryDto>>().Result;

            //ViewModel.CategoriesOptions = CategoriesOptions;

            return View(SelectedCategory);
        }

        // POST: Category/Update/5
        [HttpPost]
        public ActionResult Update(int id, Category Category)
        {
            string url = "updateCategory/" + id;
            string jsonpayload = jss.Serialize(Category);
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

        // GET: Category/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "findCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
            return View(SelectedCategory);
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "deleteCategory/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "aplication/json";
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