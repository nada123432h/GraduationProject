using otp.Models;
using Rg.Plugins.Popup.Extensions;
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
    public partial class SignUpPage : Controls.Custompage
    {


        private readonly IGoogleManager _googleManager;
        GoogleUser GoogleUser = new GoogleUser();
        public bool IsLogedIn { get; set; }
        public SignUpPage()
        {
           // _googleManager = DependencyService.Get<IGoogleManager>();

           // CheckUserLoggedIn();
            InitializeComponent();
        }
      

        async private void Button_Clicked(object sender, EventArgs e)
        {
            if (PasswordEntry2.TextColor == Color.Red)
            {
                VisualStateManager.GoToState(PasswordEntry2, "InvalidStyle");
                var pop2 = new Helpers.MessageBoxPass();
                await App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);

                return;
                
            }
           

        }

        public bool isPressed = true;
        public bool isPressed2 = true;

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (isPressed == false)
            {
                ImgPas.Source = new FontImageSource
                {
                    Glyph = "",
                    FontFamily = "FontIconSolid",
                    Color = Color.FromHex("#334257")
                };
                PasswordEntry.IsPassword = true;
                isPressed = true;
            }
            
                else
                {
                ImgPas.Source = new FontImageSource
                    {
                        Glyph = "",
                        FontFamily = "FontIconSolid",
                        Color = Color.FromHex("#334257")
                    };
                PasswordEntry.IsPassword = false;
                    isPressed = false;
               }
            
               

        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            if (isPressed2 == false)
            {
                ImgPas2.Source = new FontImageSource
                {
                    Glyph = "",
                    FontFamily = "FontIconSolid",
                    Color = Color.FromHex("#334257")
                };
                PasswordEntry2.IsPassword = true;
                isPressed2 = true;
            }

            else
            {
                ImgPas2.Source = new FontImageSource
                {
                    Glyph = "",
                    FontFamily = "FontIconSolid",
                    Color = Color.FromHex("#334257")
                };
                PasswordEntry2.IsPassword = false;
                isPressed2 = false;
            }

        }
    }
}