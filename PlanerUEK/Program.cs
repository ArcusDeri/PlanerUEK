using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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
        private static string[] _scopes = { CalendarService.Scope.Calendar };
        private static string _applicationName = "PlanerUEK";
        private enum _groups { IO2011 = 100041, SI2011 = 100031 }
        private static List<Event> _lectures = new List<Event>();

        static void Main(string[] args)
        {
            Greet();
            AddLecturesToList();
            AddEventsToCalendar(_lectures);
            Console.ReadKey();
        }

        private static void Greet() {
            Console.WriteLine("Welcome to Planer UEK! In order to save your classes in your Google Calendar you have to choose your student group.");
            Console.WriteLine("Type in corresponding number and hit Enter: ");
            WriteMenuOptions();
        }
        
        private static void WriteMenuOptions()
        {
            Console.WriteLine("1 -> KrDZIs2011IO");
            Console.WriteLine("2 -> KrDZIs2011SI");
            Console.WriteLine("3 -> Logout from Google");
        }

        private static void ShowIncorrectInputMessage() {
            Console.Clear();
            Console.WriteLine("You typed invalid character, try again.");
            WriteMenuOptions();
        }

        private static void AddLecturesToList() {
            var nodes = GetHTMLTableRows();
            RemoveTableHeader(ref nodes);

            foreach (var node in nodes)
            {
                if (node.HasClass("czerwony"))          //This class is used to change text color of postponed lectures.
                    continue;
                if(node.ChildNodes.Count != 13)
                    continue;
                if (node.SelectSingleNode("./td[3]").InnerText.Contains("obcy"))//Do not add foreign language classes.
                    continue;
                var calendarEvent = SetupNewEvent(node);
                _lectures.Add(calendarEvent);
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
                Console.WriteLine("Created new event in calendar:\n{0} {1} {2} {3}",eventItem.Start.DateTime, eventItem.Summary, eventItem.Description, eventItem.Location);
                var res = request.Execute();
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
                            return (int) _groups.IO2011;
                        case 2:
                            return (int) _groups.SI2011;
                        case 3:
                            return 0;
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
                    _scopes,
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
                ApplicationName = _applicationName,
            });
            return service;
        }

        private static HtmlNodeCollection GetHTMLTableRows() {
            int studentGroup = GetStudentGroup();
            if (studentGroup == 0){
                LogoutFromGoogle();
                Main(new string[0]);
            }
            var timeTableLink = CreateTimeTableLink(studentGroup);

            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(timeTableLink);
            var nodes = htmlDoc.DocumentNode.SelectNodes("//body/table/tr");
            return nodes;
        }

        private static void LogoutFromGoogle()
        {
            string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            if (Directory.Exists(Path.Combine(credPath, @".credentials\PlanerUEK.json")))
                Directory.Delete(Path.Combine(credPath, @".credentials\PlanerUEK.json"), true);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Logout successful.");
            Console.ForegroundColor = ConsoleColor.White;
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
