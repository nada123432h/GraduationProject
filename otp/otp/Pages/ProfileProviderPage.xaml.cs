using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace otp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileProviderPage : ContentPage
    {
        public ProfileProviderPage()
        {
            InitializeComponent();
            txtEmailProvider.Text = Helpers.Settings.ProviderEmail;
            if (Helpers.Settings.Language == "ar")
                this.FlowDirection = FlowDirection.RightToLeft;
            else
                this.FlowDirection = FlowDirection.RightToLeft;
        }
    }
}