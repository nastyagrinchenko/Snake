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
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    new Label {
                        Text = "Fail!",
                        FontSize=22,
                        VerticalOptions=LayoutOptions.CenterAndExpand,
                        BackgroundColor=Color.White,
                        TextColor=Color.Red,
                    }
                }
            };
        }
    }
}