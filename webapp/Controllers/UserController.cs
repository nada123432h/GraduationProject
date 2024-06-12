using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webapp.Models;

namespace webapp.Controllers
{
    public class UserController : ApiController
    {
        // GET: api/User
        ServicesEntities ctx = new ServicesEntities();
        public IEnumerable<TbUser> Get()
        {
            var users = ctx.TbUsers.ToList();
            return users;
        }

        // GET: api/User/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/User
        public string Post([FromBody] TbUser value)
        {
            try
            {
               
              
               
                ctx.TbUsers.Add(value);
                ctx.SaveChanges();
                return "done";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/User/5
        [HttpDelete]
        [AllowAnonymous]
        public void Delete(int id)
        {
            var item = ctx.TbUsers.Where(a => a.CustomerId == id).FirstOrDefault();
            ctx.TbUsers.Remove(item);
            ctx.SaveChanges();
        }
        [HttpDelete]

        [Route("api/user/DeleteAll")]
        [AllowAnonymous]
        public void Delete()
        {
            var allUsers = ctx.TbUsers.ToList();
            ctx.TbUsers.RemoveRange(allUsers);
            ctx.SaveChanges();
        }
    }
}
