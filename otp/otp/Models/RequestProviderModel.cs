using System;
using System.Collections.Generic;
using System.Text;

namespace otp.Models
{
  public  class RequestProviderModel
    {
        public int CustomerId { get; set; }
          public int ID { get; set; }
        public int ProviderId { get; set; }
        public string Descriptions { get; set; }
        public object ImageNameProblem { get; set; }
        public DateTime Date { get; set; }
     public DateTime Date2 { get; set; }
        public bool IsChoose { get; set; }
        public bool IsConfirmed { get; set; }
        public decimal Cost { get; set; }
        public decimal UpdatedCost { get; set; }
        public string CustomerName { get; set; }
        public string statusLabel { get; set; }

        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public bool? StatusOfRequest { get; set; } // Nullable
        public string Address { get; set; }
        public bool? IsResponse { get; set; } // Nullable
        public string ProviderName { get; set; }
    }
}
