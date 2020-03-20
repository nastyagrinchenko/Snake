using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Snake1
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private Size boxSize = new Size(22, 22);

        private Size appleSize = new Size(10, 10);

        private List<Coordinate> apples = new List<Coordinate>();

        private Coordinate currentPosition = new Coordinate(0.5, 0.5);

        private List<double> medianWindowX = new List<double> { 0, 0, 0, 0, 0, 0, 0 };
        private List<double> medianWindowY = new List<double> { 0, 0, 0, 0, 0, 0, 0 };

        public MainPage()
        {
            InitializeComponent();

            apples.Add(genApplePosition());
        }

        private void start_button_Clicked(object sender, EventArgs e)
        {
            if (Accelerometer.IsMonitoring)
                return;

            this.currentPosition = new Coordinate();
            AbsoluteLayout.SetLayoutBounds(Head, new Rectangle(new Point(currentPosition.X, currentPosition.Y), boxSize));

            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Accelerometer.Start(SensorSpeed.UI);
        }
        
        private void stop_button_Clicked(object sender, EventArgs e)
        {
            if (!Accelerometer.IsMonitoring)
                return;

            Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
            Accelerometer.Stop();
        }
        
        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;

            this.currentPosition.X += this.medianFilterX(200 * data.Acceleration.X);
            this.currentPosition.Y += this.medianFilterY(200 * data.Acceleration.Y);

            if (checkHeadBounds())
                AbsoluteLayout.SetLayoutBounds(Head, new Rectangle(new Point(currentPosition.X, currentPosition.Y), boxSize));
            else
                stopGame();

            LabelAcc.Text = $"X {data.Acceleration.X.ToString()} Y {data.Acceleration.Y.ToString()}";
            LabelPos.Text = currentPosition.ToString();
        }

        private Coordinate genApplePosition()
        {
            Coordinate possible = new Coordinate();
            do
            {
                possible = new Coordinate(new Random().Next(0, 1), new Random().Next(0, 1));
            }
            while (apples.Contains(possible));
            
            return possible;
        }

        private double medianFilterX(double value)
        {
            medianWindowX.RemoveAt(medianWindowX.Count - 1);
            medianWindowX.Add(value);
            medianWindowX.Sort((a, b) => a.CompareTo(b));

            return medianWindowX[4];
        }

        private double medianFilterY(double value)
        {
            medianWindowY.RemoveAt(medianWindowY.Count - 1);
            medianWindowY.Add(value);
            medianWindowY.Sort((a, b) => a.CompareTo(b));

            return medianWindowY[4];
        }

        private bool checkHeadBounds()
        {
            return currentPosition > 0 && currentPosition < 1;
        }

        private async void stopGame()
        {
            await Navigation.PushModalAsync(new ModalFail());
            Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
            Accelerometer.Stop();
        }
    }
}
