using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioRss.Model
{
    public static class ControlModel
    {
        public class LiveTileData
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public Uri ImageUri { get; set; }
            public Uri ReadMoreUri { get; set; }
        }
    }
}
