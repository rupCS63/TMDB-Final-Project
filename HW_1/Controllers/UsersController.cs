using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HW_1.Models;
using HW_1.Models.DAL;


namespace HW_1.Controllers
{
    public class UsersController : ApiController
    {
        // GET api/<controller>
        public List<User> Get() //get users table for the admin panel
        {
            User u = new User();
            return u.GetUsers();
        }

        // GET api/<controller>/mail/password
        public HttpResponseMessage Get(string mail, string password)
        {
            DataServices ds = new DataServices();
            User user = ds.validLoginFromDB( mail, password);
            if(user.Id > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            
            return Request.CreateResponse(HttpStatusCode.NotFound, "Email address or Password is incorrect");
            
        }

        // POST api/<controller>
        public int Post([FromBody] User user)
        {
            user.InsertUserToSQL();
            return 1;
        }

        // PUT api/<controller>/5
        public int Put(int id)
        {
            User u = new User();
            return u.addPoints(id);
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}