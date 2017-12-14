using Plugin.Connectivity;
using StudyGroup.Models;
using StudyGroup.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace StudyGroup.Views
{
    public class JoinedEventsPage : ContentPage
    {
        #region publicAttributes

        ObservableCollection<Events> eventsJson;
        ObservableCollection<Whoattended> whoattendedJson;
        ListView lw;
        Label checkLabel;
        ViewCell viewCell;
        string user;
        ObservableCollection<Events> listofevents;
        ObservableCollection<Whoattended> whoevents;
        string t;
        #endregion

        public JoinedEventsPage(string userId)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                this.Icon = "attend.png";
            }
            Title = "Attended";
            BackgroundImage = "background.png";
            user = t = userId;
        }

        private async void setEventJson(string userId)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Warning", "Please, check your internet settings!", "OK");
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Info", "Please wait while loading!", "OK");
                });
                DatabaseFunctions dbf = new DatabaseFunctions();

                eventsJson = await dbf.getEvents();

                listofevents = eventsJson;
                
                whoattendedJson = await dbf.getWhoattended();

                whoevents = whoattendedJson;

                if (listEvents(listofevents, whoevents).Count == 0)
                {
                    checkLabel = new Label()
                    {
                        Text = "Youd didn't attend to any groups.",
                        FontSize = 15,
                        TextColor = Color.White
                    };
                }
                else
                {
                    checkLabel = new Label()
                    {
                        Text = "",
                        FontSize = 15,
                        TextColor = Color.White
                    };
                }

                var personDataTemplate = new DataTemplate(() =>
                {
                    var grid = new Grid();
                    var nameLabel = new Label { TextColor = Color.White, FontAttributes = FontAttributes.Bold };
                    var whereLabel = new Label() { TextColor = Color.White, };
                    var whenLabel = new Label { TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Start };
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

                    var spectateAction = new MenuItem { Text = "Members", IsDestructive = false };
                    spectateAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
                    spectateAction.Clicked += async (sender, e) =>
                    {
                        var mi = ((MenuItem)sender);
                        Events bindingEvent = (Events)mi.BindingContext;
                        await Navigation.PushAsync(new JoinedMembersPage(bindingEvent.eventId, whoevents));
                    };

                    var deleteAction = new MenuItem { Text = "Delete", IsDestructive = true };
                    deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
                    deleteAction.Clicked += async (sender, e) =>
                    {
                        var mi = ((MenuItem)sender);
                        Events bindingEvent = (Events)mi.BindingContext;
                        Whoattended deleted = new Whoattended();
                        deleted.eventId = bindingEvent.eventId;
                        deleted.profile = user;

                        whoattendedJson = await dbf.getWhoattended();

                        foreach (var item in whoattendedJson)
                        {
                            if (item.eventId == bindingEvent.eventId && item.profile == user)
                            {
                                deleted.id = item.id;
                                break;
                            }
                        }

                        var check = await dbf.deleteWhoattended(deleted);
                        if (check)
                        {
                            await DisplayAlert("Successful", "You have unsubscribe from the event!", "OK");
                        }
                        else
                        {
                            await DisplayAlert("Warning", "Something went wrong, please check back later!", "OK");
                        }
                        whoattendedJson = await dbf.getWhoattended();
                        ObservableCollection<Whoattended> whoevents2 = whoattendedJson;
                        lw.ItemsSource = listEvents(listofevents, whoevents2);
                    };

                    viewCell.ContextActions.Add(deleteAction);
                    viewCell.ContextActions.Add(spectateAction);

                    return viewCell;
                });

                lw = new ListView()
                {
                    IsPullToRefreshEnabled = true,
                    ItemsSource = listEvents(listofevents, whoevents),
                    ItemTemplate = personDataTemplate,
                    BackgroundColor = Color.Transparent
                };

                Content = new ScrollView
                {
                    Content = new StackLayout()
                    {
                        Children = { checkLabel, lw }
                    }
                };

                lw.Refreshing += Lw_Refreshing;
            }
        }

        private void Lw_Refreshing(object sender, EventArgs e)
        {
            setEventJson(t);

        }

        private ObservableCollection<Events> listEvents(ObservableCollection<Events> linked, ObservableCollection<Whoattended> linkwho)
        {
            ObservableCollection<Events> events = new ObservableCollection<Events>();

            foreach (var item in linkwho)
            {
                if (user == item.profile)
                {
                    foreach (var item2 in linked)
                    {
                        if (item2.eventId == item.eventId)
                        {
                            events.Add(item2);
                        }
                    }
                }
            }

            return events;
        }

        protected override void OnAppearing()
        {
            setEventJson(t);
        }
    }
}
