using AnotherJiraRestClient.JiraModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;

namespace AnotherJiraRestClient.Sample
{
	class Program
	{
		static void Main(string[] args)
		{
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            JiraClient client = Client(args);

            string Jql = "created > -600d and status not in (Closed, Completed, Resolved, Done, Cancelled, Canceled) and type in ('Incident','Alerts') and assignee in  ('aafreen.wahab - ext','abhijeet.babar - ext','jkarana2','jmangapr','soham.ghosh - ext','jgopalpa','sonu.singh - ext','amulya.1.martha - ext','jghoshku','santhoshk.vullakula - ext','gopi.krishna.gaddagunti - ext') and reporter not in ('aafreen.wahab - ext','abhijeet.babar - ext','jkarana2','jmangapr','soham.ghosh - ext','jgopalpa','sonu.singh - ext','amulya.1.martha - ext','jghoshku','santhoshk.vullakula - ext','gopi.krishna.gaddagunti - ext') ";

            Issues obj=client.GetIssuesByJql(Jql,0,999);

            if (obj.issues != null)
            {
                foreach (var a in obj.issues)
                {
                    switch (a.fields.issuetype.name)
                    {
                        case "Alerts":
                            {
                                if (a.fields.customfield_10049.value.ToUpper().Contains("S1") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours < 4)
                                {
                                    sendGrid.Execute("jvullas@jci.com", "santhoshv456@gmail.com", "Resolution is in Pending...", constructHtml(a, 4)).Wait();
                                }
                                if (a.fields.customfield_10049.value.ToUpper().Contains("S1") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours > 4)
                                {
                                    sendGrid.Execute("jvullas@jci.com", "santhoshv456@gmail.com", "SLA Breached for Below Alerts", constructHtml(a, 4)).Wait();
                                }
                                if (a.fields.customfield_10049.value.ToUpper().Contains("S2") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours < 8)
                                {
                                    sendGrid.Execute("jvullas@jci.com", "santhoshv456@gmail.com", "Resolution is in Pending...", constructHtml(a, 8)).Wait();
                                }
                                if (a.fields.customfield_10049.value.ToUpper().Contains("S2") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours > 8)
                                {
                                    sendGrid.Execute("jvullas@jci.com", "santhoshv456@gmail.com", "SLA Breached for Below Alerts", constructHtml(a, 8)).Wait();
                                }
                            }
                            break;
                        case "Incident":
                            {
                                if (a.fields.customfield_10049.value.ToUpper().Contains("S1") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours < 4)
                                {
                                    sendGrid.Execute("jvullas@jci.com", "santhoshv456@gmail.com", "Resolution is in Pending...", constructHtml(a, 4)).Wait();
                                }
                                if (a.fields.customfield_10049.value.ToUpper().Contains("S1") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours > 4)
                                {
                                    sendGrid.Execute("jvullas@jci.com", "santhoshv456@gmail.com", "SLA Breached for Below Incidents", constructHtml(a,4)).Wait();
                                }
                                if (a.fields.customfield_10049.value.ToUpper().Contains("S2") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours < 8)
                                {
                                    sendGrid.Execute("jvullas@jci.com", "santhoshv456@gmail.com", "Resolution is in Pending...", constructHtml(a, 8)).Wait();
                                }
                                if (a.fields.customfield_10049.value.ToUpper().Contains("S2") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours > 8)
                                {
                                    sendGrid.Execute("jvullas@jci.com", "santhoshv456@gmail.com", "SLA Breached for Below Incidents", constructHtml(a,8)).Wait();
                                }
                                if (a.fields.customfield_10049.value.ToUpper().Contains("S3") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours < 16)
                                {
                                    sendGrid.Execute("jvullas@jci.com", "santhoshv456@gmail.com", "Resolution is in Pending...", constructHtml(a, 16)).Wait();
                                }
                                if (a.fields.customfield_10049.value.ToUpper().Contains("S3") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours > 16)
                                {
                                    sendGrid.Execute("jvullas@jci.com", "santhoshv456@gmail.com", "SLA Breached for Below Incidents", constructHtml(a, 16)).Wait();
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
			var jiraUrl = ConfigurationSettings.AppSettings["ProjectUrl"].ToString();
			var userName = ConfigurationSettings.AppSettings["UserEmailId"].ToString();
			var password = ConfigurationSettings.AppSettings["JiraApiKey"].ToString();

            var client = new JiraClient(new JiraAccount
			{
				ServerUrl = jiraUrl,
				User = userName,
				Password = password
			});
			return client;
		}

        private static string constructHtml(Issue a,int impHours)
        {
                                return " <h3>Incidents and Alerts:</h3> " +
                                       " <br/> " +
                                       " <table border='1'> " +
                                       " <tr> " +
                                       " <th> Issue Key </th> " +
                                       " <th> Summary </th> " +
                                       " <th> Assignee </th> " +
                                       " <th> Status </th> " +
                                       " <th> Remaining Time in Hours </th > " +
                                       " </tr> " +
                                       " <tr> " +
                                       " <td> " + a.key + " </td> " +
                                       " <td> " + a.fields.summary + " </td> " +
                                       " <td> " + a.fields.assignee.name + " </td> " +
                                       " <td> " + a.fields.status.name + " </td> " +
                                       " <td> " + (impHours - Convert.ToInt32(DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours)) + " </td> " +
                                       " </tr> " +
                                       " </table> ";

        }

	}
}
