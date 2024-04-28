using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Diagnostics;
namespace SeleniumTestoforPractice_Grekhov;

public class Seleniumtestforprac

{

    public ChromeDriver driver;
    
    [SetUp] 
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--headless", "--disable-extensions");
        driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
        Authorization();
    }   
    
    // Авторизация
    public void Authorization()
    {
        // Урла
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/");
        // Логин
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("Delnam@outlook.com");
        // Пароль
        var password = driver.FindElement(By.Name("Password"));
        password.SendKeys("Suabru22b22-");
        // Клик
        var submit = driver.FindElement(By.CssSelector("[value='login']"));
        submit.Click();
        // Ждём редирект
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(ExpectedConditions.UrlContains("https://staff-testing.testkontur.ru/news"));
    }
    
    
    // Проверка навигации через боковое меню
    [Test]
    public void SideMenuNavigation()
    {
        // Открыть боковое меню
        var expandMenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        expandMenu.Click();
        // Переход по вкладке "Сообщества"
        var communities = driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/div/div[3]/div/div[1]/div[2]/div"))
            .FindElement(By.CssSelector("[data-tid='Community']"));
        communities.Click();
        // Проверка урла
        WebDriverWait waitCommunities = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        waitCommunities.Until(ExpectedConditions.UrlContains("https://staff-testing.testkontur.ru/communities"));
        Assert.That(driver.Url == "https://staff-testing.testkontur.ru/communities", 
            "На странице 'Сообщества' не тот урл");
    }
    
    // Проверка строки поиска (переход на страницу профиля пользователя)
    [Test]
    public void Search()
    {
        // Клик на иконку поиска
        var searchIconSelect = driver.FindElement(By.CssSelector("[data-tid='Services']"));
        searchIconSelect.Click();
        // Клик на строку поиска
        var searchBarSelect = driver.FindElement(By.CssSelector("[data-tid='SearchBar']"));
        searchBarSelect.Click();
        // Ввод текста 
        var searchBarType = driver.FindElement(By.CssSelector("[type='text']"));
        searchBarType.SendKeys("Даниил Грехов");
        // Клик на результат поиска 
        var user = driver.FindElement(By.CssSelector("[title='Даниил Грехов']"));
        user.Click();
        // Проверка верности редиректа
        Assert.That(driver.Url == "https://staff-testing.testkontur.ru/profile/ddc96d3f-f5fd-4975-829f-375f6c3bb707", 
            "На странице профиля не тот урл");
    }

    // Проверка появления модалки журнала изменений по клику на версию
    [Test]
    public void ModalMenu()
    {
        // Открываем боковое меню
        var expandMenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        expandMenu.Click();
        // Кликаем на версию
        var version = driver.FindElement(By.CssSelector("[data-tid='SidePage__container']"))
            .FindElement(By.CssSelector("[data-tid='Version']")); 
        version.Click();
        // Проверка на появление модалки
        var openModal = driver.FindElement(By.CssSelector("[data-tid='modal-content']"));
        openModal.Should().NotBeNull();
    }
    
    // Проверка кликабельности аватарки в профиле

    [Test]

    public void AvatarClickable()
    {
        // Переходим на урл профиля
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/profile/ddc96d3f-f5fd-4975-829f-375f6c3bb707");
        // Кликаем на аватарку
        var avatar = driver.FindElement(By.CssSelector("[data-tid='PageHeader'] [data-tid='Avatar']"));
        avatar.Click();
        // Проверяем, что открылось меню просмотра фотографии
        var menuCheck = driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/div[2]/div/div"));
        menuCheck.Should().NotBeNull();
    }
    // kill chrome.exe 
    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }
    // kill conhost.exe after all tests are completed
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        var cmd = new Process();
        cmd.StartInfo.FileName = "cmd.exe";
        cmd.StartInfo.RedirectStandardInput = true;
        cmd.StartInfo.RedirectStandardOutput = true;
        cmd.StartInfo.CreateNoWindow = true;
        cmd.StartInfo.UseShellExecute = false;
        cmd.Start();
        cmd.StandardInput.WriteLine("taskkill /IM conhost.exe /F");
        cmd.StandardInput.Flush();
        cmd.StandardInput.Close();
        cmd.WaitForExit();
        Console.WriteLine(cmd.StandardOutput.ReadToEnd());
    }
}

