using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Snake1.ViewModel;
using System.Collections.ObjectModel;

namespace Snake1
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public Size boxSize = new Size(0.04, 0.02);

        public Size appleSize = new Size(0.02, 0.01);

        int delay = 0;

        public Queue<ApplesViewModel._apple> apples = new Queue<ApplesViewModel._apple>();

        private Coordinate currentPosition = new Coordinate(0.5, 0.5);

        private Queue<SnakeViewModel._part> tail = new Queue<SnakeViewModel._part>();

        private int tailSize = 1;

        SnakeViewModel SnakeTailView = new SnakeViewModel();
        ApplesViewModel ApplesView = new ApplesViewModel();

        public MainPage()
        {
            InitializeComponent();
            SnakeViewModel._part part = new SnakeViewModel._part
            {
                view = new BoxView { BackgroundColor = Color.Orange, CornerRadius = 10 },
                position = currentPosition + 0.004,
            };
            SnakeViewModel._part part2 = new SnakeViewModel._part
            {
                view = new BoxView { BackgroundColor = Color.Black, CornerRadius = 10 },
                position = currentPosition + 0.008,
            };

            tail.Enqueue(part);
            tail.Enqueue(part2);
            absLayout.Children.Add(part.view, new Rectangle(new Point(part.position.X, part.position.Y), boxSize), AbsoluteLayoutFlags.All);
            absLayout.Children.Add(part2.view, new Rectangle(new Point(part2.position.X, part2.position.Y), boxSize), AbsoluteLayoutFlags.All);

            updateApples(true);
        }

        private void start_button_Clicked(object sender, EventArgs e)
        {
            if (Accelerometer.IsMonitoring)
                return;

            resetSnake();

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
            delay++;

            if (delay == 2)
            {
                Coordinate remeberHead = currentPosition;

                currentPosition.X += -1 * data.Acceleration.X / 30;
                currentPosition.Y += data.Acceleration.Y / 30;

                if (checkHeadBounds())
                {
                    if (haveEaten())
                    {
                        updateApples();
                        //updateTail(true);
                    }
                    else
                    {
                        updateTail(false, remeberHead);
                        //AbsoluteLayout.SetLayoutBounds(Head, new Rectangle(new Point(currentPosition.X, currentPosition.Y), boxSize));
                    }
                }
                else
                {
                    stopGame();
                }


                LabelAcc.Text = $"X {data.Acceleration.X.ToString()} Y {data.Acceleration.Y.ToString()}";
                LabelPos.Text = currentPosition.ToString();
                delay = 0;
            }
        }

        private Coordinate genApplePosition()
        {
            Random r = new Random();

            int minValue = 11;
            int maxValue = 33;

            return new Coordinate(r.Next(minValue, maxValue) / (maxValue + 0.5), r.Next(minValue, maxValue) / (maxValue + 0.5));
        }

        private bool checkHeadBounds()
        {
            return currentPosition > new Coordinate() && currentPosition < new Coordinate(1,1);
        }

        private async void stopGame()
        {
            Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
            Accelerometer.Stop();
            resetSnake();

            await Navigation.PushModalAsync(new ModalFail());
        }

        private void updateTail(bool add = false, Coordinate remeberHead = null)
        {
            //SnakeViewModel._part part = new SnakeViewModel._part
            //{
            //    view = new BoxView { BackgroundColor = Color.Orange, CornerRadius=10 },
            //    position = currentPosition,
            //};

            //tail.Enqueue(part);
            if (!add)
            {
                //tail.Dequeue();
                drawTail(false, remeberHead);    
            }
            else
                drawTail(true);

        }

        private void updateApples(bool init = false)
        {
            if (!init)
            {
                absLayout.Children.Remove(apples.Peek().view);
                apples.Dequeue();
            }

            ApplesViewModel._apple ap = new ApplesViewModel._apple
            {
                view = new BoxView { CornerRadius = 10, BackgroundColor = Color.Green },
                position = genApplePosition(),
            };

            apples.Enqueue(ap);

            absLayout.Children.Add(apples.Peek().view, new Rectangle(new Point(apples.Peek().position.X, apples.Peek().position.Y), appleSize), AbsoluteLayoutFlags.All);
        }

        private void drawTail(bool add = false, Coordinate remeberHead = null)
        {
            if (add)
            {
                //tailView.Add(new BoxView
                //{
                //    CornerRadius = 10,
                //    BackgroundColor = Color.Red
                //});
                //absLayout.Children.Add(tailView.ElementAt(tailView.Count-1));
                moveSnake();
            }
            else
            {
                moveSnake(remeberHead);
            }
        }

        private void moveSnake(Coordinate remeberHead = null)
        {
            int i = 0;

            if (tail.Count == 0)
            {
                AbsoluteLayout.SetLayoutBounds(Head, new Rectangle(new Point(currentPosition.X, currentPosition.Y), boxSize));
                return;
            }

            Queue<SnakeViewModel._part> tempTail = tail;

            //tail.Peek().position = new Coordinate();
            foreach (SnakeViewModel._part part in tail)
            {
                if (i == 0)
                {
                    part.position.X = remeberHead.X;
                    part.position.Y = remeberHead.Y;
                    AbsoluteLayout.SetLayoutBounds(part.view, new Rectangle(new Point(part.position.X, part.position.Y), boxSize));
                    AbsoluteLayout.SetLayoutBounds(Head, new Rectangle(new Point(currentPosition.X, currentPosition.Y), boxSize));
                    i++;
                }
                else
                {
                    part.position.X = tempTail.ElementAt(i - 1).position.X;
                    part.position.Y = tempTail.ElementAt(i - 1).position.Y;
                    AbsoluteLayout.SetLayoutBounds(part.view, new Rectangle(new Point(part.position.X, part.position.Y), boxSize));
                    i++;
                }
            }
        }

        private bool haveEaten()
        {
            //SnakeViewModel._part nw;
            //nw.view = new BoxView { BackgroundColor = Color.Yellow };
            //nw.position = new Coordinate();

            //List<SnakeViewModel._part>  nwl = new List<SnakeViewModel._part>();
            //nwl.Add(nw);
            //SnakeTailView.Tail = nwl;

            foreach (ApplesViewModel._apple apple in apples)
            {
                double appleHieght = apple.position.Y + boxSize.Height;
                double appleWidth = apple.position.X + boxSize.Width;

                if (currentPosition > apple.position - boxSize.Height / 2 && currentPosition < apple.position + boxSize.Height / 2)
                    return true;
                else
                    continue;
            }

            return false;
        }

        private void resetSnake()
        {
            currentPosition = new Coordinate(0.5, 0.5);
            AbsoluteLayout.SetLayoutBounds(Head, new Rectangle(new Point(currentPosition.X, currentPosition.Y), boxSize));
        }
    }
}
