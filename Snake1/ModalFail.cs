using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Snake1
{
    public class ModalFail : ContentPage
    {
        public ModalFail()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text = "Fail!",
                        BackgroundColor=Color.White,
                    }
                }
            };
        }
    }
}