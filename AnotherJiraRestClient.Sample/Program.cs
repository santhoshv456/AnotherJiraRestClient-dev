using AnotherJiraRestClient.JiraModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Timers;

namespace AnotherJiraRestClient.Sample
{
	class Program
	{
        private static int seconds = 0;
        static string jiraUrl = ConfigurationSettings.AppSettings["ProjectUrl"].ToString();
        static string userName = ConfigurationSettings.AppSettings["UserEmailId"].ToString();
        static string password = ConfigurationSettings.AppSettings["JiraApiKey"].ToString();
        static string mailFrom = ConfigurationSettings.AppSettings["mailFrom"].ToString();
        static string mailTo = ConfigurationSettings.AppSettings["mailTo"].ToString();
        static string mailCc = ConfigurationSettings.AppSettings["mailCc"].ToString();
        static string timer = ConfigurationSettings.AppSettings["timer"].ToString();
        static string sendGridKey= ConfigurationSettings.AppSettings["sendGridKey"].ToString();

        static void Main(string[] args)
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = Convert.ToInt32(timer);
            aTimer.Enabled = true; 

            Console.WriteLine("Press \'q\' and Enter to quit the App...");
            while (Console.Read() != 'q') ;
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            JiraClient client = Client();

            /* string JqlAssigne = "created > -30d and status not in (Closed, Completed, Resolved, Done, Cancelled, Canceled) and type in ('Incident','Alerts') and reporter not in ('aafreen.wahab - ext','abhijeet.babar - ext','jkarana2','jmangapr','soham.ghosh - ext','jgopalpa','sonu.singh - ext','amulya.1.martha - ext','jghoshku','santhoshk.vullakula - ext','gopi.krishna.gaddagunti - ext')";

             Issues assigneObj = client.GetIssuesByJql(JqlAssigne, 0, 999);

             if (assigneObj.issues != null)
             {
                 foreach (var a in assigneObj.issues)
                 {
                     if (!a.key.Contains("GSRE"))
                     {
                         if (a.fields.assignee == null)
                         {
                             sendGrid.Execute(mailFrom, mailTo, "Assignee Not Found...", mailCc, constructHtml(a, null), sendGridKey).Wait();
                         }
                     }
                 }
             }*/

            string Jql = "created > -600d and status not in (Closed, Completed, Resolved, Done, Cancelled, Canceled) and type in ('Incident','Alerts') and assignee in  ('aafreen.wahab - ext','abhijeet.babar - ext','jkarana2','jmangapr','soham.ghosh - ext','jgopalpa','sonu.singh - ext','amulya.1.martha - ext','jghoshku','santhoshk.vullakula - ext','gopi.krishna.gaddagunti - ext') and reporter not in ('aafreen.wahab - ext','abhijeet.babar - ext','jkarana2','jmangapr','soham.ghosh - ext','jgopalpa','sonu.singh - ext','amulya.1.martha - ext','jghoshku','santhoshk.vullakula - ext','gopi.krishna.gaddagunti - ext')";

            // string Jql = "created > -600d and type in ('Incident','Alerts') and assignee in  ('aafreen.wahab - ext','abhijeet.babar - ext','jkarana2','jmangapr','soham.ghosh - ext','jgopalpa','sonu.singh - ext','amulya.1.martha - ext','jghoshku','santhoshk.vullakula - ext','gopi.krishna.gaddagunti - ext')";
            //and reporter not in ('aafreen.wahab - ext','abhijeet.babar - ext','jkarana2','jmangapr','soham.ghosh - ext','jgopalpa','sonu.singh - ext','amulya.1.martha - ext','jghoshku','santhoshk.vullakula - ext','gopi.krishna.gaddagunti - ext')";


            Issues obj = client.GetIssuesByJql(Jql, 0, 999);

            string resolveHtml = " <h3>Incidents and Alerts:</h3> " +
                                       " <br/> " +
                                       " <table border='1'> " +
                                       " <tr> " +
                                       " <th> Issue Key </th> " +
                                       " <th> Summary </th> " +
                                       " <th> Assignee </th> " +
                                       " <th> Status </th> " +
                                       " <th> Remaining Time in Hours </th > " +
                                       " </tr> ";

            string slaBreachHtml = " <h3>Incidents and Alerts:</h3> " +
                                       " <br/> " +
                                       " <table border='1'> " +
                                       " <tr> " +
                                       " <th> Issue Key </th> " +
                                       " <th> Summary </th> " +
                                       " <th> Assignee </th> " +
                                       " <th> Status </th> " +
                                       " <th> Remaining Time in Hours </th > " +
                                       " </tr> ";

            if (obj.issues != null)
            {
                foreach (var a in obj.issues)
                {
                    switch (a.fields.issuetype.name)
                    {
                        case "Alerts":
                            {
                                if (a.fields.customfield_10049 != null && a.fields.customfield_10049.value.ToUpper().Contains("S1") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours < 4)
                                {
                                    resolveHtml += constructHtml(a, 4);
                                }
                                if (a.fields.customfield_10049 != null && a.fields.customfield_10049.value.ToUpper().Contains("S1") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours > 4)
                                {
                                    slaBreachHtml += constructHtml(a, 4);
                                }
                                if (a.fields.customfield_10049 != null && a.fields.customfield_10049.value.ToUpper().Contains("S2") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours < 8)
                                {
                                    resolveHtml += constructHtml(a, 8);
                                }
                                if (a.fields.customfield_10049 != null && a.fields.customfield_10049.value.ToUpper().Contains("S2") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours > 8)
                                {
                                    slaBreachHtml += constructHtml(a, 8);
                                }
                            }
                            break;
                        case "Incident":
                            {
                                if (a.fields.customfield_10049 != null && a.fields.customfield_10049.value.ToUpper().Contains("S1") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours < 4)
                                {
                                    resolveHtml += constructHtml(a, 4);
                                }
                                if (a.fields.customfield_10049 != null && a.fields.customfield_10049.value.ToUpper().Contains("S1") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours > 4)
                                {
                                    slaBreachHtml += constructHtml(a, 4);
                                }
                                if (a.fields.customfield_10049 != null && a.fields.customfield_10049.value.ToUpper().Contains("S2") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours < 8)
                                {
                                    resolveHtml += constructHtml(a, 8);
                                }
                                if (a.fields.customfield_10049 != null && a.fields.customfield_10049.value.ToUpper().Contains("S2") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours > 8)
                                {
                                    slaBreachHtml += constructHtml(a, 8);
                                }
                                if (a.fields.customfield_10049 != null && a.fields.customfield_10049.value.ToUpper().Contains("S3") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours < 16)
                                {
                                    resolveHtml += constructHtml(a, 16);
                                }
                                if (a.fields.customfield_10049 != null && a.fields.customfield_10049.value.ToUpper().Contains("S3") && a.fields.resolutiondate == null && DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours > 16)
                                {
                                    slaBreachHtml += constructHtml(a, 16);
                                }
                            }
                            break;
                        default:
                            Console.WriteLine("Default case");
                            break;
                    }
                }

                resolveHtml += " </table> ";
                slaBreachHtml += " </table> ";

                sendGrid.Execute(mailFrom, mailTo, "Resolution is in Pending...", mailCc, resolveHtml, sendGridKey).Wait();
                sendGrid.Execute(mailFrom, mailTo, "SLA Breached for Below Incidents", mailCc, slaBreachHtml, sendGridKey).Wait();
            }

            Console.WriteLine("Completed...");
        }

        private static JiraClient Client()
		{
            var client = new JiraClient(new JiraAccount
			{
				ServerUrl = jiraUrl,
				User = userName,
				Password = password
			});
			return client;
		}

        private static string constructHtml(Issue a,int? impHours)
        {         
                    return    " <tr> " +
                              " <td> " + a.key + " </td> " +
                              " <td> " + a.fields.summary + " </td> " +
                              " <td> " + a.fields.assignee.name + " </td> " +
                              " <td> " + a.fields.status.name + " </td> " +
                              " <td> " + (impHours - Convert.ToInt32(DateTime.Now.Subtract(DateTime.Parse(a.fields.created)).Duration().TotalHours)) + " </td> " +
                              " </tr> ";

        }

	}
}
