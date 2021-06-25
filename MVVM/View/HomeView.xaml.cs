using Notifications.Wpf.Core;
using Notifications.Wpf.Core.Controls;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RocketLeagueGarage.FilesManager;
using RocketLeagueGarage.Helper;
using RocketLeagueGarage.MVVM.Model;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace RocketLeagueGarage.MVVM.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public static HomeView homeview;
        public AccountDataModel user;
        public static ChromeDriver driver;
        private CountDownTimer timer = new CountDownTimer();

        #region bools

        private static bool IsRunning = false;
        private static bool starting = false;
        private static bool done = false;

        #endregion bools

        public HomeView()
        {
            this.InitializeComponent();
            homeview = this;

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
            if (IsRunning == false && starting == false)
            {
                Debug.WriteLine("here1");
                starting = true;

                await Task.Run(ChromeDriver);

                IsRunning = true;

                await Task.Run(Element);

                IsRunning = false;

                if (done == true)
                {
                    var color2 = (Color)ColorConverter.ConvertFromString("#ffffff ");
                    RocketData.Color = color2;
                    timer.Start();
                }
            }
            else if (IsRunning == true)
            {
                Debug.WriteLine("here2");

                await Task.Run(ChromeDriverQuit);

                var color2 = (Color)ColorConverter.ConvertFromString("#ffffff ");
                RocketData.Color = color2;
            }
            else
            {
                var notificationManager = new NotificationManager(NotificationPosition.TopRight);

                await notificationManager.ShowAsync(
                new NotificationContent { Title = "Error", Message = "Not Running", Type = NotificationType.Error },
                areaName: "WindowArea");
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
                icon.Kind = RocketData.Kind;
                buttonplay.Color = RocketData.Color;
            });
        }

        private void ChromeDriver()
        {
            RocketData.OnOff = "Updating";
            RocketData.Kind = MaterialDesignThemes.Wpf.PackIconKind.Update;

            user = Save.ReadFromXmlFile<AccountDataModel>("Data", "Account");

            RocketData.WhatDoing = "Setting Up ChromeDrive";

            Thread.Sleep(1000);

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

            var color = (Color)ColorConverter.ConvertFromString("#FF0000");
            RocketData.Color = color;

            RocketData.WhatDoing = "Started ChromeDriver";
            RocketData.OnOff = "Running";
            RocketData.Kind = MaterialDesignThemes.Wpf.PackIconKind.Stop;
        }

        private void Element()
        {
            try
            {
                driver.FindElement(By.CssSelector("#acceptPrivacyPolicy")).Click();

                IWebElement email = driver.FindElement(By.CssSelector("#header-email"));
                email.SendKeys(user.Email);
                RocketData.WhatDoing = "Email Entered";

                Thread.Sleep(1000);

                IWebElement password = driver.FindElement(By.CssSelector("#header-password"));
                password.SendKeys(user.Password);
                RocketData.WhatDoing = "Password Entered";

                Thread.Sleep(1000);

                password.SendKeys(Keys.Enter);
                RocketData.WhatDoing = "Login Button Clicked";

                Thread.Sleep(1000);

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

                    Thread.Sleep(1000);

                    var trades = driver.FindElementsByClassName("rlg-trade__bump");
                    var closeup = driver.FindElement(By.ClassName("rlg-site-popup__container"));

                    int i = 0;
                    foreach (var trade in trades)
                    {
                        trade.Click();
                        RocketData.WhatDoing = $"Trade {i} Bumped";

                        Thread.Sleep(1000);

                        closeup.Click();

                        i++;
                    }
                    RocketData.WhatDoing = $"Trade Bump for {trades.Count} Done";

                    Thread.Sleep(1000);

                    RocketData.WhatDoing = "Login Button Clicked";
                }

                driver.Quit();
                done = true;
                RocketData.OnOff = "Not Running";
                RocketData.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;

                var color = (Color)ColorConverter.ConvertFromString("#FFFFFF");
                RocketData.Color = color;
            }
            catch
            {
            }
        }

        private void ChromeDriverQuit()
        {
            driver.Quit();
            IsRunning = false;
            RocketData.OnOff = "Not Running";
            RocketData.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
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