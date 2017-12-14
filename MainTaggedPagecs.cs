using Xamarin.Forms;

namespace StudyGroup.Views
{
    public class MainTaggedPagecs : TabbedPage
    {
        public MainTaggedPagecs(string user)
        {
            this.Title = "StudyGroup";
            this.Children.Add(new LoggedPage(user));
            this.Children.Add(new EventCreatePage(user));
            this.Children.Add(new JoinedEventsPage(user));
            this.Children.Add(new ProfileUpdatePage(user));
        }
    }
}
