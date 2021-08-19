using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Avalonia.Data.Converters;

namespace SubtitleEditor.Converters
{
    public class MsTimeFormatConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is Avalonia.UnsetValueType || values[1] is Avalonia.UnsetValueType) return "--:--";
            var length = (long) values[1];
            if (length == -1) return "--:--";
            var lSec = length / 1000;
            var lMin = lSec / 60;
            var lSecRem = lSec % 60;
            var currentStamp = (float) values[0] * length;
            var pSec = (int) currentStamp / 1000;
            var pMin = pSec / 60;
            var pSecRem = pSec % 60;
            return $"{pMin:D2}:{pSecRem:D2} / {lMin:D2}:{lSecRem:D2}";
            
        }
    }
}