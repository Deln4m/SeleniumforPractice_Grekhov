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
        options.AddArguments("--no-sandbox", "--window-size=1920, 540", "--disable-extensions");
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
        var Login = driver.FindElement(By.Id("Username"));
        Login.SendKeys("Delnam@outlook.com");
        // Пароль
        var Password = driver.FindElement(By.Name("Password"));
        Password.SendKeys("Suabru22b22-");
        // Клик
        var Submit = driver.FindElement(By.CssSelector("[value='login']"));
        Submit.Click();
        // Ждём редирект
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(ExpectedConditions.UrlContains("https://staff-testing.testkontur.ru/news"));
        // Проверяем редирект
        Assert.That(driver.Url == "https://staff-testing.testkontur.ru/news", 
            "После авторизации не попадаем на урл Новостей");
    }
    
    
    // Проверка навигации через боковое меню
    [Test]
    public void SideMenuNavigation()
    {
        // Открыть боковое меню
        var expandmenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        expandmenu.Click();
        // Переход по вкладке "Сообщества"
        var communities = driver.FindElements(By.CssSelector("[data-tid='Community']"))
            .First(elemnt => elemnt.Displayed);
        communities.Click();
        // Проверка юрла
        var title = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        Assert.That(driver.Url == "https://staff-testing.testkontur.ru/communities", 
            "На странице 'Сообщества' не тот урл");
        TearDown();
    }
    
    // Проверка строки поиска (переход на страницу профиля пользователя)
    [Test]
    public void Search()
    {
        // Клик на иконку поиска
        var SearchIconSelect = driver.FindElement(By.CssSelector("[data-tid='Services']"));
        SearchIconSelect.Click();
        // Клик на строку поиска
        var SearchBarSelect = driver.FindElement(By.CssSelector("[data-tid='SearchBar']"));
        SearchBarSelect.Click();
        // Ввод текста
        var SearchBarType = driver.FindElement(By.CssSelector("[type='text']"));
        SearchBarType.SendKeys("Даниил Грехов");
        SearchBarType.Should().NotBeNull();
        // Клик на результат поиска 
        var User = driver.FindElement(By.CssSelector("[title='Даниил Грехов']"));
        User.Click();
        // Проверка верности юрла
        var position = driver.FindElement(By.CssSelector("[data-tid='Tabs']"));
        position.Should().NotBeNull();
    }

    // Проверка появления модалки журнала изменений по клику на версию
    [Test]
    public void ModalMenu()
    {
        // Открываем боковое меню
        var expandmenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        expandmenu.Click();
        // Кликаем на версию
        var version = driver.FindElement(By.CssSelector("[data-tid='SidePage__container']"))
            .FindElement(By.CssSelector("[data-tid='Version']")); 
        version.Click();
        // Проверка на появление модалки
        var OpenModal = driver.FindElement(By.CssSelector("[data-tid='modal-content']"));
        OpenModal.Should().NotBeNull();

    }
    
    // Проверка кликабельности аватарки в профиле

    [Test]

    public void AvatarClickable()
    {
        // Переходим на юрлу профиля
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/profile/ddc96d3f-f5fd-4975-829f-375f6c3bb707");
        // Кликаем на автаарку
        var Avatar = driver.FindElement(By.CssSelector("[data-tid='PageHeader']"))
            .FindElement(By.CssSelector("[data-tid='Avatar']"));
        Avatar.Click();
        // Проверяем, что открылось меню просмотра фотографии
        var MenuCheck = driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/div[2]/div/div"));
        MenuCheck.Should().NotBeNull();
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

