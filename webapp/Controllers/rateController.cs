using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webapp.Models;

namespace webapp.Controllers
{
    public class rateController : ApiController
    {


        ServicesEntities ctx = new ServicesEntities();

        // GET: api/rate/{providerId}
        [Route("api/rate/{providerId}")]
        public IHttpActionResult Get(int providerId)
        {
            try
            {
                // Calculate the average rating for the specified provider
                double? averageRating = ctx.TbRatings.Any(r => r.ProviderId == providerId) ?
                                         ctx.TbRatings.Where(r => r.ProviderId == providerId).Average(r => r.Rating) :
                                         null;
                var provider = ctx.TbProviders.FirstOrDefault(p => p.ProvicderId == providerId);
                if (provider != null)
                {
                    provider.ratting = averageRating;
                    ctx.SaveChanges();
                }

                // Return the average rating
                return Ok(averageRating);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return InternalServerError(ex);
            }
        }



        // GET: api/rate/5
      

        // POST: api/rate
        public string Post([FromBody]TbRating value)
        {
            try
            {



                ctx.TbRatings.Add(value);
                ctx.SaveChanges();
                return "done";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // PUT: api/rate/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/rate/5
        public void Delete(int id)
        {
        }
    }
}
