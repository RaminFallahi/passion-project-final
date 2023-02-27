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
    public class BestSeasonToGoController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static BestSeasonToGoController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44378/api/BestSeasonToGoesData/");

        }
        // GET: BestSeasonToGo/list
        public ActionResult List()
        {
            string url = "ListBestSeasonsToGo";
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            Debug.WriteLine("The response code is ");//9-
            Debug.WriteLine(response.StatusCode);//7- statusCode allows us to see info if the request sucssesfully handeled.

            
            IEnumerable<BestSeasonToGoDto> BestSeasonToGoes = response.Content.ReadAsAsync<IEnumerable<BestSeasonToGoDto>>().Result;
            Debug.WriteLine("Number of BestSeasonToGoes recieve : ");//12-
            Debug.WriteLine(BestSeasonToGoes.Count());//13-

            return View(BestSeasonToGoes);
        }

        // GET: BestSeasonToGo/Details/5
        public ActionResult Details(int id)
        {
            DetailsBestSeasonToGo ViewModel = new DetailsBestSeasonToGo();

            string url = "findBestSeasonToGo/" + id;// 16 -establishing URL communicating end point.
            HttpResponseMessage response = client.GetAsync(url).Result;//-accessing to api from client .Result() to get the result
            BestSeasonToGoDto SelectedBestSeasonToGo = response.Content.ReadAsAsync<BestSeasonToGoDto>().Result;
            ViewModel.SelectedBestSeasonToGo = SelectedBestSeasonToGo;

            //showcase information about places related to this BestSeasonToGo
            //send a request to gather information about BestSeasonToGoes related to a particular place ID
            HttpClient client3 = new HttpClient();
            client3.BaseAddress = new Uri("https://localhost:44378/api/");
            
            string url3 = "BestSeasonToGoesData/ListBestSeasonToGoforPlaces/" + id;
            
            HttpResponseMessage PlaceResponse = client3.GetAsync(url3).Result;
            IEnumerable<placeDto> RelatedPlaces = PlaceResponse.Content.ReadAsAsync<IEnumerable<placeDto>>().Result;
            ViewModel.RelatedPlaces = RelatedPlaces;


            return View(ViewModel);
        }
        public ActionResult Error()
        {
            return View();
        }

        // GET: BestSeasonToGo/New
        public ActionResult New()
        {
            return View();
        }

        // POST: BestSeasonToGo/Create
        [HttpPost]
        public ActionResult Create(BestSeasonToGo BestSeasonToGoes)
        {
            string url = "addBestSeasonToGo";

            string jsonpayload = jss.Serialize(BestSeasonToGoes); // 20-


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

        // GET: BestSeasonToGo/Edit/5 
        public ActionResult Edit(int id)
        {
            //UpdateBestSeasonToGo ViewModel = new UpdateBestSeasonToGo();
            string url = "findBestSeasonToGo/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BestSeasonToGoDto SelectedBestSeasonToGo = response.Content.ReadAsAsync<BestSeasonToGoDto>().Result;
            //ViewModel.SelectedBestSeasonToGo = SelectedBestSeasonToGo;

            //url = "categoriesdata/listcategories/";
            //response = client.GetAsync(url).Result;
            //IEnumerable<BestSeasonToGoDto> BestSeasonToGoesOptions = response.Content.ReadAsAsync<IEnumerable<BestSeasonToGoDto>>().Result;

            //ViewModel.BestSeasonToGoesOptions = BestSeasonToGoesOptions;

            return View(SelectedBestSeasonToGo);
        }

        // POST: BestSeasonToGo/Update/5
        [HttpPost]
        public ActionResult Edit(int id, BestSeasonToGo BestSeasonToGo)
        {
            string url = "updateBestSeasonToGo/" + id;
            string jsonpayload = jss.Serialize(BestSeasonToGo);
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

        // GET: BestSeasonToGo/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "findBestSeasonToGo/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BestSeasonToGoDto SelectedBestSeasonToGo = response.Content.ReadAsAsync<BestSeasonToGoDto>().Result;
            return View(SelectedBestSeasonToGo);
        }

        // POST: BestSeasonToGo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "deleteBestSeasonToGo/" + id;
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