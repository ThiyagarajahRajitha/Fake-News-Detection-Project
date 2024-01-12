using FND.API.Data.Dtos;
using FND.API.Entities;
using FND.API.Interfaces;
using HtmlAgilityPack;
using Microsoft.IdentityModel.Tokens;
using System.ServiceModel.Syndication;
using System.Xml;

namespace FND.API.Fetcher
{
    public class NewsFetcherService : BackgroundService
    {
        private IPublisherService publisherService;
        private INewsService newsService;
        public NewsFetcherService(IServiceProvider serviceProvider)
        {
            publisherService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IPublisherService>();
            newsService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<INewsService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await fetchNews();
            var timer = new PeriodicTimer(TimeSpan.FromMinutes(5));
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
            XmlReader reader = null;
            try
            {
                reader = XmlReader.Create(publisher.RSS_Url);
            } catch(Exception ex)
            {
                Console.WriteLine("Error while loading the rss url " +  publisher.RSS_Url, ex.Message);
                return;
            }
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

                try
                {
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
                            var content = "";
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
                } catch(Exception ex) {
                    Console.WriteLine("Error while loading the news url" + newsUrl, ex.Message);
                    continue;
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
            
            return publisherList;
        }
    }
}
