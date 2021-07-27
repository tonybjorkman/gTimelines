using System;
using System.Text.Json;
using google;
namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string jsonstr = System.IO.File.ReadAllText("testdata.json");  
            var user = JsonSerializer.Deserialize<TestJson>(jsonstr);

            string locations = System.IO.File.ReadAllText("2021_MAY.json");  
            var timelineContainer = JsonSerializer.Deserialize<TimelineContainer>(locations);

            //prints the entire log directly from the locsobject
            /*foreach(TimelineObject tlo in locs.timelineObjects){
                System.Console.WriteLine(tlo);
            }*/

            //uses a parser which has a memory to detect change of days
            //and missing data
            var tlp = new TimeLineParser(timelineContainer);
            while(tlp.hasNext){
                System.Console.WriteLine(tlp.GetNext());
                System.Console.WriteLine("- - - - - - - - - - - - - -");
            }

            //json test parsing on test data. only debug/try stuff out.
            using (JsonDocument document = JsonDocument.Parse(jsonstr))
            {
                foreach (JsonElement element in document.RootElement.GetProperty("nums").EnumerateArray()){
                    int one = element.GetProperty("one").GetInt32();
                    int two = element.GetProperty("two").GetInt32();
                    System.Console.WriteLine($"one:{one},two:{two}");
                }
            }

            Console.WriteLine("End step");
        }
    }
}
