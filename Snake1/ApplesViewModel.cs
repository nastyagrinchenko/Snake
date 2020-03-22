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

        public ApplesViewModel()
        {

        }

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
