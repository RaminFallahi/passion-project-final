using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using passionProject1.Models;
using passionProject1.Models.ViewModels;
using System.Diagnostics;
using passionProject1.Migrations;


namespace passionProject1.Controllers
{
    public class BestSeasonToGoesDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Returns all seasons in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all seasons to go in the database, including their associated places.
        /// </returns>
        /// <example>
        /// GET: api/BestSeasonToGoesData/listBestSeasonToGoes
        /// </example>
        [HttpGet]
        [ResponseType(typeof(BestSeasonToGoDto))]
        public IHttpActionResult ListBestSeasonsToGo()
        {
            List<Models.BestSeasonToGo> BestSeasonToGoes = db.BestSeasonsToGo.ToList();
            List<BestSeasonToGoDto> BestSeasonToGoDto = new List<BestSeasonToGoDto>();

            BestSeasonToGoes.ForEach(a => BestSeasonToGoDto.Add(new BestSeasonToGoDto()
            {
                BestSeasonToGoID = a.BestSeasonToGoID,
                Season = a.Season,
            }));

            return Ok(BestSeasonToGoDto);
        }

        /// <summary>
        /// Gathers information about all best seasons to go related to a particular place ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all  best seasons to go in the database, including their associated  best seasons to go matched with a particular place ID
        /// </returns>
        /// <param name="id">place ID.</param>
        /// <example>
        /// GET: api/BestSeasonToGoesData/ListplacesForBestSeasonToGoes/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(BestSeasonToGoDto))]
        public IHttpActionResult ListBestSeasonToGoforPlaces(int id)
        {
            List<Models.BestSeasonToGo> BestSeasonToGoes = db.BestSeasonsToGo.Where(
                b => b.places.Any(
                    p => p.PlaceID == id)).ToList();
            List<BestSeasonToGoDto> BestSeasonToGoDto = new List<BestSeasonToGoDto>();

            BestSeasonToGoes.ForEach(a => BestSeasonToGoDto.Add(new BestSeasonToGoDto()
            {
                BestSeasonToGoID = a.BestSeasonToGoID,
                Season = a.Season,
            }));

            return Ok(BestSeasonToGoDto);
        }

        /// <summary>
        /// Returns all seasons in the system associated with a specific place.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An best season in the system matching up to the season ID primary key
        /// </returns>
        /// <example>
        // GET: api/BestSeasonToGoesData/findBestSeasonToGo/5
        /// </example>
        [ResponseType(typeof(BestSeasonToGoDto))]
        [HttpGet]
        public IHttpActionResult FindGetBestSeasonToGo(int id)
        {
            Models.BestSeasonToGo BestSeasonToGo = db.BestSeasonsToGo.Find(id);
            BestSeasonToGoDto BestSeasonToGoDto = new BestSeasonToGoDto()
            {
                BestSeasonToGoID = BestSeasonToGo.BestSeasonToGoID,
                Season = BestSeasonToGo.Season,
            };
            if (BestSeasonToGo == null)
            {
                return NotFound();
            }

            return Ok(BestSeasonToGoDto);
        }

        /// <summary>
        /// Updates a particular best season in the system with POST Data input
        /// </summary>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// CONTENT: all seasons to go in the database, including their associated places.
        /// </returns>
        /// <example>
        /// Post: api/BestSeasonToGoesData/updateBestSeasonToGo/5
        /// FORM DATA: BestSeasonToGo JSON Object
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateBestSeasonToGo(int id, Models.BestSeasonToGo bestSeasonToGo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bestSeasonToGo.BestSeasonToGoID)
            {
                return BadRequest();
            }

            db.Entry(bestSeasonToGo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BestSeasonToGoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds an BestSeasonToGo to the system
        /// </summary>
        /// <param name="BestSeasonToGo">JSON FORM DATA of an BestSeasonToGo</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: BestSeasonToGoes ID, BestSeasonToGoesData
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        // POST: api/BestSeasonToGoesData
        /// FORM DATA: BestSeasonToGoes JSON Object
        /// </example>
        [ResponseType(typeof(Models.BestSeasonToGo))]
        [HttpPost]
        public IHttpActionResult AddBestSeasonToGo(Models.BestSeasonToGo bestSeasonToGo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BestSeasonsToGo.Add(bestSeasonToGo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = bestSeasonToGo.BestSeasonToGoID }, bestSeasonToGo);
        }

        /// <summary>
        /// Deletes an BestSeasonToGo from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the BestSeasonToGo</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // DELETE: api/BestSeasonToGoesData/deleteBestSeasonToGoesData/5
        /// FORM DATA: (empty)
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Models.BestSeasonToGo))]
        public IHttpActionResult DeleteBestSeasonToGo(int id)
        {
            Models.BestSeasonToGo bestSeasonToGo = db.BestSeasonsToGo.Find(id);
            if (bestSeasonToGo == null)
            {
                return NotFound();
            }

            db.BestSeasonsToGo.Remove(bestSeasonToGo);
            db.SaveChanges();

            return Ok(bestSeasonToGo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BestSeasonToGoExists(int id)
        {
            return db.BestSeasonsToGo.Count(e => e.BestSeasonToGoID == id) > 0;
        }
    }
}