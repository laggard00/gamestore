using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Filters {
    public class OrderFilter {
        internal DateTime startDateTime { get; set; }
        internal DateTime endDateTime { get; set; }

        public string? start { get { return startDateTime.ToString(); }

                               set { if (value == null) { startDateTime = DateTime.MinValue; }
                                     else { startDateTime = DateTimeParser(value); } } }
        public string? end { get { return endDateTime.ToString(); } 

                             set { if (value == null) { endDateTime = DateTime.MaxValue; }
                                   else { endDateTime = DateTimeParser(value); } } }

        private DateTime DateTimeParser(string date) {
            var dates = date.ToCharArray();
            dates[28] = '+';
            date = string.Join("", dates);
            var decodedDate = Uri.UnescapeDataString(date);
            var splicedString = decodedDate.Substring(0, decodedDate.IndexOf('(') - 1);
            DateTime.TryParseExact(splicedString, "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz",
                                             CultureInfo.InvariantCulture,
                                             DateTimeStyles.None,
                                             out var parsedDate);
            return parsedDate;
        }
    }
}
