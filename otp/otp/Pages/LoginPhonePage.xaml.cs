using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace otp.Pages
{
    public partial class LoginPhonePage : ContentPage
    {
        public LoginPhonePage()
        {
            InitializeComponent();
            Helpers.Settings.Phone= txtphone.Text ;
        }
    }
}
