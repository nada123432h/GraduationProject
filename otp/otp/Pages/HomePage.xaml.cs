using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Plugin.StoreReview;
using Xamarin.Forms.PancakeView;

namespace otp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : Controls.Custompage
    {
        public HomePage()
        {
            InitializeComponent();
            //txtemail.Text = Helpers.Settings.Username;
           


        }

        private void chkbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

            //if (chkbox.IsChecked)
            //    App.ChangeTheme(1);
            //else
            //    App.ChangeTheme(2);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Image delIcon = (Image)sender;
            

            if (delIcon.Source is FontImageSource iconsource)
            {
                delIcon.ScaleTo(0, 250);
              
            }
        }
    }
}