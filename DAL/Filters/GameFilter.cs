using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Filters
{
    public class GameFilter
    {
        public List<Guid>? genres { get; set; }
        public List<Guid>? platforms { get; set; }
        public List<Guid>? publishers { get; set; }
        public double? maxPrice { get; set; }
        public double? minPrice { get; set; }
        public string? name { get; set; }
        public string? datePublishing { get; set; }
        public string? sort { get; set; }
        public int? page { get; set; } = 1;
        public string? pageCount { get; set; } = "all";

    }
}
