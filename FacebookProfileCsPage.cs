using System;
using FacebookLogin.Models;
using FacebookLogin.ViewModels;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;
using StudyGroup.Models;
using StudyGroup.Views;
using StudyGroup.FileStoring;
using StudyGroup.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Plugin.Connectivity;
using System.Collections.Generic;

namespace FacebookLogin.Views
{
    public class FacebookProfileCsPage : ContentPage
    {
        ActivityIndicator indicator;

        public FacebookProfileCsPage()
        {
            Title = "Education";

            BackgroundImage = "background.png";
            BindingContext = new FacebookViewModel();

            BackgroundColor = Color.White;

            var apiRequest =
                "https://www.facebook.com/dialog/oauth?client_id="
                + ClientId
                + "&display=popup&response_type=token&redirect_uri=http://www.facebook.com/connect/login_success.html";

            var webView = new WebView
            {
                Source = apiRequest,
                HeightRequest = 1
            };

            webView.Navigated += WebViewOnNavigated;

            Content = webView;
        }

        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {

            var accessToken = ExtractAccessTokenFromUrl(e.Url);

            if (accessToken != "")
            {
                var vm = BindingContext as FacebookViewModel;

                //await vm.SetFacebookUserProfileAsync(accessToken);

                SetPageContent(/*vm.FacebookProfile*/);
            }
        }

        public static int GetAge(DateTime reference, DateTime birthday)
        {
            int age = reference.Year - birthday.Year;
            if (reference < birthday.AddYears(age)) age--;

            return age;
        }

        private void SetPageContent(/*FacebookProfile facebookProfile*/)
        {
            indicator = new ActivityIndicator
            {
                IsRunning = false,
                Color = Color.FromHex("#4E8DB9"),
                HeightRequest = 50,
                WidthRequest = 50,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            publicUser = new Users()
            {
                //Name = facebookProfile.FirstName + " " + facebookProfile.LastName,
                //Age = "0",
                //ProfilePicture = facebookProfile.Picture.Data.Url,
                //Profile = facebookProfile.Id
                Name = "asd",
                Age = "0",
                Profile = "asdasd",
                ProfilePicture = "igi"
            };

            List<string> unis = new List<string>();

            unis.Add("ANNYE");
            unis.Add("ATE");
            unis.Add("ATF");
            unis.Add("AVKF");
            unis.Add("BCE");
            unis.Add("BGE");
            unis.Add("BHF");
            unis.Add("BKTF");
            unis.Add("BME");
            unis.Add("BTA");
            unis.Add("DE");
            unis.Add("DRHE");
            unis.Add("DUE");
            unis.Add("EDUTUS");
            unis.Add("EGHF");
            unis.Add("EHE");
            unis.Add("EJF");
            unis.Add("EKE");
            unis.Add("ELTE");
            unis.Add("ESZHF");
            unis.Add("GDF");
            unis.Add("GFF");
            unis.Add("GTF");
            unis.Add("GYHF");
            unis.Add("IBS");
            unis.Add("KE");
            unis.Add("KEE");
            unis.Add("KJF");
            unis.Add("KRE");
            unis.Add("LFZE");
            unis.Add("ME");
            unis.Add("METU");
            unis.Add("MKE");
            unis.Add("MOME");
            unis.Add("MTF");
            unis.Add("NKE");
            unis.Add("NYE");
            unis.Add("NYME");
            unis.Add("OE");
            unis.Add("OR-ZSE");
            unis.Add("PAE");
            unis.Add("PAF");
            unis.Add("PE");
            unis.Add("PHF");
            unis.Add("PPKE");
            unis.Add("PRTA");
            unis.Add("PTE");
            unis.Add("PTF");
            unis.Add("SE");
            unis.Add("SRTA");
            unis.Add("SSTF");
            unis.Add("SSZHF");
            unis.Add("SZAGKHF");
            unis.Add("SZE");
            unis.Add("SZFE");
            unis.Add("SZIE");
            unis.Add("SZPA");
            unis.Add("SZTE");
            unis.Add("TE");
            unis.Add("TKBF");
            unis.Add("TPF");
            unis.Add("VHF");
            unis.Add("WJLF");
            unis.Add("WSUF");
            unis.Add("ZSKE");

            Picker uniPicker = new Picker()
            {
                TextColor = Color.White,
                Title = "University",
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Transparent
            };

            foreach (var item in unis)
            {
                uniPicker.Items.Add(item);
            }

            Button storeButton = new Button() {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Create account",
                BackgroundColor = Color.FromHex("#4E8DB9"),
                TextColor = Color.White
            };

            Entry universityEntry = new Entry() {
                FontSize = 25,
                TextColor = Color.White,
                Placeholder = "Other School",
                BackgroundColor = Color.Transparent
            };

            Entry departmentEntry = new Entry() {
                FontSize = 25,
                TextColor = Color.White,
                Placeholder = "Department",
                BackgroundColor = Color.Transparent
            };

            universityEntry.TextChanged += UniversityEntry_TextChanged;
            departmentEntry.TextChanged += DepartmentEntry_TextChanged;
            storeButton.Clicked += StoreButton_Clicked;
            uniPicker.SelectedIndexChanged += (sender, args) =>
            {
                publicUser.University = uniPicker.Items[uniPicker.SelectedIndex];
            };
            Content = new ScrollView
            {
                Content =
                new StackLayout() {
                    Spacing = 30,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Margin = 5,
                    Children = {
                        indicator,
                        uniPicker,
                        universityEntry,
                        departmentEntry,
                        storeButton
                    }
                }
            }
            ;
        }

        #region elementChangesAndSelected

        private void DepartmentEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            publicUser.Department =  e.NewTextValue;
        }

        private void UniversityEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            publicUser.University = e.NewTextValue;
        }

        #endregion

        #region click

        private void StoreButton_Clicked(object sender, EventArgs e)
        {
            StoreTheUniversity(university, department);
        }

        #endregion
        
        private string ExtractAccessTokenFromUrl(string url)
        {
            if (url.Contains("access_token") && url.Contains("&expires_in="))
            {
                var at = url.Replace("https://www.facebook.com/connect/login_success.html#access_token=", "");

                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                {
                    at = url.Replace("http://www.facebook.com/connect/login_success.html#access_token=", "");
                }

                var accessToken = at.Remove(at.IndexOf("&expires_in="));

                return accessToken;
            }

            return string.Empty;
        }

        public async void StoreTheUniversity(string uni, string dep)
        {
            indicator.IsRunning = true;

            DatabaseFunctions dbf = new DatabaseFunctions();

            if (publicUser.University == null || publicUser.Department == null)
            {
                await DisplayAlert("Warning", "You have to fill in all of the fields!", "OK");
            }
            else
            {

                var userJson = await dbf.getUsers();
                ObservableCollection<Users> listofusers = userJson;

                bool haveThisUser = false;

                foreach (var item in listofusers)
                {
                    if (item.Profile == publicUser.Profile)
                    {
                        try
                        {
                            DependencyService.Get<ISaveAndLoad>().SaveText("login.txt", publicUser.Profile);
                        }
                        catch (Exception e) { Debug.WriteLine(e.Message); }
                        haveThisUser = true;
                        break;
                    }
                }
                if (!haveThisUser)
                {
                    if (!CrossConnectivity.Current.IsConnected)
                    {
                        await DisplayAlert("Warning", "Please, check your internet settings!", "OK");
                    }
                    else if (await dbf.putUser(publicUser))
                    {
                        await DisplayAlert("Successful", "Youd have created your account!", "OK");
                        try
                        {
                            DependencyService.Get<ISaveAndLoad>().SaveText("login.txt", publicUser.Profile);
                        }
                        catch (Exception e) { Debug.WriteLine(e.Message); }
                    }
                    else
                    {
                        try
                        {
                            DependencyService.Get<ISaveAndLoad>().SaveText("login.txt", "");
                        }
                        catch (Exception e) { Debug.WriteLine(e.Message); }
                        await DisplayAlert("Warning", "Something went wrong, please check back later!", "OK");
                    }
                    await Navigation.PushAsync(new MainTaggedPagecs(publicUser.Profile));
                    Navigation.RemovePage(this);
                }
                else
                {
                    await Navigation.PushAsync(new MainTaggedPagecs(publicUser.Profile));
                    Navigation.RemovePage(this);
                }

            }
        }
        
        #region publicAttributes

        private string ClientId = "1630021447293178";
        public Users publicUser { get; set; }
        public string university { get; set; }
        public string department { get; set; }

        #endregion
    }
}