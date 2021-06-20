using Notifications.Wpf.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RocketLeagueGarage.FilesManager;
using RocketLeagueGarage.Helper;
using RocketLeagueGarage.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace RocketLeagueGarage.MVVM.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public AccountDataModel user;
        public static ChromeDriver driver;
        private CountDownTimer timer = new CountDownTimer();

        #region bools

        private static bool IsRunning = false;

        #endregion bools

        public HomeView()
        {
            InitializeComponent();
            startStatusBarTimer2();
            Timer();
        }

        #region Offset

        private void onoffbutton_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonplay.Offset = 0.2;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonplay.Offset = 0.0;
            buttonplay2.Offset = 1;
        }

        #endregion Offset

        #region Buttons

        private async void onoffbutton_Click(object sender, RoutedEventArgs e)
        {
            if (IsRunning == false)
            {
                RocketData.OnOff = "Running";
                icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Stop;

                await Task.Run(ChromeDriver);

                IsRunning = true;

                await Task.Run(Element);

                IsRunning = false;

                RocketData.OnOff = "Not Running";
                icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;

                timer.Start();
            }
            else if (IsRunning == true)
            {
                await Task.Run(ChromeDriverQuit);
            }
        }

        private void TextUpdate(object sender, ElapsedEventArgs e)
        {
            TextUpdate();
            TimerDone();
        }

        #endregion Buttons

        #region void classes

        private void TimerDone()
        {
            if (timer.TimeLeftMsStr == "00:00.000")
            {
                timer.Reset();
                onoffbutton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        //timer
        private void startStatusBarTimer2()
        {
            try
            {
                System.Timers.Timer statusTime = new System.Timers.Timer();
                statusTime.Interval = 1;
                statusTime.Elapsed += new System.Timers.ElapsedEventHandler(TextUpdate);
                statusTime.Enabled = true;
            }
            catch
            {
            }
        }

        private void TextUpdate()
        {
            this.Dispatcher.Invoke(() =>
            {
                whatdoing.Text = RocketData.WhatDoing;
                onoff.Content = RocketData.OnOff;
                timelabel.Text = RocketData.TimeLabel;

                if (IsRunning == false)
                {
                    icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                }
                else
                {
                    icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Stop;
                }
            });
        }

        private async void ChromeDriver()
        {
            user = Save.ReadFromXmlFile<AccountDataModel>("Data", "Account");

            RocketData.WhatDoing = "Setting Up ChromeDrive";

            await Task.Delay(1000);

            new DriverManager().SetUpDriver(new ChromeConfig());
            RocketData.WhatDoing = "Setting Up Done, Starting Up ChromeDriver";

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("headless");

            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;

            driver = new ChromeDriver(driverService, options)
            {
                Url = $"https://rocket-league.com/trades/{user.Name}"
            };

            RocketData.WhatDoing = "Started ChromeDriver";
        }

        private async void Element()
        {
            driver.FindElement(By.CssSelector("#acceptPrivacyPolicy")).Click();

            IWebElement email = driver.FindElement(By.CssSelector("#header-email"));
            email.SendKeys(user.Email);
            RocketData.WhatDoing = "Email Entered";

            await Task.Delay(1000);

            IWebElement password = driver.FindElement(By.CssSelector("#header-password"));
            password.SendKeys(user.Password);
            RocketData.WhatDoing = "Password Entered";

            await Task.Delay(1000);

            password.SendKeys(Keys.Enter);
            RocketData.WhatDoing = "Login Button Clicked";

            await Task.Delay(1000);

            IWebElement eror = driver.FindElement(By.ClassName("rlg-site-popup__container"));
            string error = eror.Text;

            if (error.Contains("ERROR"))
            {
                RocketData.WhatDoing = "Your email or password were not recognised";

                driver.Quit();

                return;
            }
            else
            {
                IWebElement notificationperms = driver.FindElement(By.ClassName("rlg-notificationperms__decline"));
                notificationperms.Click();
                RocketData.WhatDoing = "Disable Notification";

                await Task.Delay(1000);

                var trades = driver.FindElementsByClassName("rlg-trade__bump");
                var closeup = driver.FindElement(By.ClassName("rlg-site-popup__container"));

                int i = 0;
                foreach (var trade in trades)
                {
                    trade.Click();
                    RocketData.WhatDoing = $"Trade {i} Bumped";

                    await Task.Delay(1000);

                    closeup.Click();

                    i++;
                }
                RocketData.WhatDoing = $"Trade Bump for {trades.Count} Done";
            }

            driver.Quit();
        }

        private void ChromeDriverQuit()
        {
            driver.Quit();
            RocketData.OnOff = "Not Running";
            icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
            RocketData.WhatDoing = "Stopped";
        }

        private void Timer()
        {
            //set to 20 mins
            timer.SetTime(20, 0);

            //update label text
            timer.TimeChanged += () =>
            {
                RocketData.TimeLabel = timer.TimeLeftMsStr + " " + "Minute";
            };

            //timer step. By default is 1 second
            timer.StepMs = 77;
        }

        #endregion void classes
    }
}