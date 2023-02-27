using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;//4-needed for HttpClient client ....
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;//8-
using passionProject1.Models;//11-
using System.Web.Script.Serialization;//20-
using System.Web.Optimization;
using passionProject1.Models.ViewModels;


namespace passionProject1.Controllers
{
    public class PlaceController : Controller
    {
        // 16- CODE FACTORING METHOD:
        //to avoid rewriting similar codes lines 18 to 22
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();// 20-

        static PlaceController()
        {
            client = new HttpClient();
            //17- code refactoring for the same uri address:
            client.BaseAddress = new Uri("https://localhost:44378/api/");
        }
        // GET: Place/list
        public ActionResult List()//1-
        {
            //objective: communicate with our place date api to retrieve a list of places//2-

            //curl https://localhost:44378/api/placesdata/listplaces //3-

            //HttpClient client = new HttpClient() { };//4- && 15 taking this line for code factoring method to avoid rewriting codes.

            string url = "placesdata/listplaces";//5-establishing URL communicating end point.

            HttpResponseMessage response = client.GetAsync(url).Result;//6-accessing to api from client .Result() to get the result
             
            Debug.WriteLine("The response code is ");//9-
            Debug.WriteLine(response.StatusCode);//7- statusCode allows us to see info if the request sucssesfully handeled.
                        
            IEnumerable<placeDto> places = response.Content.ReadAsAsync<IEnumerable<placeDto>>().Result;//11-
            //above line of code is saying take the content from the response as Async function in a IEnumerable format.
            Debug.WriteLine("Number of place recieve : ");//12-
            //Debug.WriteLine(places.Count());//13-
            Debug.WriteLine(places.Count());//13-

            return View(places);//1-
        }

        // GET: Place/Details/5
        public ActionResult Details(int id)
        {
            DetailsPlace ViewModel = new DetailsPlace();
            
            //objective: communicate with our place date api to retrieve a place// 14-
            //curl https://localhost:44378/api/placesdata/listplaces/{id}// 15-

            //HttpClient client = new HttpClient() { };//4- turn into comment for refactoring method.

            string url = "placesdata/findplace/" + id;// 16 -establishing URL communicating end point.
            HttpResponseMessage response = client.GetAsync(url).Result;//-accessing to api from client .Result() to get the result

            Debug.WriteLine("The response code is ");//-
            Debug.WriteLine(response.StatusCode);//- statusCode allows us to see info if the request sucssesfully handeled.

            placeDto Selectedplace = response.Content.ReadAsAsync<placeDto>().Result;//17-
            //above line of code is saying take the content from the response as Async function in a IEnumerable format.
            Debug.WriteLine("place recieve : ");//-
            Debug.WriteLine(Selectedplace.PlaceName);//-

            ViewModel.Selectedplace = Selectedplace;
            
            //showcase information about categories related to this places
            //send a request to gather information about categories related to a particular place ID
            HttpClient client2 = new HttpClient();
            client2.BaseAddress = new Uri("https://localhost:44378/api/");
            string url2 = "categoriesdata/listcategoriesforplaces/" + id;
            HttpResponseMessage CategoryResponse = client2.GetAsync(url2).Result;
            IEnumerable<CategoryDto> RelatedCategories = CategoryResponse.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;

            ViewModel.ResponsibleCategories = RelatedCategories;

            //showcase information about BestSeasonToGoes related to this places
            //send a request to gather information about BestSeasonToGoes related to a particular place ID
            
            HttpClient client3 = new HttpClient();
            client3.BaseAddress = new Uri("https://localhost:44378/api/");
            string url3 = "BestSeasonToGoesData/listBestSeasonToGoforplaces/" + id;
            HttpResponseMessage BestSeasonToGoResponse = client3.GetAsync(url3).Result;
            IEnumerable<BestSeasonToGoDto> RelatedBestSeasonToGoes = BestSeasonToGoResponse.Content.ReadAsAsync<IEnumerable<BestSeasonToGoDto>>().Result;

            ViewModel.ResponsibleBestSeasonToGoes = RelatedBestSeasonToGoes;
            
            return View(ViewModel);
        }

        //POST: place/Associate/{placeid}
        [HttpPost]
        public ActionResult Associate(int id, int placeID)
        {
            Debug.WriteLine("Attempting to associate place :" + id + " with category " + placeID);

            //call our api to associate place with category
            string url = "associateplacewithcategory/" + id + "/" + placeID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        //Get: place/UnAssociate/{id}?placeID={categoryID}
        [HttpGet]
        public ActionResult UnAssociate(int id, int categoryID)
        {
            Debug.WriteLine("Attempting to unassociate place :" + id + " with keeper: " + categoryID);

            //call our api to associate place with category
            string url = "unassociateplacewithcategory/" + id + "/" + categoryID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        // 24- for error then en error view created
        public ActionResult Error()
        {
            return View();
        }

        // GET: Place/New
        public ActionResult New()
        {
            string url = "placesdata/listplaces";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<placeDto> PlacesOptions = response.Content.ReadAsAsync<IEnumerable<placeDto>>().Result;

            return View(PlacesOptions);
        }

        // POST: Place/Create
        [HttpPost]
        public ActionResult Create(place place)
        {
            Debug.WriteLine("the json payload is :");
            // 19- the create post changed.
            Debug.WriteLine("the input place name is :");
            Debug.WriteLine(place.PlaceName);
            //curl -H "Content-Type:application/json" -d @place.json https://localhost:44378/api/placesdata/addplace
            string url = "placesdata/AddPlace";

            // 20- FOR CHNAGING A PLACE OBJECT INTO A JSON YOU CAN USE A METHOD NAME JavaScriptSerializer:
            string jsonpayload = jss.Serialize(place); // 20-

            Debug.WriteLine(jsonpayload);//to make sure the JavaScriptSerializer is working.

            HttpContent content = new StringContent(jsonpayload); // 21-

            content.Headers.ContentType.MediaType = "application/json"; // 22- to send the new place to database

            HttpResponseMessage response = client.PostAsync(url, content).Result;// 21- && 23
            Debug.WriteLine("status code is :");
            Debug.WriteLine(response.StatusCode);//7- statusCode allows us to see info if the request sucssesfully handeled.
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
            //return RedirectToAction("List");
        }

        // GET: Place/Edit/5 && 23- turn into a comment because it moved into the if statement above.
        public ActionResult Edit(int id)
        {
            Updateplace ViewModel = new Updateplace();
            //the existing place information
            string url = "placesdata/findplace/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            placeDto Selectedplace = response.Content.ReadAsAsync<placeDto>().Result;
            ViewModel.Selectedplace = Selectedplace;

            
            url = "categoriesdata/listcategories/";
            Debug.WriteLine(url);
            response = client.GetAsync(url).Result;
            IEnumerable<CategoryDto> CategoriesOptions = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            ViewModel.CategoriesOptions = CategoriesOptions;
            

            return View(ViewModel);
        }

        // POST: Place/Update/5
        [HttpPost]
        public ActionResult Update(int id, place place)
        {
            string url = "placesdata/updateplace/" + id;
            string jsonpayload = jss.Serialize(place);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Place/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "placesdata/findplace/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            placeDto Selectedplace = response.Content.ReadAsAsync<placeDto>().Result;
            return View(Selectedplace);
        }

        // POST: Place/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "placesdata/deleteplace/" + id;
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
