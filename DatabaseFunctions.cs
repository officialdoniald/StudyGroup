using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using StudyGroup.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudyGroup.Services
{
    public class DatabaseFunctions
    {
        public static MobileServiceClient MobileService =
          new MobileServiceClient(
          "https://studygroupxamarin.azurewebsites.net"
      );
        public DatabaseFunctions() { }

        public async Task<ObservableCollection<Events>> getEvents()
        {
            try
            {
                IEnumerable<Events> events = 
                    await MobileService.GetTable<Events>().ReadAsync();

                ObservableCollection<Events> returnableEvents =
                    new ObservableCollection<Events>(events);

                ObservableCollection<Events> returnableEventsReally = 
                    new ObservableCollection<Events>();

                DateTime dateNow = DateTime.Now;

                string dateNowString = dateNow.ToString("yyyy.MM.dd");
                
                foreach (var item in returnableEvents)
                {
                    string dateString = item.when.Split('_')[0];

                    if (DateTime.Parse(dateNowString).CompareTo(DateTime.Parse(dateString)) < 0)
                    {
                        returnableEventsReally.Add(item);
                    }
                }

                var orderedReturnValue = returnableEventsReally.OrderBy(i => i.eventName);

                return new ObservableCollection<Events>(orderedReturnValue);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return new ObservableCollection<Events>();
            }
        }

        public async Task<string> putEvent(Events events)
        {
            try
            {
                DateTime dateNow = DateTime.Now;

                string dateNowString = dateNow.ToString("yyyy.MM.dd");

                string dateString = events.when.Split('_')[0];

                if (DateTime.Parse(dateNowString).CompareTo(DateTime.Parse(dateString)) < 0)
                {
                    await MobileService.GetTable<Events>().InsertAsync(events);

                    return "successfull";

                }
                else return "dateproblem";

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return "server";
            }
        }

        public async Task<bool> deleteWhoattended(Whoattended whoattend)
        {
            try
            {
                await MobileService.GetTable<Whoattended>().DeleteAsync(whoattend);

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> putWhoattended(Whoattended whoattend)
        {
            try
            {
                await MobileService.GetTable<Whoattended>().InsertAsync(whoattend);

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<ObservableCollection<Whoattended>> getWhoattended()
        {
            try
            {
                IEnumerable<Whoattended> whoattends =
                    await MobileService.GetTable<Whoattended>().ReadAsync();

                ObservableCollection<Whoattended> returnableEvents =
                    new ObservableCollection<Whoattended>(whoattends);

                var orderedReturnValue = returnableEvents.OrderBy(i => i.eventId);

                return new ObservableCollection<Whoattended>(orderedReturnValue);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return new ObservableCollection<Whoattended>();
            }
        }

        public async Task<bool> deleteUser(Users user)
        {
            try
            {
                await MobileService.GetTable<Users>().DeleteAsync(user);

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<ObservableCollection<Users>> getUsers()
        {
            try
            {
                IEnumerable<Users> users =
                    await MobileService.GetTable<Users>().ReadAsync();

                ObservableCollection<Users> returnableusers =
                    new ObservableCollection<Users>(users);

                var orderedReturnValue = returnableusers.OrderBy(i => i.Name);

                return new ObservableCollection<Users>(orderedReturnValue);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return new ObservableCollection<Users>();
            }
        }

        public async Task<bool> putUser(Users users)
        {
            try
            {
                await MobileService.GetTable<Users>().InsertAsync(users);

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}
