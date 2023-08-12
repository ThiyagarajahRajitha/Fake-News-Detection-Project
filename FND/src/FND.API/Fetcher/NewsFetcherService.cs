using FND.API.Entities;
using System.Xml;
using System.ServiceModel.Syndication;
using HtmlAgilityPack;
using Microsoft.IdentityModel.Tokens;

namespace FND.API.Fetcher
{
    public class NewsFetcherService : BackgroundService
    {        
        public NewsFetcherService()
        {
            ////https://iqan.medium.com/how-to-create-a-timely-running-service-in-net-core-757f445035ca
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await fetchNews();
            var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await fetchNews();
            }
        }

        private async Task fetchNews()
        {
            // Run the fetching part.
            List<Publisher> publishers = loadPublishers();
            foreach (Publisher publisher in publishers)
            {
                List<News> news = await fetchLastestNews(publisher);
            }
        }

        private async Task<List<News>> fetchLastestNews(Publisher publisher)
        {
            List<News> newsList = new List<News>();
            using var reader = XmlReader.Create(publisher.rssUrl);
            var feed = SyndicationFeed.Load(reader);
            foreach (var item in feed.Items)
            {
                if (item == null || item.Links == null)
                {
                    continue;
                }
                Uri newsUrl = item.Links[0].Uri;
                if (newsUrl.ToString() == publisher.lastFetchedNewsUrl )
                {
                    return newsList;
                }
                if (newsList.IsNullOrEmpty())
                {
                    // This in the latest fetched news url of the publisher. This needs to be updated in the DB
                }

                using (var httpClient = new HttpClient())
                {
                    var htmlContent = await httpClient.GetStringAsync(newsUrl);

                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(htmlContent);


                    var itemFullTextDiv = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, '" + publisher.newsDiv + "')]");

                    if (itemFullTextDiv != null)
                    {
                        var paragraphs = itemFullTextDiv.SelectNodes(".//p");
                        var content =  "";
                        if (paragraphs != null)
                        {
                            foreach (var paragraph in paragraphs)
                            {
                                content = content + "\n" + paragraph.InnerText;
                                
                            }
                        }
                        News news = new News();
                        news.Url = newsUrl.ToString();
                        news.Publisher_id = publisher.id;
                        news.Topic = item.Title.Text;
                        news.Content = content;
                        Console.WriteLine($"News : {news.Url}");
                        newsList.Add(news);
                    }
                    else
                    {
                        Console.WriteLine("Content not found.");
                    }
                }
            }
            return newsList;
        }

        private List<Publisher> loadPublishers()
        {
            // Load the Publisher data from the DB.
            List<Publisher> publisherList = new List<Publisher>();
            Publisher pub = new Publisher()
            {
                id = 1,
                rssUrl = "https://news.lk/news?format=feed",
                lastFetchedNewsUrl = "https://news.lk/news/political-current-affairs/item/35550-foreign-minister-ali-sabry-to-visit-iran",
                newsDiv = "itemFullText"
            };
            publisherList.Add(pub);
            Publisher pub1 = new Publisher()
            {
                id = 2,
                rssUrl = "https://newsfirst.lk/feed",
                lastFetchedNewsUrl = "https://www.newsfirst.lk/2023/08/05/donald-trump-pleads-not-guilty-to-additional-charges-in-documents-case/",
                newsDiv = "editor-style"
            };
            publisherList.Add(pub1);
            return publisherList;
        }
    }
}
