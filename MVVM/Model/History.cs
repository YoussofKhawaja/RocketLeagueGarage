using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RocketLeagueGarage.MVVM.Model
{
    public class History
    {
        public string DateTime { get; set; }
        public string name { get; set; }
        public string test { get; set; }
        public Department Grouping { get; set; }

        public enum Department
        {
            Today,
            Yestrday
        }

        public History()
        {
        }
    }
}