using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using NUnit.Framework;

namespace ShoeStoreTest.Pages
{
    public class HomePage
    {
        public HomePage(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.Id, Using = "search_button")] public IWebElement btnSearch;

        [FindsBy(How = How.Id, Using = "remind_email_input")] public IWebElement tbEmailAddress;

        [FindsBy(How = How.XPath, Using = "//*[@id='remind_email_form']/div/input[2]")] public IWebElement btnSubmit;

        // *****************************
        // I am only searching for elements that 
        // I will use in this exercise
        // *****************************

        public bool VerifyHomePageIsDisplayed()
        {
            Assert.IsTrue(btnSearch.Displayed, "The Brand Search button is not visible!");
            return true;
        }

        public void ClickSubmitButton()
        {
            btnSubmit.Click();
        }

        public void EnterEmailAddress(string emailAddress)
        {
            tbEmailAddress.SendKeys(emailAddress);
        }        
    }
}
