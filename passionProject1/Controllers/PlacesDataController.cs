using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using passionProject1.Models;
using System.Diagnostics;
using System.Web.UI;

namespace passionProject1.Controllers
{
    public class PlacesDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all places in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CCONTENT: all places in the database. 
        /// </returns>
        /// <example>
        /// GET: api/placesdata/listplaces
        /// </example>
        [HttpGet]
        [ResponseType(typeof(placeDto))]
        public IHttpActionResult ListPlaces()
        {
            List<place> places = db.Places.ToList();
            List<placeDto> placeDtos = new List<placeDto>();

            places.ForEach(a => placeDtos.Add(new placeDto()
            {
                PlaceID = a.PlaceID,
                PlaceName = a.PlaceName,
                //categoryID = a.category.categoryID,
                //BestSeasonToGoID = a.BestSeasonToGo.BestSeasonToGoID
            }));

            return Ok(placeDtos);
        }

        /// <summary>
        /// Gathers information about all places related to a particular category ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all places in the database, including their associated categories matched with a particular category ID
        /// </returns>
        /// <param name="id">category ID.</param>
        /// <example>
        /// GET: api/placesData/ListplacesForCategories/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(placeDto))]
        public IHttpActionResult ListPlacesforCategories(int id)
        {
            List<place> places = db.Places.Where(a => a.CategoryID == id).ToList();
            List<placeDto> placeDtos = new List<placeDto>();

            places.ForEach(a => placeDtos.Add(new placeDto()
            {
                PlaceID = a.PlaceID,
                PlaceName = a.PlaceName,
                CategoryID = a.Category.CategoryID,
                BestSeasonToGoID = a.BestSeasonToGo.BestSeasonToGoID
            }));

            return Ok(placeDtos);
        }

        /// <summary>
        /// Gathers information about all places related to a particular Best Season to go ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all places in the database, including their associated best season to go matched with a particular bestSeasonToGo ID
        /// </returns>
        /// <param name="id">BestSeasonToGo ID.</param>
        /// <example>
        /// GET: api/placesData/ListplacesForBestSeasonToGo/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(placeDto))]
        public IHttpActionResult ListPlacesforBestSeasonToGo(int id)
        {
            List<place> places = db.Places.Where(
                p => p.BestSeasonToGoID == id
                ).ToList();

            List<placeDto> placeDtos = new List<placeDto>();

            places.ForEach(a => placeDtos.Add(new placeDto()
            {
                PlaceID = a.PlaceID,
                PlaceName = a.PlaceName,
                CategoryID = a.Category.CategoryID,
                BestSeasonToGoID = a.BestSeasonToGo.BestSeasonToGoID
            }));

            return Ok(placeDtos);
        }

        /*
        /// <summary>
        /// Associates a particular category with a particular place
        /// </summary>
        /// <param name="placeID">The place ID primary key</param>
        /// <param name="categoryID">The category ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/placesData/AssociatePlaceWithCategory/3
        /// </example>
        [HttpPost]
        [Route("api/placesData/AssociatePlaceWithCategory/{placeid}/{categoryid}")]
        public IHttpActionResult AssociatePlaceWithCategory(int placeid, int categoryid)
        {

            place Selectedplace = db.Places.Include(a => a.Category).Where(a => a.PlaceID == placeid).FirstOrDefault();
            Category Selectedcategory = db.Categories.Find(categoryid);

            if (Selectedplace == null || Selectedcategory == null)
            {
                return NotFound();
            }

            Selectedplace.Category.Add(Selectedcategory);
            db.SaveChanges();

            return Ok();
        }
        */


        /*
        /// <summary>
        /// removes an Associates between a particular category with a particular place
        /// </summary>
        /// <param name="placeID">The place ID primary key</param>
        /// <param name="categoryID">The category ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/placesData/AssociatePlaceWithCategory/3/1
        /// </example>
        [HttpPost]
        [Route("api/placesData/AssociatePlaceWithCategory/{placeid}/{categoryid}")]
        public IHttpActionResult UnAssociatePlaceWithCategory(int placeid, int categoryid)
        {

            place Selectedplace = db.Places.Include(a => a.Category).Where(a => a.PlaceID == placeid).FirstOrDefault();
            Category Selectedcategory = db.Categories.Find(categoryid);

            if (Selectedplace == null || Selectedcategory == null)
            {
                return NotFound();
            }

            Selectedplace.Categories.Remove(Selectedcategory);
            db.SaveChanges();

            return Ok();
        }
        */

        /// <summary>
        /// returns all places related to the system
        /// </summary>
        /// <param name="id">a primary key for a place</param>
        /// <returns>
        /// HEADER: 200(ok)
        ///CONTENT: An place in the system matching up to the place ID primary key
        /// </returns>
        /// <example>
        /// GET: api/placesdata/findplaces/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(placeDto))]
        public IHttpActionResult FindPlace(int id)
        {
            place place = db.Places.Find(id);
            placeDto placeDto = new placeDto()
            {
                PlaceID = place.PlaceID,
                PlaceName = place.PlaceName,
                //BestSeasonToGoID = BestSeasonToGo.BestSeasonToGoID,
                //categoryID = category.categoryID
            };
            if (place == null) 
            {
                return NotFound();
            }

            return Ok(placeDto);
        }

        // POST: api/PlacesData/UpdatePlaces/5

        /// <summary>
        /// update a particular place in the system with POST data input
        /// </summary>
        /// <param name="id"></param>represents the placeID primary key
        /// <param name="place">JSON FORM DATA of an place</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/PlacesData/UpdatePlaces/5
        /// FROM DATA: place JSON object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        //[Route("api/PlacesData/UpdatePlace/{id}")]
        public IHttpActionResult UpdatePlace(int id, place place)
        {
            Debug.WriteLine("I have reached the update place method!");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != place.PlaceID)
            {
                Debug.WriteLine("ID mismatch!");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + place.PlaceID);
                Debug.WriteLine("POST parameter" + place.PlaceName);
                Debug.WriteLine("POST parameter" + place.BestSeasonToGoID);
                Debug.WriteLine("POST parameter" + place.CategoryID);
                Debug.WriteLine("ID mismatch!");

                return BadRequest();
            }

            db.Entry(place).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!placeExists(id))
                {
                    Debug.WriteLine("place not found!");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Debug.WriteLine("none of the conditions triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }
        /*steps to update from windows comand prompt:
        1-cd to the folder path of the .JSON file which in this case is:C:\Users\mitic\source\repos\passionProject1\passionProject1\jsondata>
        2-update the json file
        3-C:\Users\mitic\source\repos\passionProject1\passionProject1\jsondata>curl -d @place.json -H "Content-type:application/json" "https://localhost:44378/api/PlacesData/UpdatePlace/11" 
        4-C:\Users\mitic\source\repos\passionProject1\passionProject1\jsondata>curl "https://localhost:44378/api/PlacesData/FindPlace/11"
        5- the 11 at the end is the placeID

        THE DEBUGGING PROCESS:
        1-add this using: using System.Diagnostics;
        2-write this code on any lines you re not sure about: Debug.WriteLine("ID mismatch!");
        3-when you run the server check the output
        *
        *
        ***CHECK THIS UPDATE HOW MANY TIME THAT I TRY THE DEBUGGING METHOD***
        *
        *
        */



        /// <summary>
        /// Adds an place to the system
        /// </summary>
        /// <param name="place">JSON FORM DATA of a place</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Animal ID, Animal Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        ///<example>
        ///POST: api/PlacesData/AddPlace
        ///FORM DATA: place JSON Object
        ///</example>
        [ResponseType(typeof(place))]
        [HttpPost]
        [Route("api/PlacesData/AddPlace")]
        public IHttpActionResult AddPlace(place place)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Places.Add(place);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = place.PlaceID }, place);
        }
        /*steps to add from windows comand prompt:
        1-cd to the folder path of the .JSON file which in this case is:C:\Users\mitic\source\repos\passionProject1\passionProject1\jsondata>
        2-C:\Users\mitic\source\repos\passionProject1\passionProject1\jsondata>curl https://localhost:44378/api/placesdata/listplace
        3-C:\Users\mitic\source\repos\passionProject1\passionProject1\jsondata>curl -d @place.json -H "Content-type:application/json" https://localhost:44378/api/placesdata/addplace
        */



        /// <summary>
        /// delete a place from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the place</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/PlacesData/DeletePlace/5
        ///  FROM DATA: (empty)
        /// </example>
        [ResponseType(typeof(place))]
        [HttpPost]
        [Route("api/PlacesData/Deleteplace/{id}")]
        public IHttpActionResult Deleteplace(int id)
        {
            place place = db.Places.Find(id);
            if (place == null)
            {
                return NotFound();
            }

            db.Places.Remove(place);
            db.SaveChanges();

            return Ok();
        }
        /* STEPS TO DELETE THROUGHT THE COMAND PROMPT:
        1-cd to the fplder path of the .JSON file which in this case is C:\Users\mitic\source\repos\passionProject1\passionProject1\jsondata>
        2-C:\Users\mitic\source\repos\passionProject1\passionProject1\jsondata>curl -d "" https://localhost:44378/PlacesData/Deleteplace/27
        3- 27 at the end is the PlaceID 
        */



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool placeExists(int id)
        {
            return db.Places.Count(e => e.PlaceID == id) > 0;
        }
    }
}