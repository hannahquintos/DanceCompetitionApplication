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
using DanceCompetitionApplication.Models;
using System.Diagnostics;

namespace DanceCompetitionApplication.Controllers
{
    public class CategoryDataController : ApiController
    {
        //utilizing the database connection
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        ///     Returns a list of all categories in the system
        /// </summary>
        /// <returns>
        ///     Returns all categories in the database including their category id and category name
        /// </returns>
        /// <example>
        ///     GET: api/CategoryData/ListCategories
        /// </example>
        [HttpGet]
        public IEnumerable<CategoryDto> ListCategories()
        {
            //select all from categories
            List<Category> Categories = db.Categories.ToList();

            List<CategoryDto> CategoryDtos = new List<CategoryDto>();

            Categories.ForEach(c => CategoryDtos.Add(new CategoryDto()
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName
            }
            ));

            return CategoryDtos;
        }

        /// <summary>
        ///     Recieves a category id and returns the corresponding category
        /// </summary>
        /// <param name="id"> The category's primary key, category id (as an integer) </param>
        /// <returns>
        ///     Returns one category for the given id including its category id and category name
        /// </returns>
        /// <example>
        ///     GET: api/CategoryData/FindCategory/5
        ///         returns --> <CategoryId>5</CategoryId>
        ///                     <CategoryName>Teen Tap Group</CategoryName>
        /// </example>
        [ResponseType(typeof(Category))]
        [HttpGet]
        public IHttpActionResult FindCategory(int id)
        {
            Category Category = db.Categories.Find(id);
            CategoryDto CategoryDto = new CategoryDto()
            {
                CategoryId = Category.CategoryId,
                CategoryName = Category.CategoryName
            };
            if (Category == null)
            {
                return NotFound();
            }

            return Ok(CategoryDto);
        }

        /// <summary>
        ///     Recieves a category id and the updated information about a category, then updates the category's information in the system with the data input
        /// </summary>
        /// <param name="id"> The category's primary key, category id (as an integer) of the category to update </param>
        /// <param name="category"> Updated information about a category (category object as JSON FORM DATA)
        ///                            - properties of category object include category id, category name
        /// </param>
        /// <returns>
        ///     HEADER: 200 (Success, No Content Response)
        ///         or
        ///     HEADER: 400 (Bad Request)
        ///         or
        ///     HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        ///   POST: api/CategoryData/UpdateCategory/5
        ///   FORM DATA: Category JSON object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //server side validation

            if (string.IsNullOrEmpty(category.CategoryName))
            {
                return BadRequest(ModelState);
            }

            if (id != category.CategoryId)
            {
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
        ///     Recieves information for a new category, adds the new category and its information to the system
        /// </summary>
        /// <param name="category"> New category and its information (category object as JSON FORM DATA)
        ///                            - properties of category object include category id, category name
        /// </param>        
        /// <returns>
        ///     HEADER: 201 (Created)
        ///     CONTENT: Category Id, Category Data
        ///         or
        ///     HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        ///     POST: api/CategoryData/AddCategory
        ///     FORM DATA: Category JSON object
        /// </example>
        [ResponseType(typeof(Category))]
        [HttpPost]
        public IHttpActionResult AddCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //server side validation

            if (category.CategoryId <= 0 || string.IsNullOrEmpty(category.CategoryName))
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.CategoryId }, category);
        }

        /// <summary>
        ///     Recieves a category id and deletes the corresponding category from the system
        /// </summary>
        /// <param name="id"> The category's primary key, category id (as an integer) </param>
        /// <returns>
        ///     HEADER: 200 (Ok)
        ///         or
        ///     HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        ///     POST: api/CategoryData/DeleteCategory/5
        ///     FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Category))]
        [HttpPost]
        public IHttpActionResult DeleteCategory(int id)
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
            return db.Categories.Count(e => e.CategoryId == id) > 0;
        }
    }
}