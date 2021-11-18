using Xunit;
using test1;
using OpenQA.Selenium;

namespace tests
{
    public class TestGoogleTakeout
    {
        [Fact]
        public void TestWebDelays()
        {
            var b = Browser.GetChrome();
            b.Navigate().GoToUrl("file:///home/tony/code/test1/websample/delayElement/delayElement.html");
            var result = Browser.WaitForElement(By.XPath("//div[contains(text(),'Finished')]"),20);

            Assert.True(result);
              
            result = Browser.WaitForElement(By.XPath("//div[contains(text(),'Foo-bar')]"),3);
            Assert.False(result);
            b.Quit();
        }

        [Fact]
        public void TestUrlRedir(){
            var b = Browser.GetChrome();
            b.Navigate().GoToUrl("file:///home/tony/code/test1/websample/delayElement/delayRedir.html");
            var result = Browser.WaitForUrl("file:///home/tony/code/test1/websample/delayElement/redirPage.html",20);
            Assert.True(b.Url=="file:///home/tony/code/test1/websample/delayElement/redirPage.html");
            Assert.True(result);
            
            b.Navigate().GoToUrl("file:///home/tony/code/test1/websample/delayElement/delayElement.html");
            result = Browser.WaitForUrl("file:///home/tony/code/test1/websample/delayElement/redirPage.html",4);
            Assert.False(result);
            b.Quit();
            
        }
    }
}
