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
    public interface IServicesProviders
    {
         Task<ObservableCollection<ProviderModel>> GetProviders(int serviceId);
    }
  public  class ServicesProviders : IServicesProviders
    {
        string ServiceKey = "ServiceKeyproviders";
        public async Task<ObservableCollection<ProviderModel>> GetProviders(int serviceId)
        {
            var servicesFromCache = new ObservableCollection<ProviderModel>();
            //try
            //{
            //    servicesFromCache = await BlobCache.LocalMachine.GetObject<ObservableCollection<ProviderModel>>(ServiceKey);
            //}
            //catch
            //{
            //    servicesFromCache = null;
            //}
            //if (servicesFromCache != null)
            //{
            //    return servicesFromCache;
            //}
            //else
            //{
            GenericRepository oRep = new Helpers.GenericRepository();
            var Providers = await oRep.GetAsync<ObservableCollection<ProviderModel>>(Helpers.Utility.ServerUrDatabase + string.Format("/api/ServiceProviders/{0}", serviceId));

            // foreach (var provider in Providers)
            // {

            //provider.ImageName = string.Format("{0}Content//Images/{1}", Utility.ServerUrDatabase, provider.ImageName);

            // }
            //  await BlobCache.LocalMachine.InsertObject(ServiceKey, Providers, DateTimeOffset.Now.AddMinutes(2));
            return Providers;
           // }
        }

    }
}
