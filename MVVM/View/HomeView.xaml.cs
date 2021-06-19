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
        private bool button1WasClicked = false;
        private bool done = false;
        private bool start = false;
        private string ErrorMaybe;
        private ChromeDriver driver;
        private CountDownTimer timer = new CountDownTimer();
        private System.Timers.Timer statusTime = new System.Timers.Timer();
        private System.Timers.Timer statusTime2 = new System.Timers.Timer();

        public HomeView()
        {
            InitializeComponent();
            check();
            startStatusBarTimer();
            startStatusBarTimer2();
        }

        //timer
        private void startStatusBarTimer2()
        {
            try
            {
                statusTime2.Interval = 1;
                statusTime2.Elapsed += new System.Timers.ElapsedEventHandler(text);
                statusTime2.Enabled = true;
            }
            catch
            {
            }
        }

        private void text(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                whatdoing.Text = RocketData.WhatDoing;
                onoff.Content = RocketData.OnOff;
                timelabel.Text = RocketData.TimeLabel;
            });
        }

        //timer
        private void startStatusBarTimer()
        {
            try
            {
                statusTime.Interval = 1000;
                statusTime.Elapsed += new System.Timers.ElapsedEventHandler(data);
                statusTime.Enabled = false;
            }
            catch
            {
            }
        }

        private void timerr()
        {
            try
            {
                //set to 30 mins
                timer.SetTime(20, 0);

                //update label text
                timer.TimeChanged += () =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        timelabel.Text = RocketData.TimeLabel = timer.TimeLeftMsStr + " " + "Minute";
                    });
                };

                // show messageBox on timer = 00:00.000
                timer.CountDownFinished += () => MessageBox.Show("Timer finished the work!");

                //timer step. By default is 1 second
                timer.StepMs = 77; // for nice milliseconds time switch
            }
            catch
            {
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (whatdoing.Text == "Downloading ChromeDriver")
            {
                onoff.Content = RocketData.OnOff = "Wait For download";
                return;
            }
            try
            {
                await Task.Run(async () =>
                {
                    user = Save.ReadFromXmlFile<AccountDataModel>("Data", "Account");
                    if (user.Email != null && user.Password != null && user.Name != null)
                    {
                        try
                        {
                            if (Process.GetProcesses().Count(p => p.ProcessName == "chromedriver") == 0 || start == true)
                            {
                                statusTime.Enabled = true;
                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = RocketData.WhatDoing = "starting";
                                    onoff.Content = RocketData.OnOff = "Running";
                                    icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Stop;
                                });
                                button1WasClicked = true;
                                new DriverManager().SetUpDriver(new ChromeConfig());

                                ChromeOptions options = new ChromeOptions();
                                options.AddArgument("headless");

                                ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
                                driverService.HideCommandPromptWindow = true;

                                driver = new ChromeDriver(driverService, options)
                                {
                                    Url = $"https://rocket-league.com/trades/{user.Name}"
                                };

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = RocketData.WhatDoing = "Started";
                                    onoff.Content = RocketData.OnOff = "Running";
                                    icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Stop;
                                });

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = RocketData.WhatDoing = "Accepting PrivacyPolicy ";
                                });

                                driver.FindElement(By.CssSelector("#acceptPrivacyPolicy")).Click();

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = RocketData.WhatDoing = "Accepted PrivacyPolicy";
                                });
                                Thread.Sleep(1000);
                                IWebElement email = driver.FindElement(By.CssSelector("#header-email"));

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = RocketData.WhatDoing = "Entering Email address";
                                });
                                Thread.Sleep(1000);
                                email.SendKeys(user.Email);

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = RocketData.WhatDoing = "Email address entered";
                                });
                                Thread.Sleep(1000);
                                IWebElement password = driver.FindElement(By.CssSelector("#header-password"));

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = RocketData.WhatDoing = "Entering Password";
                                });
                                Thread.Sleep(1000);
                                password.SendKeys(user.Password);

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = RocketData.WhatDoing = "Password entered";
                                });
                                Thread.Sleep(1000);

                                try
                                {
                                    password.SendKeys(Keys.Enter);
                                }
                                catch
                                {
                                }

                                IWebElement eror = driver.FindElement(By.ClassName("rlg-site-popup__container"));
                                string error = eror.Text;
                                Debug.WriteLine(error);
                                if (error.Contains("ERROR"))
                                {
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        whatdoing.Text = RocketData.WhatDoing = "Your email or password were not recognised";
                                        RocketData.OnOff = "Not Running";
                                        icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                                    });

                                    driver.Quit();
                                    return;
                                }

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = RocketData.WhatDoing = "Login Button pressed";
                                });
                                Thread.Sleep(1000);

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = RocketData.WhatDoing = "Logged in";
                                });

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = RocketData.WhatDoing = "Decline notificationperms";
                                });
                                IWebElement notificationperms = driver.FindElement(By.ClassName("rlg-notificationperms__decline"));
                                notificationperms.Click();

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = RocketData.WhatDoing = "Declined";
                                });

                                Thread.Sleep(1000);

                                var trades = driver.FindElementsByClassName("rlg-trade__bump");
                                var closeup = driver.FindElement(By.ClassName("rlg-site-popup__container"));

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = RocketData.WhatDoing = "Going to trades";
                                });

                                Thread.Sleep(1000);

                                int i = 0;

                                foreach (var trade in trades)
                                {
                                    trade.Click();

                                    Thread.Sleep(1000);

                                    closeup.Click();

                                    ErrorMaybe = closeup.Text;
                                    Debug.WriteLine(ErrorMaybe);

                                    this.Dispatcher.Invoke(() =>
                                    {
                                        if (ErrorMaybe.Contains("ERROR"))
                                        {
                                            whatdoing.Text = RocketData.WhatDoing = ErrorMaybe + " " + "Trade List" + " " + i;
                                        }
                                        else
                                        {
                                            whatdoing.Text = RocketData.WhatDoing = $"Trade {i} Bumped!";
                                        }
                                    });

                                    Thread.Sleep(1000);

                                    i++;
                                }

                                Thread.Sleep(1000);

                                this.Dispatcher.Invoke(() =>
                                {
                                    if (ErrorMaybe.Contains("ERROR"))
                                    {
                                        whatdoing.Text = RocketData.WhatDoing = $"Bump For All the Trades {i} Not Done!";
                                    }
                                    else
                                    {
                                        whatdoing.Text = RocketData.WhatDoing = $"Bump For All the Trades {i} Done!";
                                    }
                                });

                                this.Dispatcher.Invoke(() =>
                                {
                                    onoff.Content = RocketData.OnOff = "Running after timer done!";
                                    icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                                });

                                done = true;
                                driver.Quit();
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = RocketData.WhatDoing = "Stopped";
                                    onoff.Content = RocketData.OnOff = "Not Running";
                                    icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                                });

                                if (driver != null)
                                {
                                    driver.Quit();
                                }
                                else
                                {
                                    foreach (Process process in Process.GetProcessesByName("chromedriver"))
                                    {
                                        process.Kill();
                                    }
                                }
                            }
                        }
                        catch
                        {
                            whatdoing.Text = RocketData.WhatDoing = "Check internet connection and try again";
                            driver.Quit();
                            return;
                        }
                    }
                    else
                    {
                        var notificationManager = new NotificationManager();

                        await notificationManager.ShowAsync(
                        new NotificationContent { Title = "Login", Message = "Please enter Your Rocket Garage Info" },
                        areaName: "WindowArea");
                    }
                });
            }
            catch
            {
                whatdoing.Text = RocketData.WhatDoing = "Check internet connection and try again";
                if (driver != null)
                {
                    driver.Quit();
                    return;
                }
            }
        }

        private void data(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (button1WasClicked == true && done == true)
                {
                    statusTime.Start();
                    timerr();
                    timer.Start();
                    if (timer.TimeLeftMsStr == "00:00.000" && Process.GetProcesses().Count(p => p.ProcessName == "chromedriver") == 0)
                    {
                        done = false;
                        timer.Reset();
                        Debug.WriteLine("clicked");
                        this.Dispatcher.Invoke(() =>
                        {
                            start = true;
                            onoffbutton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                        });
                    }
                    else if (Process.GetProcesses().Count(p => p.ProcessName == "chromedriver") > 1)
                    {
                        start = false;
                    }
                }
            }
            catch
            {
            }
        }

        private async void check()
        {
            try
            {
                if (Process.GetProcesses().Count(p => p.ProcessName == "chromedriver") == 0)
                {
                    timelabel.Text = RocketData.TimeLabel = "Not Running, No Bump :(";

                    whatdoing.Text = RocketData.WhatDoing = "Downloading ChromeDriver";
                    await Task.Run(() => new DriverManager().SetUpDriver(new ChromeConfig()));
                    whatdoing.Text = RocketData.WhatDoing = "Download done";
                    onoff.Content = RocketData.OnOff = "Not Running";
                }
                else
                {
                    icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Stop;
                }
            }
            catch
            {
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RocketData.WhatDoing = "Restarting";
            RocketData.OnOff = "Not Running";
            if (driver != null)
            {
                driver.Quit();
            }

            onoffbutton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void onoffbutton_MouseEnter(object sender, MouseEventArgs e)
        {
            var color = (Color)ColorConverter.ConvertFromString("#0e0e0e");
            buttonplay.Color = color;
            buttonplay2.Color = color;
            buttonplay2.Offset = 0.0;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            var color = (Color)ColorConverter.ConvertFromString("#141414");
            buttonplay.Color = color;
            buttonplay2.Color = color;
            buttonplay2.Offset = 1;
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            var color = (Color)ColorConverter.ConvertFromString("#0e0e0e");
            button3.Color = color;
            button4.Color = color;
            button4.Offset = 0.0;
        }

        private void Grid_MouseLeave_1(object sender, MouseEventArgs e)
        {
            var color = (Color)ColorConverter.ConvertFromString("#141414");
            button3.Color = color;
            button4.Color = color;
            button4.Offset = 1;
        }
    }
}