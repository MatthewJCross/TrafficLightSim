using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace TrafficLights.Views
{
    /// <summary>
    /// Interaction logic for TrafficLightView.xaml
    /// </summary>
    public partial class TrafficLightView : UserControl, INotifyPropertyChanged
    {
        private BitmapImage _imageSource;
        public BitmapImage ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }

        private int _lightState;
        public int LightState
        {
            get => _lightState;
            set
            {
                _lightState = value;
                OnPropertyChanged("LightState");
            }
        }

        private TrafficLightView _slaveLight;
        public TrafficLightView SlaveLight
        {
            get => _slaveLight;
            set
            {
                _slaveLight = value;

                _slaveLight.InitialiseSlave(masterLight, 2);
                OnPropertyChanged("SlaveLight");
            }
        }

        private void OnPropertyChanged(string v)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(v));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        DispatcherTimer stateTimer = new DispatcherTimer();
        private static TrafficLightView masterLight = null;
        private static int lightCount;
        private int id;

        public TrafficLightView()
        {
            DataContext = this;
            InitializeComponent();

            id = ++lightCount;
            LightState = 0;

            string imagePath = "\\Images\\greenlight.png";
            this.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));

            stateTimer.Tick += new EventHandler(StateTimer_Tick);
            stateTimer.Interval = new TimeSpan(0, 0, 0, 0, 333);

            if (masterLight == null)
                masterLight = this;
        }

        public void InitialiseSlave(TrafficLightView trafficLight, int State)
        {
            _slaveLight = trafficLight;
            LightState = State;
            string imagePath = "\\Images\\redlight.png";
            this.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

        public void NextState()
        {
            stateTimer.Start();
        }

        private void StateTimer_Tick(object sender, EventArgs e)
        {
            string imagePath = "\\Images\\";
            LightState++;
            switch (LightState)
            {
                case 0:
                    imagePath += "greenlight.png";
                    ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                    stateTimer.Stop();
                    break;

                case 1:
                    imagePath += "amberlight.png";
                    ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                    break;

                case 2:
                    imagePath += "redlight.png";
                    ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                    stateTimer.Stop();
                    SlaveLight.NextState();
                    break;

                default:
                    imagePath += "redamberlight.png";
                    ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                    LightState = -1;
                    break;
            }
        }
    }
}
