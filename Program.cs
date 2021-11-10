using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using google;
using CalendarQuickstart;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Linq;
using OpenQA.Selenium.Support.UI;

namespace test1
{

    public class GoogleTakeout{
        private readonly String downloadUrl = "https://takeout.google.com/takeout/downloads";
        private readonly String exportUrl = "https://takeout.google.com/settings/takeout";


        public GoogleTakeout(){
        }

        public void DownloadGeoHistory(){
            StartGeoHistoryExport(this.exportUrl);
            var ready = WaitForUrl(this.downloadUrl);
            if (ready){
                GetTakeoutDownloadLink(this.downloadUrl);
                System.Console.WriteLine("Takeout downloaded");
            }
        }

        private void StartGeoHistoryExport(string url)
        {
            //Navigate to DotNet website
            try{
                Browser.GetChrome().Navigate().GoToUrl(url);
                //Click the Get Started button
                Browser.GetChrome().FindElement(By.XPath("//span[contains(text(),'Avmarkera alla')]")).Click();
                Thread.Sleep(3000);
                Browser.GetChrome().FindElement(By.XPath("//input[@name='Platshistorik']")).Click();
                Thread.Sleep(3000);
                Browser.GetChrome().FindElement(By.XPath("//span[contains(text(),'Nästa steg')]")).Click();
                Thread.Sleep(3000);
                Browser.GetChrome().FindElement(By.XPath("//span[contains(text(),'Skapa export')]")).Click();
            } catch (NoSuchElementException e){
                new Exception("Could not find element. Page not loaded?",e);       
            }
        }
        private String GetTakeoutDownloadLink(String downloadPageUrl){
            if(Browser.GetChrome().Url != downloadPageUrl){
                throw new Exception("Unexpected Url. First navigate to the correct url.");
            }
            var element = Browser.GetChrome().FindElement(By.XPath("(//a[@aria-label='Hämta'])[1]"));
            string downloadLink = element.GetAttribute("href");
            System.Console.WriteLine("Found takeout download link: "+downloadLink);
            return downloadLink;
        }

        private Boolean WaitForUrl(String url){
                WebDriverWait w = new WebDriverWait(Browser.GetChrome(),new TimeSpan(0, 10, 30));
                var success = w.Until(condition =>
                {
                    try
                    {
                        return Browser.GetChrome().Url.Contains(url);
                    }
                    catch (StaleElementReferenceException)
                    {
                        return false;
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                });
                System.Threading.Thread.Sleep(3000);
                System.Console.WriteLine("Wait condition finished");
            return success;
        } 
    }
    public class Browser{
        private static ChromeDriver chromeDriver;
        private ChromeDriver driver;

        public Browser(){
           this.driver = new ChromeDriver(".",this.CreateChromeOptions()); 
        }

        public static ChromeDriver GetChrome(){
            return chromeDriver?? (chromeDriver = new Browser().driver);
        }
        private ChromeOptions CreateChromeOptions(){
            var options = new ChromeOptions();
            options.AddArgument(@"user-data-dir=~/.config/google-chrome/");
            options.AddArgument("--allow-file-access-from-files");
            return options;
        }

        ~Browser(){
            this.driver.Quit();
        }
    }
    public class Program
    {
        Browser browser;

        static void Main(string[] args)
        {
            var takeout = new GoogleTakeout();
            takeout.DownloadGeoHistory();
            TestSecond("https://takeout.google.com/takeout/downloads");

            TestGetStarted();
             
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



        public static void TestSecond(string url="file:///home/tony/code/test1/websample/step4final/Hantera%20exporter.html") {

            var options = new ChromeOptions();
            //options.AddArgument(@"user-data-dir=~/.config/google-chrome/");
            options.AddArgument("--allow-file-access-from-files"); 


            using (var driver = new ChromeDriver(".", options))
            {
                driver.Navigate().GoToUrl(url);
                var element = WaitForDownloadLink(driver);
                //var element = driver.FindElement(By.XPath("(//a[@aria-label='Hämta'])[1]"));

                if(element is not null){
                string downloadLink = element.GetAttribute("href");
                System.Console.WriteLine("Found a match: "+downloadLink);
                driver.Navigate().GoToUrl(downloadLink);
                // Get Started section is a multi-step wizard
                // The following sections will find the visible next step button until there's no next step button left
                } else {
                    System.Console.WriteLine("Element was null");
                }
            }
            
        }

    public static int Until<T>(T t1, Func<T,int,int> fun){
        var kalle=fun(t1,6);
        return kalle;
    }

public static void olle(){
    Until<int>(1,(a,b) =>  a+1);
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



