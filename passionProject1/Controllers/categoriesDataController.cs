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

namespace passionProject1.Controllers
{
    public class CategoriesDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all categories in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all categories in the database, including their associated categories.
        /// </returns>
        /// <example>
        /// GET: api/categoriesData/listcategories
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CategoryDto))]
        public IHttpActionResult ListCategories()
        {
            List<Category> Categories = db.Categories.ToList();
            List<CategoryDto> categoryDtos = new List<CategoryDto>();

            Categories.ForEach(a => categoryDtos.Add(new CategoryDto()
            {
                CategoryID = a.CategoryID,
                CategoryName = a.CategoryName,
            }));

            return Ok(categoryDtos);
        }

        /// <summary>
        /// Gathers information about all categories related to a particular place ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all categories in the database, including their associated places matched with a particular place ID
        /// </returns>
        /// <param name="id">place ID.</param>
        /// <example>
        /// GET: api/categoriesData/ListCategoriesforPlaces/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CategoryDto))]
        public IHttpActionResult ListCategoriesforPlaces(int id)
        {
            List<Category> categories = db.Categories.Where(
                c => c.places.Any(
                 p => p.PlaceID == id)).ToList();
            List<CategoryDto> CategoryDto = new List<CategoryDto>();

            categories.ForEach(a => CategoryDto.Add(new CategoryDto()
            {
                CategoryID = a.CategoryID,
                CategoryName = a.CategoryName
            }));

            return Ok(CategoryDto);
        }

        /// <summary>
        /// Returns all categories in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all categories in the database, including their associated categories.
        /// </returns>
        /// <example>
        /// GET: api/categoriesData/find/5
        /// </example>
        [ResponseType(typeof(CategoryDto))]
        [HttpGet]
        public IHttpActionResult Findcategory(int id)
        {
            Category category = db.Categories.Find(id);
            CategoryDto CategoryDto = new CategoryDto()
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName
            };
            if (category == null)
            {
                return NotFound();
            }

            return Ok(CategoryDto);
        }

        /// <summary>
        /// Updates a particular category in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Category ID primary key</param>
        /// <param name="Categories">JSON FORM DATA of an Category</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// Post: api/categoriesData/update/5
        /// FORM DATA: Category JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpGet]
        public IHttpActionResult Updatecategory(int id, Category category)
        {
            Debug.WriteLine("reached the update category method");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.CategoryID)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + category.CategoryID);
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    Debug.WriteLine("category not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Debug.WriteLine("Non of the conditions triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add an Category to the system
        /// </summary>
        /// <param name="Categories">JSON FORM DATA of an Category</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Category ID, Category Data
        /// or
        /// HEADER: 400 (Bad Request)        
        /// </returns>
        /// <example>
        /// POST: api/categoriesData/addcategory
        /// FORM DATA: Category JSON Object
        /// </example>
        [ResponseType(typeof(Category))]
        [HttpPost]
        public IHttpActionResult Addcategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.CategoryID }, category);
        }

        /// <summary>
        /// Deletes an category from the system by it's ID.
        /// </summary>
        /// <param name="id">the primary key of Category</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (not found)        
        /// </returns>
        /// <example>
        // DELETE: api/categoriesData/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Category))]
        [HttpPost]
        public IHttpActionResult Deletecategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CategoryID == id) > 0;
        }
    }
}