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
    public partial class IntroPage : Controls.Custompage
    {
        public IntroPage()
        {
            InitializeComponent();
           
            AnimationCarsoual();
        }
        System.Timers.Timer timer;
        private async  Task StartPulseAnimation(Button button)
        {
            uint duration = 1000; // Duration for one pulse cycle (in milliseconds)
            double scale = 1.2; // Scale factor for the pulse effect

            int pulseCount = 0; // Counter for pulse iterations

            while (pulseCount < 6) // Perform the animation 4 times
            {
                // Scale up
                await button.ScaleTo(scale, duration / 2);
                // Scale down
                await button.ScaleTo(1, duration / 2);

                pulseCount++; // Increment the pulse counter
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Start the pulse animation
            await StartPulseAnimation(btnIntro);
        }
        private void AnimationCarsoual()
        {
            timer = new System.Timers.Timer(5000) { AutoReset = true, Enabled = true };
            timer.Elapsed += (s, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (caView.Position == 1)
                    {
                        caView.Position = 0;
                        return;
                    }
                    caView.Position += 1;
                }
                    );
            };
           
        }
    }
}