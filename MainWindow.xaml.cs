using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace RocketLeagueGarage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotifyIcon notifyIcon = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        //exit app
        private void ExitClicked(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
            Environment.Exit(0);
        }

        //minimize app
        private void MinimizeClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                initialTray();
            }
            catch
            {
            }
        }

        //move system window
        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        // Minimize the system tray
        private void initialTray()
        {
            try
            {
                notifyIcon = new NotifyIcon();

                // Hide the main form
                this.Visibility = Visibility.Hidden;
                // Set the various properties of the tray

                // tray bubble display content
                notifyIcon.BalloonTipText = "Rocket League Garage is running...";
                notifyIcon.Text = "Rocket League Garage";

                //The tray button is visible
                notifyIcon.Visible = true;

                //The icon displayed in the tray
                notifyIcon.Icon = new System.Drawing.Icon("RocketLeagueGarage.ico");

                //Tray bubble display time
                notifyIcon.ShowBalloonTip(1000);

                //mouse click event
                notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);

                // triggered when the form state changes
                this.StateChanged += MainWindow_StateChanged;
            }
            catch
            {
            }
        }

        // tray icon mouse click event
        private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                // left mouse button to minimize the form to hide or display the form
                if (e.Button == MouseButtons.Left)
                {
                    if (this.Visibility == Visibility.Visible)
                    {
                        this.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        notifyIcon.Dispose();
                        this.Visibility = Visibility.Visible;
                        this.Activate();
                    }
                }
                if (e.Button == MouseButtons.Right)
                {
                    //object sender = new object();
                    // EventArgs e = new EventArgs();
                    exit_Click(sender, e);// Trigger click to exit the event
                }
            }
            catch
            {
            }
        }

        // exit option
        private void exit_Click(object sender, EventArgs e)
        {
            try
            {
                if (System.Windows.MessageBox.Show("Are you sure to quit?",
                                            "RLG",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question,
                                             MessageBoxResult.Yes) == MessageBoxResult.Yes)
                {
                    //System.Windows.Application.Current.Shutdown();
                    System.Environment.Exit(0);
                }
            }
            catch
            {
            }
        }

        // window state changes, minimize tray
        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState == WindowState.Minimized)
                {
                    this.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
            }
        }
    }
}