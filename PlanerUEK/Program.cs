using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using HtmlAgilityPack;

namespace PlanerUEK
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/calendar-dotnet-quickstart.json
        private static string[] Scopes = { CalendarService.Scope.Calendar };
        private static string ApplicationName = "PlanerUEK";
        private enum Groups { IS1011 = 84721, IS1012 = 84731, IS1013 = 84741, IS1014 = 84751 }
        private static List<Event> Lectures = new List<Event>();

        static void Main(string[] args)
        {
            Greet();
            AddLecturesToList();
            AddEventsToCalendar(Lectures);
            Console.ReadKey();
        }

        private static void Greet() {
            Console.WriteLine("Welcome to Planer UEK! In order to save your classes in your Google Calendar you have to choose your student group.");
            Console.WriteLine("Type in corresponding number and hit Enter: ");
            Console.WriteLine("1 -> KrDZIs1011\n2 -> KrDZIs1012\n3 -> KrDZIs1013\n4 -> KrDZIs1014");
        }
        private static void ShowIncorrectInputMessage() {
            Console.Clear();
            Console.WriteLine("You typed invalid character, try again.");
            Console.WriteLine("1 -> KrDZIs1011\n2 -> KrDZIs1012\n3 -> KrDZIs1013\n4 -> KrDZIs1014");
        }
        private static void AddLecturesToList() {
            var nodes = GetHTMLTableRows();
            RemoveTableHeader(ref nodes);

            foreach (var node in nodes)
            {
                if (node.HasClass("czerwony"))          //This class is used to change text color of postponed lectures.
                    continue;
                if (node.SelectSingleNode("./td[4]").InnerText == "lektorat")//Do not add foreign language classes.
                    continue;
                var calendarEvent = SetupNewEvent(node);
                Lectures.Add(calendarEvent);
            }
        }
        private static void RemoveTableHeader(ref HtmlNodeCollection nodes) {
            nodes.RemoveAt(0);
        }
        private static void AddEventsToCalendar(List<Event> eventList) {
            var credential = GoogleSetup();
            var service = GetCalendarService(credential);

            Console.WriteLine(eventList.Count);
            foreach (var eventItem in eventList)
            {
                var request = service.Events.Insert(eventItem, "primary");
                var res = request.Execute();
                Console.WriteLine("Created new event in calendar:\n{0} {1} {2} {3}",eventItem.Start.DateTime, eventItem.Summary, eventItem.Description, eventItem.Location);
            }
        }
        private static string CreateTimeTableLink(int group) {
            return @"http://planzajec.uek.krakow.pl/index.php?typ=G&id=" + group + "&okres=1";
        }
        private static int GetStudentGroup() {
            bool isInputValid = false;
            int userInput;
            while (!isInputValid)
            {
                if (int.TryParse(Console.ReadLine(), out userInput))
                {
                    Console.Clear();
                    switch (userInput)
                    {
                        case 1:
                            return (int) Groups.IS1011;
                        case 2:
                            return (int) Groups.IS1012;
                        case 3:
                            return (int) Groups.IS1013;
                        case 4:
                            return (int) Groups.IS1014;
                        default:
                            ShowIncorrectInputMessage();
                            break;
                    }
                }
                else
                {
                    ShowIncorrectInputMessage();
                }
            }
            return 0;
        }
        private static Event SetupNewEvent(HtmlNode node)
        {
            Event newEvent = new Event();
            newEvent.Start = CreateEventStartDate(node);
            newEvent.End = CreateEventEndDate(node);
            newEvent.Summary = node.SelectSingleNode("./td[3]").InnerText;
            newEvent.Location = node.SelectSingleNode("./td[6]").InnerText;
            newEvent.Description = node.SelectSingleNode("./td[4]").InnerText +" " + node.SelectSingleNode("./td[5]").InnerText;
            newEvent.Reminders = GetEmptyReminders();

            return newEvent;
        }
        private static UserCredential GoogleSetup()
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, @".credentials\PlanerUEK.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
            return credential;
        }
        private static CalendarService GetCalendarService(UserCredential credential)
        {
            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }
        private static HtmlNodeCollection GetHTMLTableRows() {
            int studentGroup = GetStudentGroup();
            var timeTableLink = CreateTimeTableLink(studentGroup);

            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(timeTableLink);
            var nodes = htmlDoc.DocumentNode.SelectNodes("//body/table/tr");
            return nodes;
        }
        private static EventDateTime CreateEventStartDate(HtmlNode node) {
            string DateFormat = "yyyy-MM-dd HH:mm";
            var startEventDT = new EventDateTime();
            var providingDT = new DateTime();
            var stringDate = node.SelectSingleNode("./td[1]").InnerText + " " + node.SelectSingleNode("./td[2]").InnerText.Substring(3,5);
            providingDT = DateTime.ParseExact(stringDate, DateFormat, null);

            startEventDT.DateTime = providingDT;
            startEventDT.TimeZone = "Europe/Warsaw";
            return startEventDT;
        }
        private static EventDateTime CreateEventEndDate(HtmlNode node) {
            string DateFormat = "yyyy-MM-dd HH:mm";
            var endEventDT = new EventDateTime();
            var providingDT = new DateTime();
            var stringDate = node.SelectSingleNode("./td[1]").InnerText + " " + node.SelectSingleNode("./td[2]").InnerText.Substring(11, 5);
            providingDT = DateTime.ParseExact(stringDate, DateFormat, null);

            endEventDT.DateTime = providingDT;
            endEventDT.TimeZone = "Europe/Warsaw";
            return endEventDT;
        }
        private static Event.RemindersData GetEmptyReminders()
        {
            var reminders = new Event.RemindersData();
            reminders.UseDefault = false;
            return reminders;
        }
    }
}
