using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using MvvmHelpers;

namespace Snake1.ViewModel
{
    public class ApplesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public struct _apple
        {
            public View view;
            public Coordinate position;
        };

        private List<Coordinate> apples;

        public List<Coordinate> Apples
        {
            get => apples;
            set
            {
                if (apples == value)
                    return;

                apples = value;
                OnPropertyChanged(nameof(Apples));
            }
        }

        public ApplesViewModel()
        {
            List<Coordinate> firstList = new List<Coordinate>();
            firstList.Add(new Coordinate(0.2, 0.2));

            Apples = firstList;
        }

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
