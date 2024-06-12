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

    public class RequestController : ApiController
    {
        // GET: api/Request

        ServicesEntities ctx = new ServicesEntities();
        public IEnumerable<TbOfRequest> Get()
        {
           
            var services = ctx.TbOfRequests.ToList();
            return services;
        }

        // GET: api/Request/5
        public IEnumerable<TbOfRequest> Get(int id)
        {
            var items = ctx.TbOfRequests.Where(item => item.CustomerId == id).ToList();
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
        //public async Task<string> Post()
        //{
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }

        //    var fileUploadResult = await UploadFile();
        //    var formData = HttpContext.Current.Request.Form;

        //    try
        //    {
        //        var request = new TbOfRequest
        //        {
        //            // Assign values from form data
        //            //ID = Convert.ToInt32(formData["ID"]),

        //            ProviderId = Convert.ToInt32(formData["ProviderId"]),
        //            ServiceId = Convert.ToInt32(formData["ServiceId"]),
        //            Descriptions = formData["Descriptions"],
        //            ImageNameProblem = fileUploadResult,
        //            ServiceName = formData["ServiceNameAr"],

        //            ProviderName = formData["ProviderName"],
        //            Date = Convert.ToDateTime(formData["Date"]),
        //            Cost = Convert.ToDecimal(formData["Cost"]),
        //            Address = formData["Address"],
        //            StatusOfRequest = Convert.ToBoolean(formData["StatusOfRequest"]), // Assuming StatusOfRequest is not nullable
        //            CustomerId = Convert.ToInt32(formData["CustomerId"]),
        //            IsResponse = !string.IsNullOrEmpty(formData["IsResponse"]) ? Convert.ToBoolean(formData["IsResponse"]) : false // Handle null value for IsResponse
        //        };

        //        // Check if ImageNameProblem is uploaded
        //        if (fileUploadResult != null)
        //        {
        //            request.ImageNameProblem = fileUploadResult;
        //        }

        //        using (var ctx = new ServicesEntities())
        //        {
        //            ctx.TbOfRequests.Add(request);
        //            ctx.SaveChanges();
        //        }

        //        return "done";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}


        // PUT: api/Request/5
        public void Put(int id, [FromBody]string value)
        {
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

