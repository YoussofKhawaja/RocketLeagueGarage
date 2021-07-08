﻿using RocketLeagueGarage.MVVM.Model;
using System.Windows;
using System.Windows.Media;

namespace RocketLeagueGarage
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainWindow mainWindow;

        public void ApplicationStart(object sender, StartupEventArgs e)
        {
            mainWindow = new MainWindow();
            mainWindow.Show();
            check();
        }

        private void check()
        {
            RocketData.WhatDoing = "Doing Nothing, Not Running";
            RocketData.OnOff = "Not Running";
            RocketData.TimeLabel = "Run It To Know!";
            RocketData.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
            var color = (Color)ColorConverter.ConvertFromString("#FFFFFF");
            RocketData.Color = color;
        }
    }
}