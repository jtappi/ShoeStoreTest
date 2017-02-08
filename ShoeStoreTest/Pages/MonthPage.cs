using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using NUnit.Framework;

namespace ShoeStoreTest.Pages
{
    public class MonthPage
    {
        public MonthPage(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//*[@id='shoe_list']/li")]
        public IList<IWebElement> lstShoeContainers;

        [FindsBy(How = How.ClassName, Using = "shoe_description")]
        public IList<IWebElement> lstShoeBlurbs;

        [FindsBy(How = How.XPath, Using = "//*[@id='shoe_list']/li/div/table/tbody/tr/td/img")]
        public IList<IWebElement> lstShoeImages;

        [FindsBy(How = How.ClassName, Using = "shoe_price")]
        public IList<IWebElement> lstShoePrice;
        
        public bool VerifyMonthPageContainsShoesWithBlurbs()
        {
            try
            {                
                // Verify that the number of Shoes matches the number of Descriptions
                if (lstShoeContainers.Count == lstShoeBlurbs.Count && lstShoeContainers.Count > 0)
                {
                    foreach (var item in lstShoeBlurbs)
                    {
                        Assert.IsNotEmpty(item.Text, "A blurb does not contain any text!");
                    }
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool VerifyEachShoeContainsAnImage()
        {
            try
            {
                if (lstShoeContainers.Count == lstShoeImages.Count)
                {
                    foreach (var item in lstShoeImages)
                    {
                        Assert.IsNotEmpty(item.GetAttribute("src"),
                            "NO IMAGE: An img tag exists but the src tag is empty!");
                    }
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool VerifyEachShoeHasAPrice()
        {
            try
            {
                // This does not fail on the December page because the Acceptance Criteria
                // does not care if shoes do not exist.  t only cares for a price when 
                // shoes are present.
                if (lstShoeContainers.Count == lstShoePrice.Count)
                {
                    foreach (var price in lstShoePrice)
                    {
                        Assert.IsNotEmpty(price.Text, "The price tag is blank!  But they are not FREEE!!!!");
                    }
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
