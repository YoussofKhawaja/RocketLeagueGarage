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

        public HomeView()
        {
            InitializeComponent();
            check();
            startStatusBarTimer();
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

                timer.Start();

                //update label text
                timer.TimeChanged += () =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        timelabel.Text = timer.TimeLeftMsStr + " " + "Minute";
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
                onoff.Content = "Wait For download";
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
                                    whatdoing.Text = "starting";
                                    onoff.Content = "Running";
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
                                    whatdoing.Text = "Started";
                                    onoff.Content = "Running";
                                    icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Stop;
                                });

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = "Accepting PrivacyPolicy ";
                                });

                                driver.FindElement(By.CssSelector("#acceptPrivacyPolicy")).Click();

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = "Accepted PrivacyPolicy";
                                });
                                Thread.Sleep(1000);
                                IWebElement email = driver.FindElement(By.CssSelector("#header-email"));

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = "Entering Email address";
                                });
                                Thread.Sleep(1000);
                                email.SendKeys(user.Email);

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = "Email address entered";
                                });
                                Thread.Sleep(1000);
                                IWebElement password = driver.FindElement(By.CssSelector("#header-password"));

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = "Entering Password";
                                });
                                Thread.Sleep(1000);
                                password.SendKeys(user.Password);

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = "Password entered";
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
                                        whatdoing.Text = "Your email or password were not recognised";
                                        onoff.Content = "Not Running";
                                        icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                                    });

                                    driver.Quit();
                                    return;
                                }

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = "Login Button pressed";
                                });
                                Thread.Sleep(1000);

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = "Logged in";
                                });

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = "Decline notificationperms";
                                });
                                IWebElement notificationperms = driver.FindElement(By.ClassName("rlg-notificationperms__decline"));
                                notificationperms.Click();

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = "Declined";
                                });

                                Thread.Sleep(1000);

                                var trades = driver.FindElementsByClassName("rlg-trade__bump");
                                var closeup = driver.FindElement(By.ClassName("rlg-site-popup__container"));

                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = "Going to trades";
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
                                            whatdoing.Text = ErrorMaybe + " " + "Trade List" + " " + i;
                                        }
                                        else
                                        {
                                            whatdoing.Text = $"Trade {i} Bumped!";
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
                                        whatdoing.Text = $"Bump For All the Trades {i} Not Done!";
                                    }
                                    else
                                    {
                                        whatdoing.Text = $"Bump For All the Trades {i} Done!";
                                    }
                                });

                                this.Dispatcher.Invoke(() =>
                                {
                                    onoff.Content = "Running after timer done!";
                                    icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                                });

                                done = true;
                                driver.Quit();
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    whatdoing.Text = "Stopped";
                                    onoff.Content = "Not Running";
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
                            whatdoing.Text = "Check internet connection and try again";
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
                whatdoing.Text = "Check internet connection and try again";
                driver.Quit();
                return;
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
                //user = Save.ReadFromXmlFile<AccountDataModel>("data", "account");
                //if (user.Email != null && user.Password != null && user.Name != null)
                //{
                //    txtname.IsReadOnly = true;
                //    txtemail.IsReadOnly = true;
                //    txtpassword.IsReadOnly = true;
                //    buttonpasswors.Visibility = Visibility.Collapsed;
                //    passwordcheck.Visibility = Visibility.Collapsed;

                //    txtemail.Text = user.email;
                //    txtname.Text = user.name;
                //    txtpassword.Text = new string('*', user.password.Length);
                //}

                whatdoing.Text = "Downloading ChromeDriver";
                timelabel.Text = "Not Running";

                await Task.Run(() => new DriverManager().SetUpDriver(new ChromeConfig()));

                if (Process.GetProcesses().Count(p => p.ProcessName == "chromedriver") == 1)
                {
                    timelabel.Text = "Time for next bump starting!";
                    whatdoing.Text = "starting";
                    onoff.Content = "Running";
                }
                else if (Process.GetProcesses().Count(p => p.ProcessName == "chromedriver") == 0)
                {
                    timelabel.Text = "Not Running, No Bump :(";
                    whatdoing.Text = "Doing Nothing";
                    onoff.Content = "Not Running";
                }
            }
            catch
            {
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }
    }
}