using FND.API.Entities;
using System.Xml;
using System.ServiceModel.Syndication;
using HtmlAgilityPack;
using Microsoft.IdentityModel.Tokens;
using FND.API.Data.Repositories;
using FND.API.Data;
using FND.API.Services;
using FND.API.Data.Dtos;

namespace FND.API.Fetcher
{
    public class NewsFetcherService : BackgroundService
    {
        private PublisherService publisherService;
        private NewsService newsService;
        public NewsFetcherService(IServiceProvider serviceProvider)
        {
            publisherService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<PublisherService>();
            newsService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<NewsService>();
            //https://iqan.medium.com/how-to-create-a-timely-running-service-in-net-core-757f445035ca
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await fetchNews();
            var timer = new PeriodicTimer(TimeSpan.FromMinutes(15));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await fetchNews();
            }
        }

        private async Task fetchNews()
        {
            // Run the fetching part.
            List<Publication> publishers = await loadPublishersAsync();
            foreach (Publication publisher in publishers)
            {
                fetchAndClassifyLastestNews(publisher);
            }
        }

        private async void fetchAndClassifyLastestNews(Publication publisher)
        {
            List<ClassifyNewsDto> newsList = new List<ClassifyNewsDto>();
            using var reader = XmlReader.Create(publisher.RSS_Url);
            var feed = SyndicationFeed.Load(reader);
            foreach (var item in feed.Items)
            {
                if (item == null || item.Links == null)
                {
                    continue;
                }
                Uri newsUrl = item.Links[0].Uri;
                if (newsUrl.ToString() == publisher.LastFetchedNewsUrl )
                {
                    Console.WriteLine($"No new News to be fetched for : {publisher.Publication_Name}");
                    return;
                }
                if (newsList.IsNullOrEmpty())
                {
                    publisherService.updateLastFetchedNews(publisher.Publication_Id, newsUrl.ToString());
                    // This in the latest fetched news url of the publisher. This needs to be updated in the DB
                }

                using (var httpClient = new HttpClient())
                {
                    var htmlContent = await httpClient.GetStringAsync(newsUrl);

                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(htmlContent);


                    var itemFullTextDiv = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, '" + publisher.NewsDiv + "')]");

                    if (itemFullTextDiv != null)
                    {
                        var paragraphs = itemFullTextDiv.SelectNodes(".//p");
                        var divs = itemFullTextDiv.SelectNodes(".//div");
                        var content =  "";
                        if (paragraphs != null)
                        {
                            foreach (var paragraph in paragraphs)
                            {
                                content = content + "\n" + paragraph.InnerText;
                                
                            }
                        }
                        if (divs != null)
                        {
                            foreach (var div in divs)
                            {
                                content = content + "\n" + div.InnerText;

                            }
                        }
                        ClassifyNewsDto news = new ClassifyNewsDto();
                        news.Url = newsUrl.ToString();
                        news.Publication_Id = publisher.Publication_Id;
                        news.Topic = item.Title.Text;
                        news.Content = content;
                        var result = await newsService.ClassifyNews(news);
                        Console.WriteLine($"News : {news.Url} , classification result : {result}");
                        newsList.Add(news);
                    }
                    else
                    {
                        Console.WriteLine("Content not found.");
                    }
                }
            }
            return;
        }

        private async Task<List<Publication>> loadPublishersAsync()
        {
            var publishers = await publisherService.GetApprovedPublication();
            // Load the Publisher data from the DB.
            List<Publication> publisherList = new List<Publication>();
            foreach (Users user in publishers)
            {
                Publication pub = user.Publication;
                if (pub == null || string.IsNullOrWhiteSpace(pub.NewsDiv)) {
                    continue;
                }
                publisherList.Add(pub);
            }
            //Publisher pub = new Publisher()
            //{
            //    id = 1,
            //    rssUrl = "https://news.lk/news?format=feed",
            //    lastFetchedNewsUrl = "https://news.lk/news/political-current-affairs/item/35550-foreign-minister-ali-sabry-to-visit-iran",
            //    newsDiv = "itemFullText"
            //};
            //publisherList.Add(pub);
            //Publisher pub1 = new Publisher()
            //{
            //    id = 2,
            //    rssUrl = "https://newsfirst.lk/feed",
            //    lastFetchedNewsUrl = "https://www.newsfirst.lk/2023/08/05/donald-trump-pleads-not-guilty-to-additional-charges-in-documents-case/",
            //    newsDiv = "editor-style"
            //};
            //publisherList.Add(pub1);
            return publisherList;
        }
    }
}
