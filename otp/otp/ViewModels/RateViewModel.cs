using FreshMvvm;
using Newtonsoft.Json;
using otp.Helpers;
using otp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace otp.ViewModels
{
    public class RateViewModel : FreshBasePageModel
    {

        public ProviderModel CurrentOfProvider { get; set; }
        public async override void Init(object initData)
        {
            base.Init(initData);
            CurrentOfProvider = initData as ProviderModel;

        }
        private double _selectedRating;
        public double SelectedRating
        {
            get => _selectedRating;
            set
            {
                if (_selectedRating != value)
                {
                    _selectedRating = value;
                    RaisePropertyChanged(nameof(SelectedRating));
                }
            }
        }
        public Command RatingChangedCommand { get; set; }

        private int _coloredStarCount;
        public int ColoredStarCount
        {
            get => _coloredStarCount;
            set
            {
                if (_coloredStarCount != value)
                {
                    _coloredStarCount = value;
                    RaisePropertyChanged(nameof(ColoredStarCount));
                }
            }
        }

     
       

        public RateViewModel()
        {

            RatingChangedCommand = new Command(async () => await UpdateRatingInDatabase());
        }
        public RateViewModel(ProviderModel currentprovider)
        {
            CurrentOfProvider = currentprovider;
            RatingChangedCommand = new Command(async () => await UpdateRatingInDatabase());

        }

        private async Task UpdateRatingInDatabase()
        {
            try
            {
                // Create an anonymous object to hold the provider ID and rating
                var requestData = new
                {
                    ProviderId = CurrentOfProvider.ProvicderId,
                    Rating = SelectedRating
                };

                // Serialize the object into JSON string
                string jsonContent = JsonConvert.SerializeObject(requestData);

                // Post the JSON content to the server
                string result = await Helpers.Utility.PostData("/api/rate", jsonContent);

                // Handle the result as needed
                // For example, you can display a success message or handle errors
            }
            catch (Exception ex)
            {
                // Handle exceptions
            }
        }




    }


}
