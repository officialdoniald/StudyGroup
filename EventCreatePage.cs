using Plugin.Connectivity;
using StudyGroup.FileStoring;
using StudyGroup.Models;
using StudyGroup.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace StudyGroup.Views
{
    public class EventCreatePage : ContentPage
    {
        public EventCreatePage(string useId)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                this.Icon = "create.png";
            }
            Title = "Create";
            BackgroundImage = "background.png";
        }

        #region elemntChangedAndSelected

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            date = e.NewDate;
        }

        private void EventWhereEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            events.where = e.NewTextValue;
        }

        private void EventDepEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            events.department = e.NewTextValue;
        }

        private void EventUniEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            events.university = e.NewTextValue;
        }

        private void EventNameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            events.eventName = e.NewTextValue;
        }

        #endregion

        #region clicks

        private async void CreateeventButton_Clicked(object sender, EventArgs e)
        {
            DatabaseFunctions dbf = new DatabaseFunctions();

            indicator.IsRunning = true;

            whoattended = new Whoattended();

            string fnev = "";

            try { fnev = DependencyService.Get<ISaveAndLoad>().LoadText("login.txt");}catch (Exception) { }

            whoattended.profile = fnev;

            events.when = date.ToString("yyyy.MM.dd") + " " + hour;
            events.eventId = events.when + "_" + whoattended.profile + "_" + events.eventName;

            whoattended.eventId = events.eventId;
            
            if (!CrossConnectivity.Current.IsConnected)
            {
                indicator.IsRunning = false;
                await DisplayAlert("Warning", "Please, check your internet settings!", "OK");
            }
            else if (events.where == null || hour == null || events.department == null || events.eventName == null
                || events.university == null)
            {
                indicator.IsRunning = false;
                await DisplayAlert("Warning", "You have to fill in all of the fields!", "OK");
            }
            else
            {
                var successStoreEvent = await dbf.putEvent(events);

                var successStoreWhoattended = await dbf.putWhoattended(whoattended);

                if (successStoreEvent == "successfull" && successStoreWhoattended)
                {
                    await DisplayAlert("Successful", "You have created the group!", "OK");

                    indicator.IsRunning = false;
                }
                else if (successStoreEvent == "dateproblem")
                {
                    indicator.IsRunning = false;
                    await DisplayAlert("Warning", "You have to change the date!", "OK");
                }
                else
                {
                    indicator.IsRunning = false;
                    await DisplayAlert("Warning", "Something went wrong, please check back later!", "OK");
                }
            }
        }

        #endregion
        
        #region overrideFunctions

        protected override void OnAppearing()
        {
            events = new Events();

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
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.Transparent
            };

            foreach (var item in unis)
            {
                uniPicker.Items.Add(item);
            }

            createeventButton = new Button()
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Create group",
                BackgroundColor = Color.FromHex("#4E8DB9"),
                TextColor = Color.White
            };
            indicator = new ActivityIndicator
            {
                IsRunning = false,
                Color = Color.FromHex("#4E8DB9"),
                HeightRequest = 30,
                WidthRequest = 30,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            Entry eventNameEntry = new Entry()
            {
                Placeholder = "Group name",
                FontSize = 25,
                TextColor = Color.White,
                BackgroundColor = Color.Transparent
            };
            Entry eventUniEntry = new Entry()
            {
                Placeholder = "Other School",
                TextColor = Color.White,
                FontSize = 25,
                BackgroundColor = Color.Transparent
            };

            Entry eventDepEntry = new Entry()
            {
                Placeholder = "Department",
                TextColor = Color.White,
                FontSize = 25,
                BackgroundColor = Color.Transparent
            };

            Entry eventWhereEntry = new Entry()
            {
                Placeholder = "Where",
                TextColor = Color.White,
                FontSize = 25,
                BackgroundColor = Color.Transparent
            };

            DatePicker datePicker = new DatePicker()
            {
                TextColor = Color.White,
                BackgroundColor = Color.Transparent
            };

            Picker hourPicker = new Picker()
            {
                TextColor = Color.White,
                Title = "Hour",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.Transparent
            };

            date = DateTime.Now;

            List<string> hours = new List<string>();

            hours.Add("0:00");
            hours.Add("0:30");
            hours.Add("1:00");
            hours.Add("1:30");
            hours.Add("2:00");
            hours.Add("2:30");
            hours.Add("3:00");
            hours.Add("3:30");
            hours.Add("4:00");
            hours.Add("4:30");
            hours.Add("5:00");
            hours.Add("5:30");
            hours.Add("6:00");
            hours.Add("6:30");
            hours.Add("7:00");
            hours.Add("7:30");
            hours.Add("8:00");
            hours.Add("8:30");
            hours.Add("9:00");
            hours.Add("9:30");
            hours.Add("10:00");
            hours.Add("10:30");
            hours.Add("11:00");
            hours.Add("11:30");
            hours.Add("12:00");
            hours.Add("12:30");
            hours.Add("13:00");
            hours.Add("13:30");
            hours.Add("14:00");
            hours.Add("14:30");
            hours.Add("15:00");
            hours.Add("15:30");
            hours.Add("16:00");
            hours.Add("16:30");
            hours.Add("17:00");
            hours.Add("17:30");
            hours.Add("18:00");
            hours.Add("18:30");
            hours.Add("19:00");
            hours.Add("19:30");
            hours.Add("20:00");
            hours.Add("20:30");
            hours.Add("21:00");
            hours.Add("21:30");
            hours.Add("22:00");
            hours.Add("22:30");
            hours.Add("23:00");
            hours.Add("23:30");

            foreach (var item in hours)
            {
                hourPicker.Items.Add(item);
            }

            createeventButton.Clicked += CreateeventButton_Clicked;
            datePicker.DateSelected += DatePicker_DateSelected;
            eventNameEntry.TextChanged += EventNameEntry_TextChanged;
            eventUniEntry.TextChanged += EventUniEntry_TextChanged;
            eventDepEntry.TextChanged += EventDepEntry_TextChanged;
            eventWhereEntry.TextChanged += EventWhereEntry_TextChanged;

            hourPicker.SelectedIndexChanged += (sender, args) =>
            {
                hour = hourPicker.Items[hourPicker.SelectedIndex];
            };

            uniPicker.SelectedIndexChanged += (sender, args) =>
            {
                events.university = uniPicker.Items[uniPicker.SelectedIndex];

            };

            Content = new ScrollView()
            {
                Content = new StackLayout()
                {
                    Spacing = 30,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children = {
                    eventNameEntry,
                        uniPicker,
                    eventUniEntry,
                    eventDepEntry,
                    eventWhereEntry,
                    datePicker,
                    hourPicker,indicator,
                    createeventButton
                    }
                }
            };
        }

        #endregion

        #region publicAttributes

        public Events events { get; set; }

        public Whoattended whoattended { get; set; }

        public DateTime date { get; set; }

        public string hour { get; set; }

        Button createeventButton;

        ActivityIndicator indicator;
        
        #endregion

    }
}
