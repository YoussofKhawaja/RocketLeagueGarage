using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Media;

namespace RocketLeagueGarage.MVVM.Model
{
    [Serializable]
    public class RocketData
    {
        public static string WhatDoing { get; set; }

        public static string OnOff { get; set; }

        public static string TimeLabel { get; set; }

        public static string Error { get; set; }

        public static string IsRunning { get; set; }

        public static string SettingUp { get; set; }

        public static string Done { get; set; }

        public static PackIconKind Kind { get; set; }

        public static Color Color { get; set; }
    }
}