using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using webapp.Models;

namespace webapp.Controllers
{
    public class ServicesController : ApiController
    {
        // GET: api/Services
        [HttpGet]
        [Route("api/Services")]
        public IEnumerable<VwServic> Get()
        {
            ServicesEntities ctx = new ServicesEntities();
            var services = ctx.VwServics.ToList();
            return services;
        }

        // GET: api/Services/5
        public VwServic Get(int id)
        {
            ServicesEntities ctx = new ServicesEntities();
            var service = ctx.VwServics.FirstOrDefault(x => x.ServiceId == id);
            return service;
        }

        // POST: api/Services
        [HttpPost]
        [Route("api/Services")]
        public async Task<string> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var fileUploadResult = await UploadFile();
            var formData = HttpContext.Current.Request.Form;

            try
            {
                var service = new TbService
                {
                    ServiceId = Convert.ToInt32(formData["ServiceId"]),
                    ServiceName = formData["ServiceName"],
                    ServiceNameAr = formData["ServiceNameAr"],
                    ServiceIcon = fileUploadResult // Set the ServiceIcon property to the relative path
                };

                using (var ctx = new ServicesEntities())
                {
                    ctx.TbServices.Add(service);
                    ctx.SaveChanges();
                }

                return "done";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Method to handle file upload
        private async Task<string> UploadFile()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/Uploads"); // Change to ~/Uploads
            Directory.CreateDirectory(root);
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // Process the file data.
                foreach (var file in provider.FileData)
                {
                    var name = file.Headers.ContentDisposition.FileName.Trim('"');
                    var localFileName = file.LocalFileName;
                    var filePath = Path.Combine(root, name);
                    File.Move(localFileName, filePath);
                    var relativePath = "http://ashello-001-site1.ktempurl.com/"+$"Uploads/{name}"; // Create the relative path
                    return relativePath; // Return the relative path
                }

                // If no file was uploaded
                return null;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return $"Error: {ex.Message}";
            }
        }

        // PUT: api/Services/5
        public void Put(int id, [FromBody] string value)
        {
            // Implementation for updating a service
        }

        // DELETE: api/Services/5
        [HttpDelete]
        [Route("api/Services/{id}")]
        [AllowAnonymous]
        public IHttpActionResult Delete(int id)
        {
            using (var ctx = new ServicesEntities())
            {
                var item = ctx.TbServices.FirstOrDefault(a => a.ServiceId == id);
                if (item == null)
                {
                    return NotFound();
                }

                ctx.TbServices.Remove(item);
                ctx.SaveChanges();
                return Ok();
            }
        }
    }
}
