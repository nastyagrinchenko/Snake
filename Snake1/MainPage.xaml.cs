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

        public Size tailBoxSize = new Size(0.02, 0.01);

        public Size appleSize = new Size(0.02, 0.01);

        private int appleCount = 0;

        public Queue<ApplesViewModel._apple> apples = new Queue<ApplesViewModel._apple>();

        private Coordinate currentPosition = new Coordinate(0.5, 0.5);

        private Queue<SnakeViewModel._part> tail = new Queue<SnakeViewModel._part>();

        public MainPage()
        {
            InitializeComponent();

            updateApples(true);
        }

        private void start_button_Clicked(object sender, EventArgs e)
        {
            if (Accelerometer.IsMonitoring)
                return;

            updateApples(true);

            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Accelerometer.Start(SensorSpeed.UI);
        }
        
        private void stop_button_Clicked(object sender, EventArgs e)
        {
            resetSnake();

            if (!Accelerometer.IsMonitoring)
                return;

            Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
            Accelerometer.Stop();
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;

            double remeberHeadX = currentPosition.X;
            double remeberHeadY = currentPosition.Y;

            currentPosition.X += -1 * data.Acceleration.X / 40;
            currentPosition.Y += data.Acceleration.Y / 40;

            if (checkHeadBounds())
            {
                if (haveEaten())
                {
                    updateApples();
                    drawTail(new Coordinate(remeberHeadX, remeberHeadY), true);
                    appleCount++;
                }
                else
                {
                    drawTail(new Coordinate(remeberHeadX, remeberHeadY), false);
                }
            }
            else
            {
                stopGame();
                resetSnake();
            }

            LabelPos.Text = $"Apple count {appleCount}";
        }

        private Coordinate genApplePosition()
        {
            Random r = new Random();
            Coordinate pos = new Coordinate();

            int minValue = 11;
            int maxValue = 33;

            do
                pos = new Coordinate(r.Next(minValue, maxValue) / (maxValue + 0.5), r.Next(minValue, maxValue) / (maxValue + 0.5));
            while (new Rectangle(new Point(pos.X, pos.Y), appleSize).IntersectsWith(wallUp.Bounds) && new Rectangle(new Point(pos.X, pos.Y), appleSize).IntersectsWith(wallDown.Bounds));

            return pos;
        }

        private bool checkHeadBounds()
        {
            if (currentPosition > new Coordinate(0, 0) && currentPosition < new Coordinate(1, 1))
            {
                if (Head.Bounds.IntersectsWith(wallUp.Bounds))
                    return false;
                else
                {
                    if (Head.Bounds.IntersectsWith(wallDown.Bounds))
                        return false;
                    else
                        return true;
                }
            }
            else
                return false;
        }

        private async void stopGame()
        {
            Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
            Accelerometer.Stop();
            resetSnake();

            await Navigation.PushModalAsync(new ModalFail(appleCount));

            appleCount = 0;
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
                view = new BoxView { CornerRadius = 10, BackgroundColor = Color.Azure },
                position = genApplePosition(),
            };

            apples.Enqueue(ap);

            absLayout.Children.Add(apples.Peek().view, new Rectangle(new Point(apples.Peek().position.X, apples.Peek().position.Y), appleSize), AbsoluteLayoutFlags.All);
        }

        private void drawTail(Coordinate remeberHead, bool add = false)
        {
            if (add)
            {
                tail.Enqueue(new SnakeViewModel._part
                {
                    view = new BoxView { CornerRadius = 10, BackgroundColor = Color.Orange },
                    position = new Coordinate(),
                });
                absLayout.Children.Add(tail.ElementAt(tail.Count - 1).view, new Rectangle(new Point(tail.ElementAt(tail.Count - 1).position.X, tail.ElementAt(tail.Count - 1).position.Y), tailBoxSize), AbsoluteLayoutFlags.All);
                moveSnake(remeberHead);
            }
            else
            {
                moveSnake(remeberHead);
            }
        }

        private void moveSnake(Coordinate remeberHead)
        {
            int i = 0;
            Queue<SnakeViewModel._part> dublicate = new Queue<SnakeViewModel._part>();

            if (tail.Count == 0)
            {
                AbsoluteLayout.SetLayoutBounds(Head, new Rectangle(new Point(currentPosition.X, currentPosition.Y), boxSize));
                return;
            }

            Queue<SnakeViewModel._part> tempTail = tail;

            foreach (SnakeViewModel._part part in tail)
            {
                if (i == 0)
                {
                    dublicate.Enqueue(new SnakeViewModel._part { view = part.view, position = remeberHead });
                    AbsoluteLayout.SetLayoutBounds(part.view, new Rectangle(new Point(remeberHead.X, remeberHead.Y), tailBoxSize));
                    AbsoluteLayout.SetLayoutBounds(Head, new Rectangle(new Point(currentPosition.X, currentPosition.Y), boxSize));
                    i++;
                }
                else
                {
                    dublicate.Enqueue(new SnakeViewModel._part { view = part.view, position = tail.ElementAt(i - 1).position });
                    AbsoluteLayout.SetLayoutBounds(part.view, new Rectangle(new Point(tail.ElementAt(i - 1).position.X, tail.ElementAt(i - 1).position.Y), tailBoxSize));
                    i++;
                }
            }

            tail = dublicate;
        }

        private bool haveEaten()
        {
            foreach (ApplesViewModel._apple apple in apples)
            {
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

            foreach (SnakeViewModel._part part in tail)
            {
                absLayout.Children.Remove(part.view);
            }
            tail.Clear();

            foreach (ApplesViewModel._apple part in apples)
            {
                absLayout.Children.Remove(part.view);
            }

            apples.Clear();

        }
    }
}
