// See https://aka.ms/new-console-template for more information
using WebScraperApplication.Logic;
using WebScraperApplication.Models;

//singleton object creation.
var result = WebScrapperLogic.Instance();

//function call to take the job infos from the webpage. 
var jobInfos = await result.GetDataFromWebPage();

//Output Displaying the job data.
foreach(JobInfo job in jobInfos)
{
    Console.Write("Job Title: ");
    Console.WriteLine(job.Title);
    Console.WriteLine();
    Console.WriteLine("Job Descirption:");
    Console.WriteLine(job.Description);
    Console.WriteLine();
    Console.WriteLine("Responsibilites");
    foreach(string res in job.Responsibilites)
        Console.WriteLine("-> "+res);
    Console.WriteLine();
    Console.WriteLine("Basic Qualifications:");
    foreach (string res in job.BasicQualifications)
        Console.WriteLine("-> " + res);
    Console.WriteLine();
    Console.WriteLine("Preffered Qualifications:");
    foreach (string res in job.PreferredQualifications)
        Console.WriteLine("-> " + res);
    Console.WriteLine();
    Console.WriteLine();
}
Console.ReadLine();