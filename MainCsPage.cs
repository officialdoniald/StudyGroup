using System;
using Xamarin.Forms;

namespace FacebookLogin.Views
{
    public class MainCsPage : ContentPage
    {

        public MainCsPage()
        {
            Title = "Study Group";
            BackgroundImage = "background.png";
            var loginButton = new Button
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Login with Facebook",
                BackgroundColor = Color.FromHex("#4E8DB9"),
                TextColor = Color.White
            };
            //Image im = new Image() {
            //    Source = ImageSource.FromFile("facebook.png"),
            //    HeightRequest = 100,
            //    WidthRequest = 100,
            //    Aspect = Aspect.AspectFit
            //};
            //var tapGesture = new TapGestureRecognizer();
            //tapGesture.Tapped += LoginWithFacebook_Clicked;
            //tapGesture.NumberOfTapsRequired = 1;
            //im.GestureRecognizers.Add(tapGesture);

            loginButton.Clicked += LoginWithFacebook_Clicked;

            Content = new StackLayout
            {
                
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        HorizontalTextAlignment = TextAlignment.Center,
                        Text = "You have to login into your Facebook account. We will use your profile picture, name and age.",
                        FontSize = 15,
                        TextColor = Color.White
                    },
                    loginButton
                }
            };
        }

        #region click

        private async void LoginWithFacebook_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FacebookProfileCsPage());
            Navigation.RemovePage(this);
        }
        
        #endregion
    }
}