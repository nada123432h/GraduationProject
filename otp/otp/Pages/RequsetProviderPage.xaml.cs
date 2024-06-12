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
    public partial class RequsetProviderPage : Controls.Custompage
    {
        public RequsetProviderPage()
        {
            InitializeComponent();
            if (Helpers.Settings.Language == "ar")
                this.FlowDirection = FlowDirection.RightToLeft;
            else
                this.FlowDirection = FlowDirection.LeftToRight;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }
    }
}