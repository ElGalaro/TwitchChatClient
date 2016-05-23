using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Data;
using System.Windows.Media;

namespace TwitchChatClient.Ui.Converters
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = value.ToString();
            if(color == string.Empty)
                return new SolidColorBrush(Color.FromRgb(0,0,0));
            return (SolidColorBrush) new BrushConverter().ConvertFrom(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
