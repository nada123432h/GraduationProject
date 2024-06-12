using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Services.Protocols;
using webapp.Models;

namespace webapp.Controllers
{

    public class RequestProviderController : ApiController
    {
        // GET: api/Request

        ServicesEntities ctx = new ServicesEntities();
        public IEnumerable<VwRequestsProvider> Get()
        {
           
            var services = ctx.VwRequestsProviders.ToList();
            return services;
        }

        // GET: api/Request/5
        public IEnumerable<VwRequestsProvider> Get(int id)
        {
            var items = ctx.VwRequestsProviders.Where(item => item.ProviderId == id).ToList();
           return items;
        }
        [HttpGet]
        [Route("api/RequestProvider/GetRequestsById/{requestId}")]
        public IEnumerable<VwRequestsProvider> GetRequestsById(int requestId)
        {
            var items = ctx.VwRequestsProviders.Where(item => item.ID == requestId).ToList();
            return items;
        }



        // POST: api/Request
        public string Post([FromBody] TbOfRequest value)
        {
          
            try
            {
               // TbRequestsProvider value2 = new TbRequestsProvider();
               
                ctx.TbOfRequests.Add(value);
               // ctx.TbRequestsProviders.Add(value2);
            ctx.SaveChanges();
                return "done";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            
        }

        // PUT: api/Request/5
        // PUT: api/YourController/5
        [HttpPut]
        [Route("api/RequestProvider/Put/{id}")]
        public void Put(int id, [FromBody] TbOfRequest value)
        {
            // Your existing logic here
            var DbItem = ctx.TbOfRequests.Where(a => a.ID == id).FirstOrDefault();
            DbItem.StatusOfRequest = value.StatusOfRequest;
            DbItem.IsResponse = value.IsResponse;
            DbItem.Cost = value.Cost;
            DbItem.Date = value.Date;
            ctx.SaveChanges();
        }

        // DELETE: api/Request/5

        [HttpDelete]
        [AllowAnonymous]
        public void Delete(int id)
        {
            var item = ctx.TbOfRequests.Where(a => a.ID == id).FirstOrDefault();
            ctx.TbOfRequests.Remove(item);
            ctx.SaveChanges();
        }
    }
    }

