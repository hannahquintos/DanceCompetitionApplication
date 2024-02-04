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
    public class DancerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DancerData
        public IQueryable<Dancer> GetDancers()
        {
            return db.Dancers;
        }

        // GET: api/DancerData/5
        [ResponseType(typeof(Dancer))]
        public IHttpActionResult GetDancer(int id)
        {
            Dancer dancer = db.Dancers.Find(id);
            if (dancer == null)
            {
                return NotFound();
            }

            return Ok(dancer);
        }

        // PUT: api/DancerData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDancer(int id, Dancer dancer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dancer.DancerId)
            {
                return BadRequest();
            }

            db.Entry(dancer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DancerExists(id))
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

        // POST: api/DancerData
        [ResponseType(typeof(Dancer))]
        public IHttpActionResult PostDancer(Dancer dancer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Dancers.Add(dancer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dancer.DancerId }, dancer);
        }

        // DELETE: api/DancerData/5
        [ResponseType(typeof(Dancer))]
        public IHttpActionResult DeleteDancer(int id)
        {
            Dancer dancer = db.Dancers.Find(id);
            if (dancer == null)
            {
                return NotFound();
            }

            db.Dancers.Remove(dancer);
            db.SaveChanges();

            return Ok(dancer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DancerExists(int id)
        {
            return db.Dancers.Count(e => e.DancerId == id) > 0;
        }
    }
}