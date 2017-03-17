using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.IO;
using System.Linq;
using PluginLibrary.Models;

namespace PluginLibrary.Helper
{
    public static class GetMyDashboardSubmissions
    {
        public static string GetSubmissionByTitle(HtmlAgilityPack.HtmlDocument doc, string _title)
        {

            string _submissionID = "";
            List<Submission> listSubmissions = new List<Submission>();


            //listSubmissions = GetAllSubmissions(doc, "");

            int index = listSubmissions.FindIndex(x => x.Title.Contains(_title));
            if (index != -1)
                _submissionID = listSubmissions[index].SubmissionGUID;
            return _submissionID;
        }
        public static List<Submission> GetAllSubmissions(HtmlAgilityPack.HtmlDocument doc, string tableCondition)
        {
            // list of the Submissions on the page
            List<Submission> listSubmission = new List<Submission>();
            HtmlNode TabeleUhtml;
            string uslov;


            // searching for all tables in the HTML document
            if (tableCondition != "")
            {
                uslov = "//table[@id='" + tableCondition + "']";
            }
            else
            {
                uslov = "//table";
            }
            try
            {
                TabeleUhtml = doc.DocumentNode.SelectSingleNode(uslov);
                if (TabeleUhtml != null)
                {
                    // podesavam se na zeljenu poziciju i nakon toga unutar tog noda uzimam sve sledece
                    HtmlNode PrviReduTabeliSaHref = doc.DocumentNode.SelectNodes(uslov).First();
                    HtmlNode[] redoviHref = PrviReduTabeliSaHref.SelectNodes($".//tr//td//a").ToArray();
                    // preuzima sve nodove koji imaju tag a u redu koloni td i redu tr pocevsi od prvog sloga u toj tabeli tr
                    // obrada svakog noda
                    int i = 1;
                    foreach (var CELL in redoviHref)
                    {
                        string guid = "";
                        string outerhtml = CELL.OuterHtml.Trim('\n', '\r', ' ');

                        // odabir noda koji sadrzi podataka o title i guid submissiona
                        if (outerhtml.Contains("Submission"))
                        {
                            string title = CELL.InnerText.Trim('\n', '\r', ' ');
                            if (outerhtml.Contains("Details"))
                                guid = outerhtml.Substring(outerhtml.IndexOf("Details") + 8, 36);
                            else
                            if (outerhtml.Contains("Edit"))
                                guid = outerhtml.Substring(outerhtml.IndexOf("Edit") + 5, 36);

                            listSubmission.Add(new Submission
                            {
                                Title = title,
                                SubmissionGUID = guid,
                            });
                        }
                    }
                    i++;
                }
            }
            catch (InvalidCastException e)
            {
                //Logger.Log($"Greska se javila zbog : {e.Message}  ", sw);
            }
            return listSubmission;
        }
    }
}
