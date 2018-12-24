
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MFAX01V3
{
    public class ConvertReadToStyle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Boolean)
            {
                return (bool)value ? "Đã Xem" : "Chưa Xem";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class EnumtoExStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "fjesLINE_UNAVAILABLE":
                    return "Line Không Có";
                case "fjesBUSY":
                    return "Line Bận";
                case "fjesNO_DIAL_TONE":
                    return "Không Đổ Chuông";
                case "fjesFATAL_ERROR":
                    return "FATAL_ERROR";
                case "fjesNO_ANSWER":
                    return "Không Trả Lời";
                case "fjesCALL_ABORTED":
                    return "Bị Từ Chối";
                default:
                    return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public class EnumToStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            switch (value.ToString())
            {
                case "fjsRETRIES_EXCEEDED":
                    return "Gửi Lỗi";
                case "fjsCANCELED":
                    return "Hủy Fax";
                case "fjsNOLINE":
                    return "Không Line";
                default:
                    return value;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

    }
    public class ConvertSizeToPixel : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[1] == null) return null;
            double h = double.Parse(values[0].ToString());
            double re = double.Parse(values[1].ToString());
            return h * re;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConvertBoolToImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
             if (value is Boolean)
            {
                return (bool)value ? "/MFAX01V3;component/images/Ok-icon.png" : "/MFAX01V3;component/images/warning-xxl.png";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
