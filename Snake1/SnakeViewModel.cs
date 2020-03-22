using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Snake1.ViewModel
{
    public class SnakeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public struct _part
        {
            public View view;
            public Coordinate position;
        };

        public _part Part;

        private List<_part> _tail;

        public List<_part> Tail
        {
            get => _tail;

            set
            {
                if (_tail == value)
                    return;

                _tail = value;
                OnPropertyChanged(nameof(Tail));
            }
        }

        public int r = 2;

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public SnakeViewModel()
        {
            Part = new _part();
            Tail = new List<_part>();
        }

    }
}
