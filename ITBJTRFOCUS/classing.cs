using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ITBJTRFOCUS
{
    [Serializable()]
    public class leaderboard
    {
        public string Name { get; set; }
        public int Score { get; set; }
    }


}
