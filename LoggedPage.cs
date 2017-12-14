using Plugin.Connectivity;
using StudyGroup.Models;
using StudyGroup.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace StudyGroup.Views
{
    public class LoggedPage : ContentPage
    {
        #region publicAttributes

        SearchBar filterSearchBar;
        ObservableCollection<Events> eventsJson;
        ObservableCollection<Events> listofevents;
        ListView lw;
        ViewCell viewCell;
        string t;

        #endregion
        
        public LoggedPage(string userId)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                this.Icon = "events.png";
            }
            Title = "Groups";
            
            BackgroundImage = "background.png";
            BindingContext = this;
            t = userId;
            setEventJson(t);
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Hello!", "Please wait while loading!", "OK");
            });

        }

        private void setTheListView(string filter, ObservableCollection<Events> list)
        {
            ObservableCollection<Events> events = new ObservableCollection<Events>();
            
            foreach (var item in list)
            {
                if (item.eventName.ToLower().Contains(filter.ToLower()) )
                {
                    events.Add(item);
                }
            }
            lw.ItemsSource = events;
        }
        
        private async void setEventJson(string userId)
        {
            DatabaseFunctions dbf = new DatabaseFunctions();
            
            eventsJson = await dbf.getEvents();

            listofevents = eventsJson;

            filterSearchBar = new SearchBar()
            {
                SearchCommand = new Command(() => { setTheListView(filterSearchBar.Text, eventsJson); })
            };
            if (eventsJson.Count == 0)
            {
                await DisplayAlert("Info","There are not any group yet.","OK");
            }
            var personDataTemplate = new DataTemplate(() =>
            {
                var grid = new Grid();
                var nameLabel = new Label { TextColor = Color.White, FontAttributes = FontAttributes.Bold };
                var whereLabel = new Label() { TextColor = Color.White, };
                var whenLabel = new Label { TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Center };
                var uniLabel = new Label() { TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Start };
                var depLabel = new Label { TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Start };


                nameLabel.SetBinding(Label.TextProperty, "eventName");
                whereLabel.SetBinding(Label.TextProperty, "where");
                whenLabel.SetBinding(Label.TextProperty, "when");
                uniLabel.SetBinding(Label.TextProperty, "university");
                depLabel.SetBinding(Label.TextProperty, "department");

                grid.Children.Add(nameLabel, 0, 0);
                grid.Children.Add(whereLabel, 1, 0);
                grid.Children.Add(whenLabel, 2, 0);
                grid.Children.Add(uniLabel, 0, 1);
                grid.Children.Add(depLabel, 1, 1);

                viewCell = new ViewCell
                {
                    View = grid,
                };
                var joinAction = new MenuItem { Text = "Join" };
                joinAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
                joinAction.Clicked += async (sender, e) =>
                {
                    var mi = ((MenuItem)sender);
                    Events bindingEvent = (Events)mi.BindingContext;
                    Whoattended whoclass = new Whoattended();
                    whoclass.eventId = bindingEvent.eventId;
                    whoclass.profile = userId;

                    ObservableCollection<Whoattended> whoatt = await dbf.getWhoattended();

                    bool val = true;

                    foreach (var item in whoatt)
                    {
                        if (item.eventId == whoclass.eventId && item.profile == whoclass.profile)
                        {
                            val = false;
                            await DisplayAlert("Warning", "You have already attended to this event!", "OK");
                            break;
                        }
                    }
                    if (val == true)
                    {
                        var successStore = await dbf.putWhoattended(whoclass);

                        if (successStore)
                        {
                            await DisplayAlert("Successful", "You have joined to this event!", "OK");
                        }
                        else
                        {
                            await DisplayAlert("Warning", "Something went wrong, please check back later!", "OK");
                        }
                    }
                };

                var spectateAction = new MenuItem { Text = "Members", IsDestructive = false };
                spectateAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
                spectateAction.Clicked += async (sender, e) =>
                {
                    var mi = ((MenuItem)sender);
                    Events bindingEvent = (Events)mi.BindingContext;
                    ObservableCollection<Whoattended> whoatt = await dbf.getWhoattended();

                    await Navigation.PushAsync(new JoinedMembersPage(bindingEvent.eventId, whoatt));
                };

                viewCell.ContextActions.Add(joinAction);
                viewCell.ContextActions.Add(spectateAction);

                return viewCell;
            });
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Warning", "Please, check your internet settings!", "OK");
            }

            lw = new ListView()
            {
                IsPullToRefreshEnabled = true,
                ItemsSource = eventsJson,
                ItemTemplate = personDataTemplate,
                BackgroundColor = Color.Transparent
            };

            lw.Refreshing += Lw_Refreshing;

            if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Other)
            {
                Button refreshButton = new Button() {
                    HorizontalOptions = LayoutOptions.Center,
                    Text = "Refresh",
                    BackgroundColor = Color.FromHex("#4E8DB9"),
                    TextColor = Color.White
                };
                refreshButton.Clicked += RefreshButton_Clicked;

                Content = new ScrollView
                {
                    Content = new StackLayout()
                    {
                        Children = { filterSearchBar, refreshButton,lw }
                    }
                };
            }
            else
            {
                Content = new StackLayout()
                {
                    Children = { filterSearchBar,lw }
                };
            }
            
        }

        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            setEventJson(t);
        }
        
        private void Lw_Refreshing(object sender, EventArgs e)
        {
            setEventJson(t);
        }

    }
}