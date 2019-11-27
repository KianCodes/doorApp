using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SQLite;

namespace doorApp.Droid.Model
{
    public class Reg
    {
        [PrimaryKey, AutoIncrement]
        public int? id { get; set; }
        public string ipAddr { get; set; }
        public string macAddr { get; set; }
        public string nickname { get; set; }
        public string status { get; set; }

    }
}