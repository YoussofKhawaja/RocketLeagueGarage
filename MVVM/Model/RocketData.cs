using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketLeagueGarage.MVVM.Model
{
    [Serializable]
    public class RocketData
    {
        public static string WhatDoing { get; set; }

        public static string OnOff { get; set; }

        public static string TimeLabel { get; set; }

        public static PackIconKind Kind { get; set; }
    }
}