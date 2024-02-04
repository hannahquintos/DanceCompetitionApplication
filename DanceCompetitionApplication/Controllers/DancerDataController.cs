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
        //utilizing the database connection
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        ///     Returns a list of all dancers in the system
        /// </summary>
        /// <returns>
        ///     Returns all dancers in the database including their dancer id, first name, last name, and date of birth
        /// </returns>
        /// <example>
        ///     GET: api/DancerData/ListDancers
        /// </example>
        [HttpGet]
        public IEnumerable<DancerDto> ListDancers()
        {
            //select all from dancers
            List<Dancer> Dancers = db.Dancers.ToList();

            List<DancerDto> DancerDtos = new List<DancerDto>();

            Dancers.ForEach(d => DancerDtos.Add(new DancerDto()
            {
                DancerId = d.DancerId,
                FirstName = d.FirstName,
                LastName = d.LastName,
                DateOfBirth = d.DateOfBirth
            }
            ));

            return DancerDtos;
        }

        /// <summary>
        ///     Recieves a dancer id and returns the corresponding dancer
        /// </summary>
        /// <param name="id"> The primary key, dancer id (as an integer) </param>
        /// <returns>
        ///     Returns one dancer for the given id including their dancer id, first name, last name, and date of birth
        /// </returns>
        /// <example>
        ///     GET: api/DancerData/FindDancer/5
        ///         returns --> <DancerId>5</DancerId>
        ///                     <DateOfBirth>2007-06-25T00:00:00</DateOfBirth>
        ///                     <FirstName>Chiara</FirstName>
        ///                     <LastName>Carneiro</LastName>
        /// </example>
        [ResponseType(typeof(Dancer))]
        [HttpGet]
        public IHttpActionResult FindDancer(int id)
        {
            Dancer Dancer = db.Dancers.Find(id);
            DancerDto DancerDto = new DancerDto()
            {
                DancerId = Dancer.DancerId,
                FirstName = Dancer.FirstName,
                LastName = Dancer.LastName,
                DateOfBirth= Dancer.DateOfBirth
            };
            if (Dancer == null)
            {
                return NotFound();
            }

            return Ok(DancerDto);
        }

        // POST: api/DancerData/UpdateDancer/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDancer(int id, Dancer dancer)
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

        // POST: api/DancerData/AddDancer
        [ResponseType(typeof(Dancer))]
        [HttpPost]
        public IHttpActionResult AddDancer(Dancer dancer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Dancers.Add(dancer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dancer.DancerId }, dancer);
        }

        // POST: api/DancerData/DeleteDancer/5
        [ResponseType(typeof(Dancer))]
        [HttpPost]
        public IHttpActionResult DeleteDancer(int id)
        {
            Dancer dancer = db.Dancers.Find(id);
            if (dancer == null)
            {
                return NotFound();
            }

            db.Dancers.Remove(dancer);
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

        private bool DancerExists(int id)
        {
            return db.Dancers.Count(e => e.DancerId == id) > 0;
        }
    }
}