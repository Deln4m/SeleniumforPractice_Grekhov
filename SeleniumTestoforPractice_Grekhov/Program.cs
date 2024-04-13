using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTestoforPractice_Grekhov;

public class Seleniumtestforprac

{
    [Test]
    public void Authorization()
    {
        var options = new ChromeOptions(); 
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
        
        // -зайти в хром 
        var driver = new ChromeDriver();
        
        // - url стафа https://staff-testing.testkontur.ru/
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/");
        Thread.Sleep(5000);
        
        //login+password
        var Login = driver.FindElement(By.Id("Username"));
        Login.SendKeys("Delnam@outlook.com");
        var Password = driver.FindElement(By.Name("Password"));
        Password.SendKeys("Suabru22b22-");

        var Submit = driver.FindElement(By.Name("button"));
        Submit.Click();
        Thread.Sleep(3000);
        
        //проверка на нужную страницу
        var currenturl = driver.Url;
        Assert.That(currenturl == "https://staff-testing.testkontur.ru/news");
        
        //kill process
        driver.Quit();
    }
}