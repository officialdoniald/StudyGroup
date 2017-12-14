using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.Connectivity;
using StudyGroup.Models;
using StudyGroup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace StudyGroup.Views
{
    public class JoinedMembersPage : ContentPage
    {
        #region publicAttributes

        ListView lw;
        ViewCell viewCell;
        string eventId;
        ObservableCollection<Whoattended> wholist;

        #endregion

        public JoinedMembersPage(string eventid, ObservableCollection<Whoattended> wholist)
        {
            Title = "Joined members";
            BackgroundImage = "background.png";
            eventId = eventid;
            this.wholist = wholist;
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Info", "Please wait while loading!", "OK");
            });
            setCont();
        }

        private async void setCont()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Warning", "Please, check your internet settings!", "OK");
            }
            else
            {
                DatabaseFunctions dbf = new DatabaseFunctions();

                var userJson = await dbf.getUsers();

                ObservableCollection<Users> listofevents = userJson;

                ObservableCollection<Whoattended> whoevents = wholist;

                ObservableCollection<Users> reallyList = new ObservableCollection<Users>();

                List<string> list = new List<string>();

                foreach (var item in whoevents)
                {
                    if (eventId == item.eventId)
                    {
                        list.Add(item.profile);
                    }
                }

                foreach (var item in list)
                {
                    foreach (var item2 in listofevents)
                    {
                        if (item == item2.Profile)
                        {
                            reallyList.Add(item2);
                        }
                    }
                }

                var personDataTemplate = new DataTemplate(() =>
                {
                    var grid = new Grid();
                    var nameLabel = new Label
                    {
                        TextColor = Color.White,
                        FontAttributes = FontAttributes.Bold,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        BackgroundColor = Color.Transparent
                    };

                    var uniLabel = new Label()
                    {
                        TextColor = Color.White,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalOptions = LayoutOptions.Center,
                        BackgroundColor = Color.Transparent
                    };

                    var depLabel = new Label
                    {
                        TextColor = Color.White,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalOptions = LayoutOptions.Center,
                        BackgroundColor = Color.Transparent
                    };

                    var webImage = new CircleImage
                    {
                        BorderThickness = 0,
                        HeightRequest = 250,
                        WidthRequest = 250,
                        Aspect = Aspect.AspectFit,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    };

                    webImage.SetBinding(Image.SourceProperty, "ProfilePicture");
                    nameLabel.SetBinding(Label.TextProperty, "Name");
                    uniLabel.SetBinding(Label.TextProperty, "University");
                    depLabel.SetBinding(Label.TextProperty, "Department");

                    grid.Children.Add(webImage, 0, 0);
                    Grid.SetRowSpan(webImage, 2);
                    grid.Children.Add(nameLabel, 1, 0);
                    Grid.SetColumnSpan(nameLabel, 2);
                    grid.Children.Add(uniLabel, 1, 1);
                    grid.Children.Add(depLabel, 2, 1);

                    viewCell = new ViewCell
                    {
                        View = grid
                    };
                    var joinAction = new MenuItem { Text = "Facebook" };
                    joinAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
                    joinAction.Clicked += (sender, e) =>
                    {
                        var mi = ((MenuItem)sender);
                        Users bindingUsers = (Users)mi.BindingContext;
                        Device.OpenUri(new Uri("http://www.facebook.com/" + bindingUsers.Profile));
                    };
                    viewCell.ContextActions.Add(joinAction);

                    return viewCell;
                });

                lw = new ListView()
                {
                    IsPullToRefreshEnabled = true,
                    ItemsSource = reallyList,
                    ItemTemplate = personDataTemplate,
                    VerticalOptions = LayoutOptions.Center,
                    BackgroundColor = Color.Transparent
                };
                lw.Refreshing += Lw_Refreshing;
                Content = new ScrollView
                {
                    Content = new StackLayout()
                    {
                        Children = { lw }
                    }
                };
            }
        }
        
        private void Lw_Refreshing(object sender, EventArgs e)
        {
            setCont();
        }
    }
}
