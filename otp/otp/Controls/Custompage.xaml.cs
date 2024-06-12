using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace otp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Custompage : ContentPage
    {
        public Custompage()
        {
            InitializeComponent();
           
           if (Helpers.Settings.Language == "ar")
             this.FlowDirection = FlowDirection.RightToLeft;
            else
               this.FlowDirection = FlowDirection.LeftToRight;
        }
    }
}