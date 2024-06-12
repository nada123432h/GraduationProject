using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;


using System.Threading.Tasks;

using otp.Models;
using Akavache;
using System.Reactive.Linq;
using otp.Helpers;

namespace otp.Services
{
    public interface IRequestService
    {
        Task PostService(RequestModel model);
    }
  public  class RequestService : IRequestService
    {
        string ServiceKey = "ServiceKey";
        public async Task PostService(RequestModel model)
        {
            GenericRepository oRep = new Helpers.GenericRepository();
            await oRep.PostAsync(string.Format("{0}/api/Request", Helpers.Utility.ServerUrl), model);


        }

    }
}
