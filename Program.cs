using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Collections.Generic;

namespace FindWikiLinks
{
    class FindWikiLinks
    {
        public const string WIKILINK = @"https://en.wikipedia.org/wiki/";

        public static void Main(string[] args)
        {

            //ChromeOptions options = new ChromeOptions();
            //options.AddArguments("headless");

            IWebDriver driver = new ChromeDriver();

            try
            {
                System.Console.WriteLine("Enter article title in Wikipedia:");
                string userProvidedArticle = Console.ReadLine();
                List<string> links = new List<string>();
                GetWikiLinks(1, driver, $"{WIKILINK}{userProvidedArticle}", 0, links);

            }
            finally
            {
                driver.Quit();
            }
        }

        public static void GetWikiLinks(int maxLevels, IWebDriver driver, string link, int currentLevel, List<string> links)
        {
            if (currentLevel >= maxLevels || maxLevels >= 20 || string.IsNullOrEmpty(link))
                return;


            // Navigate to Url
            driver.Navigate().GoToUrl(link);

            try
            {
                var element = driver.FindElement(By.XPath("//*[text()='Wikipedia does not have an article with this exact name.']"));
                if (element != null)
                {
                    System.Console.WriteLine($"Article does not exist in WIKI!");
                    return;
                }
            }
            catch (NoSuchElementException e)
            {

            }

            // Get all the links
            IList<IWebElement> elements = driver.FindElements(By.XPath("//a[contains(@href, '')]"));


            foreach (IWebElement e in elements)
            {
                links.Add(e.GetAttribute("href"));
                System.Console.WriteLine(e.GetAttribute("href"));
                GetWikiLinks(maxLevels, driver, e.GetAttribute("href"), ++currentLevel, links);
            }

        }

    }
}
