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
        ///     Returns a list of all dancers in the system related to a particular performance
        /// </summary>
        /// <returns>
        ///     Returns all dancers in the database related to a particular performance id including their dancer id, first name, last name, and date of birth
        /// </returns>
        /// <param name="id"> The performances's primary key, performance id (as an integer) </param>
        /// <example>
        ///     GET: api/DancerData/ListDancersForPerformance/2
        /// </example>
        [HttpGet]
        public IEnumerable<DancerDto> ListDancersForPerformance(int id)
        {
            //select all from dancers
            List<Dancer> Dancers = db.Dancers.Where(
                d=>d.DancerPerformances.Any(
                    p=>p.PerformanceId==id)
                ).ToList();

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
        ///     Returns a list of all dancers in the system not related to a particular performance
        /// </summary>
        /// <returns>
        ///     Returns all dancers in the database not related to a particular performance id including their dancer id, first name, last name, and date of birth
        /// </returns>
        /// <param name="id"> The performances's primary key, performance id (as an integer) </param>
        /// <example>
        ///     GET: api/DancerData/ListDancersNotInPerformance/2
        /// </example>
        [HttpGet]
        public IEnumerable<DancerDto> ListDancersNotInPerformance(int id)
        {
            //select all from dancers
            List<Dancer> Dancers = db.Dancers.Where(
                d => !d.DancerPerformances.Any(
                    p => p.PerformanceId == id)
                ).ToList();

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
        /// <param name="id"> The dancer's primary key, dancer id (as an integer) </param>
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

        /// <summary>
        ///     Recieves a dancer id and the updated information about a dancer, then updates the dancer's information in the system with the data input
        /// </summary>
        /// <param name="id"> The dancer's primary key, dancer id (as an integer) of the dancer to update </param>
        /// <param name="dancer"> Updated information about a dancer (dancer object as JSON FORM DATA)
        ///                            - properties of dancer object include dancer id, first name, last name, date of birth
        /// </param>
        /// <returns>
        ///     HEADER: 200 (Success, No Content Response)
        ///         or
        ///     HEADER: 400 (Bad Request)
        ///         or
        ///     HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        ///   POST: api/DancerData/UpdateDancer/5
        ///   FORM DATA: Dancer JSON object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDancer(int id, Dancer dancer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //server side validation

            /*Debug.WriteLine("Dancer date of birth is " + dancer.DateOfBirth);
            Debug.WriteLine("DateTime min value is " + DateTime.MinValue);*/

            if (string.IsNullOrEmpty(dancer.FirstName) || string.IsNullOrEmpty(dancer.LastName) || dancer.DateOfBirth == DateTime.MinValue)
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

        /// <summary>
        ///     Recieves information for a new dancer, adds the new dancer and its information to the system
        /// </summary>
        /// <param name="dancer"> New dancer and its information (dancer object as JSON FORM DATA)
        ///                            - properties of dancer object include dancer id, first name, last name, date of birth
        /// </param>        
        /// <returns>
        ///     HEADER: 201 (Created)
        ///     CONTENT: Dancer Id, Dancer Data
        ///         or
        ///     HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        ///     POST: api/DancerData/AddDancer
        ///     FORM DATA: Dancer JSON object
        /// </example>
        [ResponseType(typeof(Dancer))]
        [HttpPost]
        public IHttpActionResult AddDancer(Dancer dancer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //server side validation

            /*Debug.WriteLine("Dancer date of birth is " + dancer.DateOfBirth);
            Debug.WriteLine("DateTime min value is " + DateTime.MinValue);*/

            if (dancer.DancerId <= 0 || string.IsNullOrEmpty(dancer.FirstName) || string.IsNullOrEmpty(dancer.LastName) || dancer.DateOfBirth == DateTime.MinValue)
            {
                return BadRequest(ModelState);
            }

            db.Dancers.Add(dancer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dancer.DancerId }, dancer);
        }

        /// <summary>
        ///     Recieves a dancer id and deletes the corresponding dancer from the system
        /// </summary>
        /// <param name="id"> The dancer's primary key, dancer id (as an integer) </param>
        /// <returns>
        ///     HEADER: 200 (Ok)
        ///         or
        ///     HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        ///     POST: api/DancerData/DeleteDancer/5
        ///     FORM DATA: (empty)
        /// </example>
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