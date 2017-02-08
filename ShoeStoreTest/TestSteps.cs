using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using ShoeStoreTest.Pages;

namespace ShoeStoreTest
{
    [TestFixture(typeof(ChromeDriver))]
    [TestFixture(typeof(FirefoxDriver))]
    [TestFixture(typeof(InternetExplorerDriver))]
    public class TestSteps<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        
        private const string HomePageUrl = "http://shoestore-manheim.rhcloud.com/";
        
        [SetUp]
        protected void SetUp()
        {
            // assign driver for test
            _driver = new TWebDriver();

            // Launch home page
            _driver.Navigate().GoToUrl(HomePageUrl);

            // Maximize the browser window to avoid any screen size, element misplaced issues
            _driver.Manage().Window.Maximize();
        }

        [Test]
        public void VerifyMonthPageContents()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Verify that we are actually on the home page
            HomePage homePage = new HomePage(_driver);
            Assert.IsTrue(homePage.VerifyHomePageIsDisplayed());

            // Create a list of the months links
            List<IWebElement> monthList = _driver.FindElements(By.XPath("//*[@id='header_nav']/nav/ul/li/a")).ToList();
           
            // Create a string list to avoid stale element exception
            List<string> monthLinks = new List<string>();

            foreach (var month in monthList)
            {
                monthLinks.Add(month.Text);
            }

            // Counter
            int counter = 0;

            // Create a button we can click
            IWebElement monthToClick;

            // Iterate through the months
            while (counter < monthList.Count)
            {
                // Find and click on desired month
                monthToClick = _driver.FindElement(By.LinkText(monthLinks[counter]));
                monthToClick.Click();

                // Create a new instance of the Month Page
                MonthPage monthPage = new MonthPage(_driver);

                // The below 'waits' allow the test to execute successfully
                // on Firefox as well as the other browsers.  I can also avoid Thread.Sleeps this way
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[4]/h2")));
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("shoe_list")));

                // ***********************************************************************
                // ***AC1: Month should display a small Blurb of each shoe
                // ***********************************************************************
                // ***********************************************************************
                // ***What is considered a 'small Blurb'?
                // ***For this test I will use the description as the blurb
                // ***********************************************************************

                // Verify that the month page contains shoes
                Assert.IsTrue(monthPage.lstShoeContainers.Count > 0, $"The {monthLinks[counter]} page does not contain any shoes!");

                // Verify that the month page contains a small blurb for each shoe
                Assert.IsTrue(monthPage.VerifyMonthPageContainsShoesWithBlurbs(), $"The {monthLinks[counter]} page does not contain a blurb for each shoe!");

                // Verify that each shoe has an image associated with it
                Assert.IsTrue(monthPage.VerifyEachShoeContainsAnImage(), $"At least one shoe on the {monthLinks[counter]} page does not have an image showing!");
                
                // Verify that each shoe has a price associated with it
                Assert.IsTrue(monthPage.VerifyEachShoeHasAPrice(), $"At least one price on the {monthLinks[counter]} page field is blank!");

                // Increase the counter
                counter++;
            }

        }

        [Test]
        [TestCase("jt.bhatt@email.com")]      
        public void VerifyEmailAddressCanBeSubmitted(string emailaddress)
        {
            // Verify that we are actually on the home page
            HomePage homePage = new HomePage(_driver);
            Assert.IsTrue(homePage.VerifyHomePageIsDisplayed());

            // Enter the email address
            homePage.EnterEmailAddress(emailaddress);

            // Click Submit
            homePage.ClickSubmitButton();

            // Needed to pass on FireFox
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='flash']/div")));            

            // Verify Thank You Message
            string tyMessage = _driver.FindElement(By.XPath("//*[@id='flash']/div")).Text;
            Assert.AreEqual($"Thanks! We will notify you of our new shoes at this email: {emailaddress}", tyMessage);            
        }

        [TearDown]
        protected void TearDown()
        {
            _driver.Quit();            
        }

    }
}
