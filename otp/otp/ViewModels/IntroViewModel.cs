using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using otp.Models;
using Xamarin.Forms;

namespace otp.ViewModels
{
   public class IntroViewModel: FreshBasePageModel
    {

        
        public Command GoRgister { get; set; }

           
        public IntroViewModel()
        {
            ListSliders = LoadItems();
         
            GoRgister = new Command(OnGoRgister);
         

        }

        ObservableCollection<IntroClass> _ListSliders;
        public ObservableCollection<IntroClass> ListSliders
        {
            get
            {
                return _ListSliders;
            }
            set
            {
                _ListSliders = value;
                if (_ListSliders != null)
                    RaisePropertyChanged("ListSliders");

            }
        }


    

        public ObservableCollection<IntroClass> LoadItems()
        {
            ObservableCollection<IntroClass> lstItems = new ObservableCollection<IntroClass>();

            IntroClass oItems = new IntroClass();

            oItems.Description1 = "ابحث عن أقرب الخدمات إليك هنا!";
            oItems.Description2 = "استكشف الخدمات القريبة المتاحة في المنطقه التي تتواجد فيها";
          //  oItems.btn = "سجل الان";
            lstItems.Add(oItems);

            oItems = new IntroClass();
            oItems.Description1 = "مرحبًا بك مرة أخرى";


   



            lstItems.Add(oItems);


            return lstItems;
        }

        async void OnGoRgister()
        {

          await  CoreMethods.PushPageModel<ChoseViewModel>(null,true);


        }
     


    }
}
