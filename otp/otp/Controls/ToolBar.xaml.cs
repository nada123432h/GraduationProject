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
    public partial class ToolBar : ContentView
    {

        public event EventHandler<string> MenuClick;
        public static readonly BindableProperty TitleProperty = BindableProperty.Create("Title", typeof(string), typeof(ContentView), "", propertyChanged: OnEventNameChanged);

        public static readonly BindableProperty HasBackButtonProperty = BindableProperty.Create("HasBackButton", typeof(bool), typeof(ContentView), true, propertyChanged: OnBackButtonChanged);

        public static readonly BindableProperty HasCartButtonProperty = BindableProperty.Create("HasCartButton", typeof(bool), typeof(ContentView), true, propertyChanged: OnCartButtonChanged);

        public string Title
        {
            get { return (string)base.GetValue(TitleProperty); }
            set
            {
                base.SetValue(TitleProperty, value);
            }
        }

        public bool HasBackButton
        {
            get { return (bool)base.GetValue(HasBackButtonProperty); }
            set
            {
                base.SetValue(HasBackButtonProperty, value);
            }
        }

        public bool HasCartButton
        {
            get { return (bool)base.GetValue(HasCartButtonProperty); }
            set
            {
                base.SetValue(HasCartButtonProperty, value);
            }
        }
        static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null)
                return;
            var tolbal = bindable as ToolBar;
            tolbal.lblTitle.Text = newValue.ToString();
        }
        static void OnBackButtonChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null)
                return;
            var tolbal = bindable as ToolBar;
            var fontSource = new FontImageSource();
            fontSource.FontFamily = "FontIconSolid";
            if ((bool)newValue)
            {
                fontSource.Glyph = null;
            }
            else
            {
                fontSource.Glyph = null;
            }
            tolbal.imgBack.Source = fontSource;
           
        }

        static void OnCartButtonChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null)
                return;
            var tolbal = bindable as ToolBar;
            var fontSource = new FontImageSource();
            fontSource.FontFamily = "FontIconSolid";
            if ((bool)newValue)
            {
                fontSource.Glyph = null;
            }
            else
            {
                fontSource.Glyph = null;
            }
            tolbal.imgCart.Source = fontSource;

        }
        public ToolBar()
        {
            InitializeComponent();
            if (Helpers.Settings.Language == "ar")
            {
                //imgBack.FlowDirection = FlowDirection.RightToLeft;
                var fontSource = new FontImageSource();
                fontSource.FontFamily = "FontIconSolid";
                fontSource.Color = Color.FromHex("#141E46");
                fontSource.Glyph = "";
                imgBack.Source = fontSource;
                imgBack.FlowDirection = FlowDirection.RightToLeft;

            }
            else
            {
                imgBack.FlowDirection = FlowDirection.LeftToRight;

            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (HasBackButton)
            {
                Navigation.PopModalAsync(true);
            }
            else
            {
                MenuClick?.Invoke(this, "ss");
            }

        }
    }
}