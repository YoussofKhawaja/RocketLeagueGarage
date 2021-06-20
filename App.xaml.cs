using RocketLeagueGarage.MVVM.Model;
using RocketLeagueGarage.MVVM.View;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

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
        }
    }
}