using FacebookLogin.Views;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.Connectivity;
using StudyGroup.FileStoring;
using StudyGroup.GetImageFromAzure;
using StudyGroup.Models;
using StudyGroup.Services;
using System;
using System.Diagnostics;
using System.IO;
using Xamarin.Forms;

namespace StudyGroup.Views
{
    public class ProfileUpdatePage : ContentPage
    {
        #region publicAttributes
        Label asweq = new Label() {
            Text = "asd"
        };
        Users t;

        #endregion
        public ProfileUpdatePage(string userId)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                this.Icon = "settings.png";
            }
            BackgroundImage = "background.png";
            Title = "Profile";
            t = new Users();
            t.Profile = userId;
            Button updateButton = new Button()
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Login again with Facebook",
                BackgroundColor = Color.FromHex("#4E8DB9"),
                TextColor = Color.White
            };

            Button sasButton = new Button()
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Get my sas and image",
                BackgroundColor = Color.FromHex("#4E8DB9"),
                TextColor = Color.White
            };

            Image d = new Image();
            
            updateButton.Clicked += UpdateButton_Clicked;
            sasButton.Clicked += SasButton_Clicked;
            Content = new ScrollView
            {
                Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                
                    Children = {
                    new Label
                    {
                        HorizontalTextAlignment = TextAlignment.Center,
                        Text = "Update your Facebook profile",
                        FontSize = 15,
                        TextColor = Color.White
                    },asweq,sasButton,
                     updateButton
                }
                }
            };
        }



        #region click
        private async void SasButton_Clicked(object sender, EventArgs e)
        {
            /*get the guid
             
                
             */
            var client = new MobileServiceClient("http://xamarinalliancebackend.azurewebsites.net/");
            var token = await client.InvokeApiAsync("/api/StorageToken/CreateToken");
            var guid = await client.InvokeApiAsync("/api/XamarinAlliance/ReceiveCredit");
            asweq.Text = guid.ToString();

            //var asdasd = DependencyService.Get<IImageFromAzure>().GetImageFromAzure(token.ToString());
            
        }
        private async void UpdateButton_Clicked(object sender, EventArgs e)
        {
            DatabaseFunctions dbf = new DatabaseFunctions();

            var userList = await dbf.getUsers();
            
            foreach (var item in userList)
            {
                if (item.Profile == t.Profile)
                {
                    t = item;
                }
            }

            var check = await dbf.deleteUser(t);
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Warning", "Please, check your internet settings!", "OK");
            }
            else if (check)
            {
                DependencyService.Get<ISaveAndLoad>().SaveText("login.txt", "");

                await Navigation.PushAsync(new MainCsPage());
            }
            else
            {
                await DisplayAlert("Warning","Something went wrong, please check back later!","OK");
            }
            
        }

        #endregion
        
    }
}
