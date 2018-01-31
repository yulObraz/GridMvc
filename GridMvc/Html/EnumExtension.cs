using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace GridMvc.Html {
    public static class EnumExtensions
    {
        public static string DisplayName(this Enum e)
        {
            var da = (DisplayAttribute[])(e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false));
            return da.Length > 0 ? da[0].Name : e.ToString();
        }
    }
}
