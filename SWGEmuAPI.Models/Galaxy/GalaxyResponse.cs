using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Model.Galaxy
{
    public class GalaxyResponse : GalaxyRequest
    {
        public string address { get; set; }
        public int pingport { get; set; }
        public int port { get; set; }
        public int population { get; set; }
    }
}
