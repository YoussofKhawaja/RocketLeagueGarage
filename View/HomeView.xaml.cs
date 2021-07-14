using Notifications.Wpf.Core;
using Notifications.Wpf.Core.Controls;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RocketLeagueGarage.FilesManager;
using RocketLeagueGarage.Helper;
using RocketLeagueGarage.Model;
using RocketLeagueGarage.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using WebDriverManager.Helpers;

namespace RocketLeagueGarage.View
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
        private HistoryLogsViewModel viewmodel = new HistoryLogsViewModel();

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
            try
            {
                user = Save.ReadFromXmlFile<AccountDataModel>("Data", "Account");
                if (RocketData.IsRunning == "Running")
                {
                    Debug.WriteLine("here");
                    await Task.Run(ChromeDriverQuit);

                    timer.Stop();
                    timer.Reset();

                    RocketData.IsRunning = "NotRunning";
                }
                else if (RocketData.SettingUp == "SettingUp")
                {
                    var notificationManager = new NotificationManager(NotificationPosition.TopRight);

                    await notificationManager.ShowAsync(
                    new NotificationContent { Title = "Error", Message = "Updating", Type = NotificationType.Error },

                    areaName: "WindowArea");
                }
                else if (timer.IsRunnign)
                {
                    timer.Stop();
                    timer.Reset();

                    Debug.WriteLine("timerhere");

                    RocketData.SettingUp = "SettingUp";

                    await Task.Run(ChromeDriver);

                    RocketData.SettingUp = "NotSettingUp";
                    RocketData.IsRunning = "Running";

                    await Task.Run(Element);

                    if (RocketData.Done == "Done")
                    {
                        timer.Start();
                    }
                }
                else if (!user.Email.Contains("@"))
                {
                    var notificationManager = new NotificationManager(NotificationPosition.TopRight);

                    await notificationManager.ShowAsync(
                    new NotificationContent { Title = $"Error", Message = $"Please inclued an '@' in the email address. {user.Email} is missing an '@'", Type = NotificationType.Error },

                    areaName: "WindowArea");
                }
                else if (user.Email.Last().ToString() == "@")
                {
                    var notificationManager = new NotificationManager(NotificationPosition.TopRight);

                    await notificationManager.ShowAsync(
                    new NotificationContent { Title = "Error", Message = $"Please enter a part of following '@' {user.Email} is incomplete.", Type = NotificationType.Error },

                    areaName: "WindowArea");
                }
                else
                {
                    if (user.Name != null && user.Email != null && user.Password != null)
                    {
                        Debug.WriteLine("normal");
                        RocketData.SettingUp = "SettingUp";

                        await Task.Run(ChromeDriver);

                        RocketData.SettingUp = "NotSettingUp";
                        RocketData.IsRunning = "Running";

                        await Task.Run(Element);

                        if (RocketData.Done == "Done")
                        {
                            timer.Start();
                        }

                        RocketData.IsRunning = "NotRunning";
                    }
                }
            }
            catch
            {
            }
        }

        private void TextUpdate(object sender, ElapsedEventArgs e)
        {
            try
            {
                TextUpdate();
                TimerDone();
            }
            catch
            {
            }
        }

        #endregion Buttons

        #region void classes

        private void TimerDone()
        {
            try
            {
                if (timer.TimeLeftMsStr == "00:00.000")
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        onoffbutton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    });

                    timer.Reset();
                }
            }
            catch
            {
            }
        }

        //timer
        private void startStatusBarTimer2()
        {
            try
            {
                System.Timers.Timer statusTime = new System.Timers.Timer();

                statusTime.Elapsed += new System.Timers.ElapsedEventHandler(TextUpdate);
                statusTime.Enabled = true;
            }
            catch
            {
            }
        }

        public void Write()
        {
            try
            {
                if (viewmodel.History != null && viewmodel.History.Count != 0)
                    if (RocketData.WhatDoing == viewmodel.History[viewmodel.History.Count - 1].name)
                        return;

                viewmodel.AddItems(new List<History>() { new History() { name = RocketData.WhatDoing, DateTime = DateTime.Now.ToString(), test = DateTime.Now.ToString("yyyy/MM/dd") } });

                Save.WriteToXmlFile<List<History>>(viewmodel.History.ToList(), "Data", "history");
            }
            catch
            {
            }
        }

        private void TextUpdate()
        {
            try
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
            catch
            {
            }
        }

        private void ChromeDriver()
        {
            try
            {
                RocketData.OnOff = "Updating";
                RocketData.Kind = MaterialDesignThemes.Wpf.PackIconKind.Update;

                Task.Run(Write).Wait();

                user = Save.ReadFromXmlFile<AccountDataModel>("Data", "Account");

                RocketData.WhatDoing = "Setting Up ChromeDrive";

                Task.Run(Write).Wait();

                Thread.Sleep(1000);

                new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
                RocketData.WhatDoing = "Setting Up Done, Starting Up ChromeDriver";

                Task.Run(Write).Wait();

                ChromeOptions options = new ChromeOptions();
                options.AddArgument("start-maximized");
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
                RocketData.TimeLabel = "Waiting To Finish This Task";
                RocketData.Kind = MaterialDesignThemes.Wpf.PackIconKind.Stop;

                Task.Run(Write);
            }
            catch
            {
            }
        }

        private void Element()
        {
            try
            {
                driver.FindElement(By.CssSelector("#acceptPrivacyPolicy")).Click();

                IWebElement loginbuttonpress = driver.FindElement(By.XPath("/html/body/header/section[1]/div/div[2]/div/a[1]"));

                RocketData.WhatDoing = "Accept Privacy Policy";

                loginbuttonpress.Click();

                RocketData.WhatDoing = "Accepted Privacy Policy";

                Task.Run(Write).Wait();

                Thread.Sleep(1000);

                IWebElement email = driver.FindElement(By.XPath("/html/body/main/main/section/div/div/div[1]/form/input[2]"));
                email.SendKeys(user.Email);
                RocketData.WhatDoing = "Email Entered";

                Task.Run(Write).Wait();

                Thread.Sleep(1000);

                IWebElement password = driver.FindElement(By.XPath("/html/body/main/main/section/div/div/div[1]/form/input[3]"));
                password.SendKeys(user.Password);
                RocketData.WhatDoing = "Password Entered";

                Task.Run(Write).Wait();

                Thread.Sleep(1000);

                password.SendKeys(Keys.Enter);
                RocketData.WhatDoing = "Login Button Clicked";

                Task.Run(Write).Wait();

                Thread.Sleep(1000);

                IWebElement eror = driver.FindElement(By.ClassName("rlg-site-popup__container"));
                string error = eror.Text;

                if (error.Contains("ERROR"))
                {
                    RocketData.WhatDoing = "Your email or password were not recognised";

                    Task.Run(Write).Wait();

                    ChromeDriverQuit();

                    return;
                }
                else
                {
                    IWebElement notificationperms = driver.FindElement(By.ClassName("rlg-notificationperms__decline"));
                    notificationperms.Click();
                    RocketData.WhatDoing = "Decline Notification";

                    Task.Run(Write).Wait();

                    Thread.Sleep(1000);

                    var trades = driver.FindElementsByClassName("rlg-trade__bump");
                    var closeup = driver.FindElementByXPath("/html/body/div[2]/div/span");
                    IWebElement closeuptext = driver.FindElementByClassName("rlg-site-popup__content");

                    int i = 1;
                    foreach (var trade in trades)
                    {
                        trade.Click();

                        Thread.Sleep(2000);

                        RocketData.Error = closeuptext.Text;

                        Debug.WriteLine(closeuptext.Text);

                        if (RocketData.Error.Contains("ERROR"))
                        {
                            RocketData.WhatDoing = RocketData.Error + " " + "Trade List" + " " + i;
                            Task.Run(Write).Wait();
                        }
                        else
                        {
                            RocketData.WhatDoing = $"Trade {i} Bumped!";
                            Task.Run(Write).Wait();
                        }                      
                        closeup.Click();

                        i++;

                        Thread.Sleep(3000);
                    }
                    RocketData.WhatDoing = $"Trade Bump for {trades.Count} Done";

                    Task.Run(Write).Wait();

                    Thread.Sleep(1000);

                    RocketData.WhatDoing = "Running After Timer Over!";

                    Task.Run(Write);
                }

                driver.Quit();
                RocketData.OnOff = "Not Running";
                RocketData.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;

                var color = (Color)ColorConverter.ConvertFromString("#FFFFFF");
                RocketData.Color = color;

                Task.Run(Write);

                var color2 = (Color)ColorConverter.ConvertFromString("#ffffff ");
                RocketData.Color = color2;

                RocketData.Done = "Done";
            }
            catch
            {
                ChromeDriverQuit();
            }
        }

        private void ChromeDriverQuit()
        {
            try
            {
                RocketData.WhatDoing = "Stopping Please Wait...";

                driver.Quit();

                var color2 = (Color)ColorConverter.ConvertFromString("#ffffff ");
                RocketData.Color = color2;

                RocketData.OnOff = "Not Running";
                RocketData.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                RocketData.WhatDoing = "Stopped";

                Task.Run(Write);
            }
            catch
            {
            }
        }

        private void Timer()
        {
            try
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
            catch
            {
            }
        }

        #endregion void classes
    }
}