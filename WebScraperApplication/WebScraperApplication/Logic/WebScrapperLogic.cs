using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebScraperApplication.Models;

namespace WebScraperApplication.Logic
{
    //using singleton desing pattern
    public sealed class WebScrapperLogic
    {
        private static readonly WebScrapperLogic webScrapperLogic = new WebScrapperLogic();
        private WebScrapperLogic()
        { }
        public static WebScrapperLogic Instance()
            { return webScrapperLogic; }

        //gets the response from the webpages
        public async Task<List<JobInfo>> GetDataFromWebPage()
        {
            List<JobInfo> data = new List<JobInfo>();
            string url = "https://boards.greenhouse.io/embed/job_board?for=coursera";
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(url);
            var jobList = ParseHTMLJobs(response); 
            foreach (var jobId in jobList)
            {
                response = await httpClient.GetStringAsync("https://boards.greenhouse.io/embed/job_app?for=coursera&token="+jobId);
                JobInfo jobInfo = ParseHTMLJobInfo(response);
                data.Add(jobInfo);
            }
            return data;
        }

        //parse the html string to get the job ids.
        public List<string> ParseHTMLJobs(string htmlData)
        {
            List<string> result = new List<string>();   
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlData);
            var jobLinks = htmlDocument.DocumentNode.Descendants("a")
                            .Where(node => node.GetAttributeValue("href","").Contains("jobs?gh_jid"))
                            .ToList();
            foreach(var link in jobLinks)
            {
                if (link.Attributes.Count > 0)
                    result.Add(link.Attributes[2].Value.Substring(link.Attributes[2].Value.IndexOf("=")+1));
            }
            return result;
        }

        //parse the html string to get all the information from a job.
        public JobInfo ParseHTMLJobInfo(string htmlData)
        {
            JobInfo jobInfo = new JobInfo();
            int des = 0,res = 0,bq = 0,pq = 0;
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlData);
            var header = htmlDocument.DocumentNode.Descendants("div")
                            .Where(node => node.GetAttributeValue("id", "").Contains("header"))
                            .ToList();
            jobInfo.Title = header[0].ChildNodes[1].InnerText.Replace(@"&amp;", "&");
            jobInfo.Location = Regex.Replace(header[0].ChildNodes[5].InnerText, @"\t|\n|\r", "").Trim();
            var content = htmlDocument.DocumentNode.Descendants("div")
                            .Where(node => node.GetAttributeValue("id", "").Contains("content"))
                            .ToList();
            
            for(int i = 0; i < content[0].ChildNodes.Count; i++)
            {
                if (content[0].ChildNodes[i].InnerText.Contains("Job Overview"))
                    des = i + 2;
                if (content[0].ChildNodes[i].InnerText.Contains("Responsibilities"))
                    res = i + 2;
                if (content[0].ChildNodes[i].InnerText.Contains("Basic Qualifications") || content[0].ChildNodes[i].InnerText.Contains("Your Skills"))
                    bq = i + 2;
                if (content[0].ChildNodes[i].InnerText.Contains("Preferred Qualifications"))
                    pq = i + 2;
            }
            jobInfo.Description = Regex.Replace(content[0].ChildNodes[des].InnerText, @"&amp;","&").Trim();
            jobInfo.Responsibilites = Regex.Replace(content[0].ChildNodes[res].InnerText, @"&amp;", "&").Trim().Split("\n").ToList();
            jobInfo.BasicQualifications = Regex.Replace(content[0].ChildNodes[bq].InnerText, @"&amp;", "&").Trim().Split("\n").ToList();
            jobInfo.PreferredQualifications = Regex.Replace(content[0].ChildNodes[pq].InnerText, @"&amp;", "&").Trim().Split("\n").ToList();
            return jobInfo;
        }

    }
}
