using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace otp.ViewModels
{
    public class IntroductionViewModel : FreshBasePageModel
    {
        public ICommand OpenLinkCommand { get; private set; }
        public Command GoRgister { get; set; }
        private string description1;
        public string Description1
        {
            get { return description1; }
            set
            {
                description1 = value;
                RaisePropertyChanged(nameof(Description1));
            }
        }
        private string description2;
        public string Description2
        {
            get { return description2; }
            set
            {
                description2 = value;
                RaisePropertyChanged(nameof(Description2));
            }
        }

        public IntroductionViewModel()
        {
            Description1 = "تعد شركتنا الرائدة في تقديم فرص عمل للفنيين ذوي الخبرة .\n"
            + "إذا كنت تبحث عن فرصة للانضمام او التعرف اكثر علينا:\n";
            Description2 = "\n إذا كنت عضوًا في شركتنا، يُرجى:";
            OpenLinkCommand = new Command(OpenLink);
            GoRgister = new Command(OnGoRgister);
        }

        private void OpenLink()
        {
            var url = "https://6633cac352829c12f1cab22b--admirable-halva-6e7f2f.netlify.app/"; // Replace with your desired link

            Launcher.OpenAsync(new Uri(url));
        }

        async void OnGoRgister()
        {
            await CoreMethods.PushPageModel<LoginProviderViewModel>(null, true);
        }

    }

}