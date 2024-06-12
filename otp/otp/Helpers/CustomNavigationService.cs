using FreshMvvm;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace otp.Helpers
{
    public class CustomNavigationService : FreshMvvm.FreshNavigationContainer, IFreshNavigationService
    {
        public CustomNavigationService(Page page) : base(page)
        {
        }

        public async Task PushPageModel<T>(object data = null, bool modal = false, bool animate = true) where T : FreshBasePageModel
        {
            var navigation = CurrentPage.Navigation;
            if (navigation != null)
            {
                var pageModel = FreshPageModelResolver.ResolvePageModel<T>(data);
                var page = FreshPageModelResolver.ResolvePageModel(pageModel.GetType(), data);

                if (animate)
                {
                    await Task.WhenAll(CurrentPage.TranslateTo(-CurrentPage.Width, 0, 250),
                        navigation.PushAsync(page, false));
                    await CurrentPage.TranslateTo(0, 0, 250, Easing.CubicInOut);
                }
                else
                {
                    await navigation.PushAsync(page, false);
                }
            }
        }

        // Implement other navigation methods like PopPageModel, etc. similarly.
    }
}
