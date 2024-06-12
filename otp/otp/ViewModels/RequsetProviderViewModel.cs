using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using otp.Models;
using FreshMvvm;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Bindings;
using otp.Services;
using Rg.Plugins.Popup.Services;
using Newtonsoft.Json;
using System.IO;
using Rg.Plugins.Popup.Extensions;
using System.Threading.Tasks;
using otp.Helpers;

namespace otp.ViewModels
{
    public class RequsetProviderViewModel : FreshBasePageModel
    {
        string _Username;
        public string Username
        {

            get
            {
                return _Username;
            }
            set
            {
                _Username = value;
                RaisePropertyChanged("Username");
            }
        }
        DateTime _Date;
        public DateTime Date
        {

            get
            {
                return _Date;
            }
            set
            {
                _Date = value;
                RaisePropertyChanged("Date");
            }
        }
        bool _isConfirmed;

        public bool IsConfirmed
        {
            get { return _isConfirmed; }
            set
            {
                if (_isConfirmed != value)
                {
                    _isConfirmed = value;
                    RaisePropertyChanged(nameof(IsConfirmed));
                }
            }
        }

        decimal _CostUpdated;
        public decimal CostUpdated
        {

            get
            {
                return _CostUpdated;
            }
            set
            {
                _CostUpdated = value;
                RaisePropertyChanged("CostUpdated");
            }
        }

        bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    RaisePropertyChanged(nameof(IsRefreshing));
                }
            }
        }
        ObservableCollection<RequestProviderModel> _ListProvider;
        public ObservableCollection<RequestProviderModel> ListProvider
        {

            get
            {
                return _ListProvider;
            }
            set
            {
                _ListProvider = value;
                RaisePropertyChanged("ListProvider");
            }
        }
        public ProviderModel CurrentOfProvider { get; set; }
        public ServiceModel CurrentServiceProvider { get; set; }
        public Provider CurrentProvider { get; set; }
        public override async void Init(object initData)
        {
            base.Init(initData);
            CurrentProvider = initData as Provider;

            await InitializeProvider();


        }







        //   public ProviderModel CurrentProvider { get; set; }


        public users Currentuser { get; set; }
        public RequestModel Request { get; set; }
        public Command PostService { get; set; }
        public Command imgPic { get; set; }
        public Command OnRefreshRequested { get; }
        public Command Rejected { get; set; }
        public Command GotToProfile { get; set; }
        public Command GoChat { get; set; }



        public Command Confirmed { get; set; }





        public RequsetProviderViewModel()
        {


            ListProvider = new ObservableCollection<RequestProviderModel>();
            ListOfProvider = new ObservableCollection<ProviderModel>();

            OnRefreshRequested = new Command(async () => await RefreshCategories());
            Rejected = new Command<RequestProviderModel>(OnRejected);
            Confirmed = new Command<RequestProviderModel>(OnConfirmed);

            GoChat = new Command(OnGoChat);
            GotToProfile = new Command(OnGoProgile);

        }

        ObservableCollection<ProviderModel> _ListOfProvider;
        public ObservableCollection<ProviderModel> ListOfProvider
        {

            get
            {
                return _ListOfProvider;
            }
            set
            {
                _ListOfProvider = value;
                RaisePropertyChanged("ListOfProvider");
            }
        }
        public async Task InitializeProvider()
        {
            string json2 = await Helpers.Utility.CallWebApi($"/api/RequestProvider/{CurrentProvider.ProvicderId}");
            ListProvider = JsonConvert.DeserializeObject<ObservableCollection<RequestProviderModel>>(json2);
            Username = CurrentProvider.ProviderName;
            IsRefreshing = false;
            foreach (var request in ListProvider)
            {
                if (request.IsResponse == true)
                {
                    request.statusLabel = "تم قبول تعديلك ";
                    request.IsChoose = false;
                    request.IsConfirmed = true;
                }
                else if (request.IsResponse == null)
                {

                    request.IsChoose = true;
                    request.IsConfirmed = false;

                }

                if (request.StatusOfRequest == true)
                {
                    request.statusLabel = "لقد قبلت طلب العميل";
                    request.IsChoose = false;
                    request.IsConfirmed = true;

                }
            }

        }






        async Task RefreshCategories()
        {
            IsRefreshing = true;

            await InitializeProvider();
        }


        async void OnRejected(RequestProviderModel oRequestProviderModel)
        {
            //if (oRequestProviderModel.Date2.Date == new DateTime(1900, 1, 1))
            //{
            //    await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MsgxfForgetmailPhone(), true);
            //    return;
            //}
            if (oRequestProviderModel.UpdatedCost == 0)
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MsgxfForgetmailPhone(), true);
                return;
            }
            int id = oRequestProviderModel.ID; // Adjust property name if necessary
            oRequestProviderModel.StatusOfRequest = false;



            Date = oRequestProviderModel.Date2;
            // Call the UpdateStatusOfRequest method from Utility
            string result = await Utility.UpdateStatusOfRequest(id, false, oRequestProviderModel.UpdatedCost, Date);
            await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MsgSend(), true);
            oRequestProviderModel.IsChoose = true;

        }

        async void OnConfirmed(RequestProviderModel oRequestProviderModel)
        {
            int id = oRequestProviderModel.ID; // Extract the ID of the request
            oRequestProviderModel.StatusOfRequest = true; // Update the status to confirmed in the local model

            // Call the UpdateStatusOfRequest method from Utility to update the status on the server
            string result = await Utility.UpdateStatusOfRequest(id, true, oRequestProviderModel.Cost, oRequestProviderModel.Date);
            await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MsgSend(), true);
            oRequestProviderModel.IsChoose = true;
        }

        async void OnGoChat()
        {
            await CoreMethods.PushPageModel<ChatViewModel>(null, true);
        }

        ObservableCollection<ServiceModel> _ListServicesl;
        public ObservableCollection<ServiceModel> ListServices
        {

            get
            {
                return _ListServicesl;
            }
            set
            {
                _ListServicesl = value;
                RaisePropertyChanged("ListServices");
            }
        }
        async void OnGoProgile()
        {
            string json3 = await Helpers.Utility.CallWebApi($"/api/serviceprovider/{CurrentProvider.ServiceId}");
            ListOfProvider = JsonConvert.DeserializeObject<ObservableCollection<ProviderModel>>(json3);
            CurrentOfProvider = ListOfProvider.Where(a => a.ProvicderId == CurrentProvider.ProvicderId).FirstOrDefault();
            var _service = new Services.ServicesService();
            ListServices = await _service.GetServices();

            // تعيين اسم الخدمة إلى CurrentOfProvider
            var service = ListServices.FirstOrDefault(a => a.ServiceId == CurrentOfProvider.ServiceId);
            CurrentOfProvider.ServiceName = service.ServiceName;
            await CoreMethods.PushPageModel<ProfileProviderViewModel>(CurrentOfProvider, modal: true);
        }





    }
}
