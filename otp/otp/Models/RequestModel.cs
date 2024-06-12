using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace otp.Models
{
   public class RequestModel: FreshBasePageModel
    {

        public object TbProvider { get; set; }
        public object TbService { get; set; }



        private bool _isButtonEnabled ;

        public bool IsButtonEnabled
        {
            get { return _isButtonEnabled; }
            set
            {
                if (_isButtonEnabled != value)
                {
                    _isButtonEnabled = value;
                    RaisePropertyChanged(nameof(IsButtonEnabled));
                }
            }
        }


        private bool _isVisableButton ;

        public bool isVisableButton
        {
            get { return _isVisableButton; }
            set
            {
                if (_isVisableButton != value)
                {
                    _isVisableButton = value;
                    RaisePropertyChanged(nameof(isVisableButton));
                }
            }
        }







        public int ID { get; set; }
        public int ProviderId { get; set; }
        public int ServiceId { get; set; }
        public string Descriptions { get; set; }
        public string ImageNameProblem { get; set; }
        public bool? StatusOfRequest { get; set; } // Nullable
        public bool? IsResponse{ get; set; } // Nullable
        public string ServiceName { get; set; }
        public string ServiceNameAr { get; set; }
        public Color StatusColor { get; set; }
        public string Address { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsWaiting { get; set; }
        public bool IsVisableDelete { get; set; }
        public bool? IsConfirmedbuttons { get; set; } // Nullable
        public int CustomerId { get; set; }

        public string ProviderName { get; set; }
        public string statusLabel { get; set; }

        public DateTime Date { get; set; }


        public decimal Cost { get; set; }




    }
}
