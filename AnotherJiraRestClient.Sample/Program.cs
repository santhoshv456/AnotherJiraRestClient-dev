using AnotherJiraRestClient.JiraModel;
using System;
using System.Collections.Generic;
using System.Net;

namespace AnotherJiraRestClient.Sample
{
	class Program
	{
		static void Main(string[] args)
		{
            //sendGrid.Execute("","","","").Wait();

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            JiraClient client = Client(args);

            string Jql = "'Impact Start Time' is empty AND  created >= '2019-08-01 07:00' and created <='2019-12-14 23:59' and type in ('Incident')";

			string projectKey = args[3];
			string issueKey = projectKey + "-" + args[4];
			string customFieldToUpdate = args[5];

            List<Project> Pro =client.GetProjects();

            Issues obj=client.GetIssuesByJql(Jql,0,4);

			//ProjectMeta projectMetaData = client.GetProjectMeta(projectKey);

			Issue issueWithAllFields = client.GetIssue(issueKey);          

            if (obj.issues != null)
            {
                foreach (var a in obj.issues)
                {
                    switch (a.fields.issuetype.name)
                    {
                        case "Alerts":
                            break;
                        case "Incident":
                            {
   
                                if (a.fields.summary.ToUpper().Contains("S1") && a.fields.resolutiondate!=null && DateTime.Parse(a.fields.resolutiondate).Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours>4)
                                {
                                    sendGrid.Execute("santhoshk.vullakula-ext@jci.com", "santhoshk.vullakula-ext@jci.com", a.fields.summary, "<strong>" + a.fields.summary + "</strong>").Wait();
                                }
                                if(a.fields.summary.ToUpper().Contains("S1") && a.fields.resolutiondate==null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours>4)
                                {

                                }
                                if(a.fields.summary.ToUpper().Contains("S1") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours<4)
                                {

                                }
                            }
                            break;
                        default:
                            Console.WriteLine("Default case");
                            break;
                    }                 
                }
            }

            Console.WriteLine("Completed...");
            Console.ReadKey();
        }

		private static JiraClient Client(string[] args)
		{
			var jiraUrl = args[0];
			var userName = args[1];
			var password = args[2];

			var client = new JiraClient(new JiraAccount
			{
				ServerUrl = jiraUrl,
				User = userName,
				Password = password
			});
			return client;
		}
	}
}
