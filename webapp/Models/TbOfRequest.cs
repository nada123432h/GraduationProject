//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace webapp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TbOfRequest
    {
        public int ID { get; set; }
        public Nullable<int> ProviderId { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public string Descriptions { get; set; }
        public string ImageNameProblem { get; set; }
        public string ServiceName { get; set; }
        public string ProviderName { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string Address { get; set; }
        public Nullable<bool> StatusOfRequest { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<bool> IsResponse { get; set; }
    
        public virtual TbUser TbUser { get; set; }
    }
}
