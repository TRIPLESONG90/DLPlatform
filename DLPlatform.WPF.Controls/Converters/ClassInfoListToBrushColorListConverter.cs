using DLPlatform.WPF.Controls.Labeling;
using PLPlatform.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DLPlatform.WPF.Controls.Converters
{
    public class ClassInfoListToBrushColorListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<ClassInfo> classInfos)
            {
                return classInfos.Select(x => new BrushColor()
                {
                    R = x.Color.R,
                    G = x.Color.G,
                    B = x.Color.B
                }).ToList();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
