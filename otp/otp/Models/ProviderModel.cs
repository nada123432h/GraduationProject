using System;
using System.Collections.Generic;
using System.Text;

namespace otp.Models
{
  public   class ProviderModel
    {
        public int ProvicderId { get; set; }
        public string ProviderName { get; set; }
        public string Area { get; set; }
        public string Description { get; set; }
        public int userId { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public decimal ServiceCost { get; set; }
        public int ServiceId { get; set; }
        public string ImageName { get; set; }
        public string ServiceName { get; set; }
        public string ServiceNameAr { get; set; }
        public string ServiceIcon { get; set; }
        public Nullable<double> Rating { get; set; }
         public Nullable<double> ratting { get; set; }
    }
}
