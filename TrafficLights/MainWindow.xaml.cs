using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace TrafficLights
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly int MIN_LIGHTS = 2;
        static readonly int MAX_LIGHTS = 4;
        static readonly int MIN_CARS = 1;
        static readonly int MAX_CARS = 5;

        DispatcherTimer lightsTimer = new DispatcherTimer();

        int currentLight = 0;
        int numberOfCars = 2;
        int numberOfLights = 2;
        int lightTimerVal = 0;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            lightTimerVal = 2000 + (numberOfCars * 1000);
            lightsTimer.Tick += new EventHandler(LightsTimer_Tick);
            lightsTimer.Interval = new TimeSpan(0, 0, 0, 0, lightTimerVal);
            lightsTimer.Start();

            masterLight.SlaveLight = slaveLight1;
            UpdateLights();
            UpdateCars();
        }

        private void LightsTimer_Tick(object sender, EventArgs e)
        {
            switch (currentLight++)
            {
                case 0:
                    masterLight.NextState();
                    break;

                case 1:
                    masterLight.SlaveLight.NextState();
                    break;

                case 2:
                    masterLight.SlaveLight.SlaveLight.NextState();
                    break;

                case 3:
                    masterLight.SlaveLight.SlaveLight.SlaveLight.NextState();
                    break;
            }

            currentLight %= numberOfLights;
        }

        private void Button_DecClick(object sender, RoutedEventArgs e)
        {
            if (numberOfCars > MIN_CARS)
            {
                numberOfCars--;
                lightTimerVal = 2000 + (numberOfCars * 1000);
                lightsTimer.Interval = new TimeSpan(0, 0, 0, 0, lightTimerVal);
                UpdateCars();
            }
        }

        private void Button_IncClick(object sender, RoutedEventArgs e)
        {
            if (numberOfCars < MAX_CARS)
            {
                numberOfCars++;
                lightTimerVal = 2000 + (numberOfCars * 1000);
                lightsTimer.Interval = new TimeSpan(0, 0, 0, 0, lightTimerVal);
                UpdateCars();
            }
        }

        private void Button_DecLightsClick(object sender, RoutedEventArgs e)
        {
            if (numberOfLights > MIN_LIGHTS)
            {
                numberOfLights--;
                UpdateLights();
            }
        }

        private void Button_IncLightsClick(object sender, RoutedEventArgs e)
        {
            if (numberOfLights < MAX_LIGHTS)
            {
                numberOfLights++;
                UpdateLights();
            }
        }

        private void UpdateCars()
        {
            lblLightTimerVal.Content = "Timer Val(ms): " + lightTimerVal.ToString();
            lblNoCars.Content = "Number of Cars: " + numberOfCars.ToString();
            for (int i = 1; i <= MAX_CARS; i++)
            {
                string name = "CarRight" + i.ToString();
                object item = mainRoad.FindName(name);
                ((Image)item).Visibility = i < numberOfCars + 1 ? Visibility.Visible : Visibility.Hidden;

                name = "CarLeft" + i.ToString();
                item = mainRoad.FindName(name);
                ((Image)item).Visibility = i < numberOfCars + 1 ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void UpdateLights()
        {
            lblNoLights.Content = "Number of Lights: " + numberOfLights.ToString();
            switch (numberOfLights)
            {
                case 2:
                    masterLight.SlaveLight = slaveLight1;
                    slaveLight2.Visibility = Visibility.Hidden;
                    slaveLight3.Visibility = Visibility.Hidden;
                    break;

                case 3:
                    slaveLight1.SlaveLight = slaveLight2;
                    slaveLight2.Visibility = Visibility.Visible;
                    slaveLight3.Visibility = Visibility.Hidden;
                    break;

                case 4:
                    slaveLight1.SlaveLight = slaveLight2;
                    slaveLight2.Visibility = Visibility.Visible;
                    slaveLight2.SlaveLight = slaveLight3;
                    slaveLight3.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
