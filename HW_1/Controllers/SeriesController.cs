using HW_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HW_1.Controllers
{
    public class SeriesController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Series> Get()
        {
            Series series = new Series();
            List<Series> seriesList = series.Get();
            return seriesList;
        }



        // GET api/<controller>/5
        public string Get(string genre)
        { //get series by genre
            return "Error";
        }

        // POST api/<controller>
        public int Post([FromBody] Series series)
        {
            series.InsertToSQL();
            return 1;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        // GET api/<controller>/?yearOfBirth=***,favGenre=
        public List<Series> Get(int yearOfBirth, string favGenre)
        { //get series by genre
            Series s = new Series();
            return s.getRecommendations(yearOfBirth, favGenre);
        }
    }
}