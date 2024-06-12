using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webapp.Models;

namespace webapp.Controllers
{
    public class ProviderController : ApiController
    {
        // GET: api/User
        ServicesEntities ctx = new ServicesEntities();
        public IEnumerable<TbProvider> Get()
        {
            var users = ctx.TbProviders.ToList();
            return users;
        }

        // GET: api/User/5
        public IEnumerable<TableOfProvider> Get(int id)
        {
            var items = ctx.TableOfProviders.Where(item => item.providerid == id).ToList();
            return items;
        }

        // POST: api/User
        public string Post([FromBody] TbProvider value)
        {
            try
            {
               
              
               
                ctx.TbProviders.Add(value);
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
        public void Delete(int id)
        {

            ServicesEntities ctx = new ServicesEntities();
            var item = ctx.TbProviders.Where(a => a.ProvicderId == id).FirstOrDefault();
            ctx.TbProviders.Remove(item);
            ctx.SaveChanges();
        }
    }
}
