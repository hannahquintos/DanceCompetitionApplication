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

namespace DanceCompetitionApplication.Controllers
{
    public class PerformanceDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PerformanceData/ListPerformances
        [HttpGet]
        public IEnumerable<PerformanceDto> ListPerformances()
        {
            //send query to database


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

        // GET: api/PerformanceData/FindPerformance/5
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

            return Ok(PerformanceDto);
        }

        // POST: api/PerformanceData/UpdatePerformance/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePerformance(int id, Performance performance)
        {
            if (!ModelState.IsValid)
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

        // POST: api/PerformanceData/AddPerformance
        [ResponseType(typeof(Performance))]
        [HttpPost]
        public IHttpActionResult AddPerformance(Performance performance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Performances.Add(performance);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = performance.PerformanceId }, performance);
        }

        // POST: api/PerformanceData/DeletePerformance/5
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