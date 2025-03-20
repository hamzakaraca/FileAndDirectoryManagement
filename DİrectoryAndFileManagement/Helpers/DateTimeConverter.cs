using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DİrectoryAndFileManagement.Helpers
{
    using System;
    using System.Globalization;

    public class DateTimeConverter
    {
        public static DateTime ConvertToDateTime(string date, string time, string format = "dd/MM/yyyy HH:mm:ss")
        {
            try
            {
                string dateTimeString = $"{date} {time}";
                DateTime dateTime = DateTime.ParseExact(dateTimeString, format, CultureInfo.InvariantCulture);
                return dateTime;
            }
            catch (FormatException)
            {
                throw new ArgumentException("Geçersiz tarih veya saat formatı.");
            }
        }
    }
}
