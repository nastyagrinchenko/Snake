using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private Coordinate currentPosition = new Coordinate(0, 0);

        private int boxSize = 22;

        public MainPage()
        {
            InitializeComponent();

            AbsoluteLayout absoluteLayout = new AbsoluteLayout
            {
                BackgroundColor = Color.Aqua,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            absoluteLayout.Children.Add(
                new BoxView {
                    Color = Color.Red,
                    AutomationId = "1",
                    CornerRadius = 5,
                },
                new Rectangle(currentPosition.X, currentPosition.Y, boxSize, boxSize));

            Content = new StackLayout
            {
                Children = {
                    absoluteLayout,
                },
            };
        }

        private void start_button_Clicked(object sender, EventArgs e)
        {
            if (Accelerometer.IsMonitoring)
                return;

            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Accelerometer.Start(SensorSpeed.UI);
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            Console.WriteLine($"X: {data.Acceleration.X}, Y: {data.Acceleration.Y}, Z: {data.Acceleration.Z}");
        }

        private void stop_button_Clicked(object sender, EventArgs e)
        {
            if (!Accelerometer.IsMonitoring)
                return;

            Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
            Accelerometer.Stop();
        }
    }
}
