using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalendarQuickstart
{
    class GCalEvent : Event
    {
        public DateTime date;
        string toString;
        public GCalEvent(ClassifiedEvent ce){
            EventDateTime start = new EventDateTime();
            start.DateTime = ce.start;
            EventDateTime end = new EventDateTime();
            end.DateTime = ce.end;

            this.Start = start;
            this.End = end;
            this.Summary = ce.name;
            this.Description = ce.description;
            this.toString = ce.ToString();
        }

        public override string ToString()
        {
            return toString;
        }

    }
    class GCalendar
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/calendar-dotnet-quickstart.json
        static string[] Scopes = { CalendarService.Scope.Calendar };
        static string ApplicationName = "Google Calendar API .NET Quickstart";

        DateTime datePrevInsert;

        public CalendarService service;

        string calendarId;

        public GCalendar(string calendarId){
            this.calendarId = calendarId;
        }

        public void InsertEvent(Event ev){
            if (((DateTime)ev.Start.DateTime).Date != datePrevInsert.Date){
                datePrevInsert = ((DateTime)ev.Start.DateTime);
                System.Console.WriteLine($"**New Day** {datePrevInsert.ToString("dddd, dd MMMM yyyy")}");
            }
            service.Events.Insert(ev, calendarId).Execute();
            System.Console.WriteLine($"Inserted into Calendar: '{ev}'");
        }

        public void Initialize()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Console.Write("checking connection..");
            try{
                Events events = request.Execute();
            Console.WriteLine("OK");
            } catch (Exception e) {
                Console.WriteLine("Failed with exception:"+e.ToString());
                throw;
            }
            

            /*create event
            var ev = new Event();
            EventDateTime start = new EventDateTime();
            start.DateTime = new DateTime(2021, 7, 29, 11, 0, 0);

            EventDateTime end = new EventDateTime();
            end.DateTime = new DateTime(2021, 7, 29, 11, 30, 0);

            ev.Start = start;
            ev.End = end;
            ev.Summary = "API Timelines New Event";
            ev.Description = "Place Description...";

            Console.WriteLine($"Event created: {ev.HtmlLink}\n");
            
            Console.Read();
            */
        }

        public void PrintColorIds(){
            Colors colors = service.Colors.Get().Execute();
            // Print available event colors.
            foreach (KeyValuePair<String, ColorDefinition> color in colors.Event__) {
                System.Console.WriteLine("ColorId : " + color.Key);
                System.Console.WriteLine("  Background: " + color.Value.Background);
                System.Console.WriteLine("  Foreground: " + color.Value.Foreground);
            }
        }
    }
}