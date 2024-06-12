using System;
using Xamarin.Forms;

namespace otp.Converters
{
    public class MessageBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is bool isIncoming))
                return Color.FromHex("#041C32");

            return isIncoming ? Color.FromHex("#ECB365") : Color.FromHex("#041C32");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
