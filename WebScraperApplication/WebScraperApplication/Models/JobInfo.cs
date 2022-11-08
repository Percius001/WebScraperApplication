using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraperApplication.Models
{
    public class JobInfo
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public List<string> Responsibilites { get; set; }
        public List<string> BasicQualifications { get; set; }
        public List<string> PreferredQualifications { get; set; }

    }
}
