using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Net.Http;
using HtmlAgilityPack;
using ExamProject.SQLite;
using ExamProject.Models;

namespace ExamProject.Wired
{
    public class GetWired
    {
        public async void getWiredText()
        {
            ExamDbCRUD exdr = new ExamDbCRUD();
            var url = "https://www.wired.com/feed/rss";
            using var reader = XmlReader.Create(url);
            var feed = SyndicationFeed.Load(reader);
            HttpClient client = new HttpClient();
            var items = feed.Items.ToList();
            var i = 0;
            var k = 0;
            while (i<5)
            {
                k++;
                using (var response = await client.GetAsync(items[k].Links[0].Uri))
                {
                    using (var content = response.Content)
                    {
                        // read answer in non-blocking way
                        var result = await content.ReadAsStringAsync();
                        var document = new HtmlDocument();
                        document.LoadHtml(result);
                        if (items[k].Links[0].Uri.ToString().Contains("gallery"))
                        {
                            continue;
                        }
                        var nodes = document.DocumentNode.SelectNodes("//div[@class='body__inner-container']").ToList();
                        var header = document.DocumentNode.SelectNodes("//h1").ToList()[0].InnerText;
                        var text = "";
                        foreach (var node in nodes)
                        {
                            text += node.InnerText;
                        }
                        i++;
                        exdr.AddWiredText(text,header);
                    }
                }

            }
            ExamDb.connection.Close();
        }
    }
}
