using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Snake1
{
    public class ModalFail : ContentPage
    { 
        public ModalFail(int count)
        {
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor=Color.White,
                Children = {
                    new Label {
                        Text = "Game over \nYour score : " + count,
                        FontSize=22,
                        VerticalOptions=LayoutOptions.CenterAndExpand,
                        HorizontalOptions=LayoutOptions.Center,
                        BackgroundColor=Color.White,
                        TextColor=Color.Red,
                    }
                }
            };
        }
    }
}