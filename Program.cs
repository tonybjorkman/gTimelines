using System;
using System.Text.Json;
using google;
using CalendarQuickstart;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Google Timelines -> Calendar Loader");
            GeoFencingService.Test();
            
            string locations = System.IO.File.ReadAllText("2021_MAY.json");  
            var timelineContainer = JsonSerializer.Deserialize<TimelineContainer>(locations);
            
            var gc = new GCalendar("c_iksc4fa2mjhmautqv1k3lsb2ls@group.calendar.google.com");
            gc.Initialize();

            var tlp = new TimeLineParser(timelineContainer);
            ClassifiedEvent ce;
            do{
                ce = tlp.GetNextClassifiedEvent();
                if (ce is not null){
                    var nextEvent = new GCalEvent(ce);
                    gc.InsertEvent(nextEvent);
                    System.Console.WriteLine("- - - - - - - - - - - - - -");
                }
            } while(ce is not null);


            Console.WriteLine("End of program");
        }

        public void testJson(){
            string jsonstr = System.IO.File.ReadAllText("testdata.json");  
            var user = JsonSerializer.Deserialize<TestJson>(jsonstr);

            //json test parsing on test data. only debug/try stuff out.
            using (JsonDocument document = JsonDocument.Parse(jsonstr))
            {
                foreach (JsonElement element in document.RootElement.GetProperty("nums").EnumerateArray()){
                    int one = element.GetProperty("one").GetInt32();
                    int two = element.GetProperty("two").GetInt32();
                    System.Console.WriteLine($"one:{one},two:{two}");
                }
            }
        }
    }
}
