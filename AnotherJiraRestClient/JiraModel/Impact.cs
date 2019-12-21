using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnotherJiraRestClient.JiraModel
{
    public class Impact
    {

        public static Impact UNKNOWN_VERSION = new Impact()
        {
            value = string.Empty,
            id = string.Empty,
            self = string.Empty
        };


        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }

    }
}
