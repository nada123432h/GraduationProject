using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webapp.Models;

namespace webapp.Controllers
{
    public class ServiceProvidersController : ApiController
    {
        // GET: api/ServiceProviders
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ServiceProviders/5
        public IEnumerable<VwServiceProvider> Get(int id)
        {
            try
            {
                ServicesEntities ctx = new ServicesEntities();
                return ctx.VwServiceProviders.Where(a => a.ServiceId == id).ToList();
            }
            catch
            {
                return new List<VwServiceProvider>();
            }
        }
        [HttpGet]
       
        [Route("api/serviceprovider/{providerid}")]
        public IEnumerable<VwServiceProvider> Get2(int providerid)
        {
            try
            {
                ServicesEntities ctx = new ServicesEntities();
                return ctx.VwServiceProviders.Where(a => a.ServiceId == providerid).ToList();
            }
            catch
            {
                return new List<VwServiceProvider>();
            }
        }


        // POST: api/ServiceProviders
      
        public string Post([FromBody]TbServiceProvider value)
        {
            try
            {
                ServicesEntities ctx = new ServicesEntities();


                ctx.TbServiceProviders.Add(value);
                ctx.SaveChanges();
                return "done";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // PUT: api/ServiceProviders/5
        [HttpPut]
        [Route("api/ServiceProviders/Put/{providerId}")]
        public void Put(int providerId, [FromBody] TbProvider value)
        {
            try
            {
                ServicesEntities ctx = new ServicesEntities();
                var provider = ctx.TbProviders.FirstOrDefault(p => p.ProvicderId == providerId);

               

                // Update the rating
                provider.Rating = value.Rating;

                // Save changes to the database
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
            }
        }




        // DELETE: api/ServiceProviders/5
        [HttpDelete]
        [AllowAnonymous]
        public void Delete(int id)
        {
            ServicesEntities ctx = new ServicesEntities();
            var item = ctx.TbServiceProviders.Where(a => a.ProviderId == id).FirstOrDefault();
            ctx.TbServiceProviders.Remove(item);
            ctx.SaveChanges();
        }
    }
}
