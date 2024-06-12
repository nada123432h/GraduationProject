using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace otp.Models
{
    public class users: FreshBasePageModel
    {
        public int CustomerId { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public string SelectedImageUrl { get; set; }
        private ImageSource _selectedImage;
        public ImageSource SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                RaisePropertyChanged("SelectedImage");

            }
        }
    }
}