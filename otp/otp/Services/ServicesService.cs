using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;


using System.Threading.Tasks;

using otp.Models;
using Akavache;
using System.Reactive.Linq;

namespace otp.Services
{
    public interface IServicesService
    {
         Task<ObservableCollection<ServiceModel>> GetServices();
    }
  public  class ServicesService: IServicesService
    {
        string ServiceKey = "ServiceKey";
        public async Task<ObservableCollection<ServiceModel>> GetServices()
        {
            Helpers.GenericRepository oRep = new Helpers.GenericRepository();
            var services = await oRep.GetAsync<ObservableCollection<ServiceModel>>(Helpers.Utility.ServerUrDatabase + "/api/Services");

            foreach (var service in services)
            {
                service.ServiceName = (Helpers.Settings.Language == "ar") ? service.ServiceNameAr : service.ServiceName;
                // service.ServiceIcon = string.Format("{0}Content//Images/{1}", Helpers.Utility.ServerUrDatabase, service.ServiceIcon);

            }
            //await BlobCache.LocalMachine.InsertObject(ServiceKey, services,DateTimeOffset.Now.AddDays(5));
            return services;
            
        }

    }
}
