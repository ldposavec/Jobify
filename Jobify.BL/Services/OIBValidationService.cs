using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading.Tasks;

namespace Jobify.BL.Services
{
    public class OIBValidationService
    {
        private readonly IWebDriver _driver;
        public OIBValidationService(IWebDriver driver)
        {
            _driver = driver;
        }
        public async Task<bool> ValidateOIBAsync(string oib)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _driver.Navigate().GoToUrl("https://sudreg.pravosudje.hr/registar/f?p=150:1");
                    WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                    var oibField = wait.Until(d => d.FindElement(By.Id("P1_OIB")));
                    oibField.SendKeys(oib);

                    var searchButton = _driver.FindElement(By.Id("B1173792588657644954"));
                    searchButton.Click();

                    return _driver.PageSource.Contains("Aktivno");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                    return false;
                }
                finally
                {
                    _driver.Quit();
                }
            });
        }
    }
}
