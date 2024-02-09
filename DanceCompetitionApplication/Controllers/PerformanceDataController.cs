using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DanceCompetitionApplication.Models;
using Microsoft.Ajax.Utilities;

namespace DanceCompetitionApplication.Controllers
{
    public class PerformanceDataController : ApiController
    {
        //utilizing the database connection
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        ///     Returns a list of all performances in the system
        /// </summary>
        /// <returns>
        ///     Returns all performances in the database including their performance id, performance time, routine name, studio, and category name
        /// </returns>
        /// <example>
        ///     GET: api/PerformanceData/ListPerformances
        /// </example>
        [HttpGet]
        public IEnumerable<PerformanceDto> ListPerformances()
        {
            //select all from performances
            List<Performance> Performances = db.Performances.ToList();

            List<PerformanceDto> PerformanceDtos = new List<PerformanceDto>();

            Performances.ForEach(p => PerformanceDtos.Add(new PerformanceDto()
            {
                PerformanceId = p.PerformanceId,
                PerformanceTime = p.PerformanceTime,
                RoutineName = p.RoutineName,
                Studio = p.Studio,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.CategoryName
            }
            ));

            return PerformanceDtos;
        }

        /// <summary>
        ///     Recieves a performance id and returns the corresponding performance
        /// </summary>
        /// <param name="id"> The performance's primary key, performance id (as an integer) </param>
        /// <returns>
        ///     Returns one performance for the given id including its performance id, performance time, routine name, studio, category id, and category name
        /// </returns>
        /// <example>
        ///     GET: api/PerformanceData/FindPerformance/5
        ///         returns --> <CategoryId>5</CategoryId>
        ///                     <CategoryName>Teen Tap Group</CategoryName>
        ///                     <PerformanceId>5</PerformanceId>
        ///                     <PerformanceTime>2024-03-09T09:22:00</PerformanceTime>
        ///                     <RoutineName>The Verdict</RoutineName>
        ///                     <Studio>Elite Danceworx</Studio>
        /// </example>
        [ResponseType(typeof(Performance))]
        [HttpGet]
        public IHttpActionResult FindPerformance(int id)
        {
            Performance Performance = db.Performances.Find(id);
            PerformanceDto PerformanceDto = new PerformanceDto()
            {
                PerformanceId = Performance.PerformanceId,
                PerformanceTime = Performance.PerformanceTime,
                RoutineName = Performance.RoutineName,
                Studio = Performance.Studio,
                CategoryId = Performance.CategoryId,
                CategoryName = Performance.Category.CategoryName
            };
            if (Performance == null)
            {
                return NotFound();
            }
            Debug.WriteLine("performance time: " + Performance.PerformanceTime);
            return Ok(PerformanceDto);
        }

        /// <summary>
        ///     Recieves a performance id and the updated information about a performance, then updates the performance's information in the system with the data input
        /// </summary>
        /// <param name="id"> The performance's primary key, performance id (as an integer) of the performance to update </param>
        /// <param name="performance"> Updated information about a performance (performance object as JSON FORM DATA)
        ///                            - properties of performance object include performance id, performance time, routine name, studio, category id
        /// </param>
        /// <returns>
        ///     HEADER: 200 (Success, No Content Response)
        ///         or
        ///     HEADER: 400 (Bad Request)
        ///         or
        ///     HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        ///   POST: api/PerformanceData/UpdatePerformance/5
        ///   FORM DATA: Performance JSON object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePerformance(int id, Performance performance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //server side validation

            /*Debug.WriteLine("Performance time is " + performance.PerformanceTime);
            Debug.WriteLine("DateTime min value is " + DateTime.MinValue);*/

            if (string.IsNullOrEmpty(performance.RoutineName) || performance.PerformanceTime == DateTime.MinValue || string.IsNullOrEmpty(performance.Studio) || performance.CategoryId <= 0)
            {
                return BadRequest(ModelState);
            }

            if (id != performance.PerformanceId)
            {
                return BadRequest();
            }

            db.Entry(performance).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PerformanceExists(id))
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
        ///     Recieves information for a new performance, adds the new performance and its information to the system
        /// </summary>
        /// <param name="performance"> New performance and its information (performance object as JSON FORM DATA)
        ///                            - properties of performance object include performance id, performance time, routine name, studio, category id
        /// </param>        
        /// <returns>
        ///     HEADER: 201 (Created)
        ///     CONTENT: Performance Id, Performance Data
        ///         or
        ///     HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        ///     POST: api/PerformanceData/AddPerformance
        ///     FORM DATA: Performance JSON object
        /// </example>
        [ResponseType(typeof(Performance))]
        [HttpPost]
        public IHttpActionResult AddPerformance(Performance performance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //server side validation

            /*Debug.WriteLine("Performance time is " + performance.PerformanceTime);
            Debug.WriteLine("DateTime min value is " + DateTime.MinValue);*/

            if (performance.PerformanceId <=0 || string.IsNullOrEmpty(performance.RoutineName) || performance.PerformanceTime == DateTime.MinValue || string.IsNullOrEmpty(performance.Studio) || performance.CategoryId <= 0)
            {
                return BadRequest(ModelState);
            }

            db.Performances.Add(performance);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = performance.PerformanceId }, performance);
        }

        /// <summary>
        ///     Recieves a performance id and deletes the corresponding performance from the system
        /// </summary>
        /// <param name="id"> The performance's primary key, performance id (as an integer) </param>
        /// <returns>
        ///     HEADER: 200 (Ok)
        ///         or
        ///     HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        ///     POST: api/PerformanceData/DeletePerformance/5
        ///     FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Performance))]
        [HttpPost]
        public IHttpActionResult DeletePerformance(int id)
        {
            Performance performance = db.Performances.Find(id);
            if (performance == null)
            {
                return NotFound();
            }

            db.Performances.Remove(performance);
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

        private bool PerformanceExists(int id)
        {
            return db.Performances.Count(e => e.PerformanceId == id) > 0;
        }
    }
}