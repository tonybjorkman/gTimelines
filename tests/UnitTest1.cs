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
            //System.Threading.Thread.Sleep(10000);

            //var result2 = Browser.WaitForElement(By.XPath("//div[contains(text),'Foobar')]"),5);
            Assert.True(result);
            //Assert.False(result2);
        }
    }
}
