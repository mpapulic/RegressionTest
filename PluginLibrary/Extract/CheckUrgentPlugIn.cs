using Microsoft.VisualStudio.TestTools.WebTesting;
using System.ComponentModel;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace PluginLibrary
{
    [DisplayName("Submission status")]
    [Description("Check does submission is urgent or very urgent.")]
    public class CheckUrgentPlugIn : ExtractionRule
    {
        [DisplayName("Checking does chosen submission is in status URGENT or VERY urgent")]
        [Description("Urgent or VeyUrgent submission is found")]
        public string IsUrgentValid { get; set; }
        public override void Extract(object sender, ExtractionEventArgs e)
        {

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(e.Response.BodyString);
            string typeOfSubmission = CheckUrgent.UrgentSubmission(doc, e.WebTest.Context["submissionGUID"].ToString());
            // check is test for URGENT submission
            if (e.WebTest.Context["IsUrgentSubmission"].ToString().ToUpper() == "TRUE")
            {
                if (typeOfSubmission == "URGENT")
                    e.WebTest.Context.Add("IsUrgentValid", "PASS");
                else
                    e.WebTest.Context.Add("IsUrgentValid", "FAILED");

            }
            else
            {
                if (e.WebTest.Context["IsVeryUrgentSubmission"].ToString().ToUpper() == "TRUE")
                {
                    if (typeOfSubmission == "VERYURGENT")
                        e.WebTest.Context.Add("IsUrgentValid", "PASS");
                    else
                        e.WebTest.Context.Add("IsUrgentValid", "FAILED");
                }
            }
            // check is test for VERYURGENT submission
        }
    }
    public static class CheckUrgent
    {
        public static string UrgentSubmission(HtmlAgilityPack.HtmlDocument doc, string submission_guid)
        {
            string urgent = "";
            //System.IO.File.WriteAllText(@"C:\Temp\UrgentSubmission0.txt", $"Usao u UrgentSubmission , ulazni parametri rezultat za submission: /r /n {doc} /r /n TRAZI SE SUBMISSION GUID: {submission_guid}.");

            // list of the submissions
            List<string> tableSubmissions = new List<string>();

            // searching for all tables in the HTM document
            int k = 1;
            HtmlNodeCollection nodesTable = doc.DocumentNode.SelectNodes("//table");
            // priprema tabele za preuzimanje svih vrednosti iz Global Submission Liste

            // passing through all tables in the HTML document
            int iTablesCount = nodesTable.Count;
            if (iTablesCount == 0) return urgent;
            foreach (var TABLE in nodesTable)
            {
                // Searching all rows in the table [k]
                HtmlNodeCollection nodesRows = TABLE.SelectNodes($"//table[{k}]//tr");
                if (nodesRows != null)
                {
                    int j = 0;
                    // prolazak kroz svako polje i cuvanje stringa sa podacima o svakom submissionu
                    foreach (var ROW in nodesRows)
                    {
                        HtmlNodeCollection nodesCells = ROW.SelectNodes($"//tr[{j}]//td");
                        if (nodesCells != null)
                        {
                            // row which contains data
                            string a = "";
                            int i = 0;
                            // 
                            foreach (var CELL in nodesCells)
                            {
                                // upis vrednosti detalja i atributa
                                a = a + "," + CELL.OuterHtml.Trim('\n', '\r', ' ');
                                i++;
                            }
                            tableSubmissions.Add(a);
                        }
                        else
                        {
                            // headers of the columns
                            HtmlNodeCollection nodesHeaders = ROW.SelectNodes($"//tr[{j}]//th");
                            if (nodesHeaders != null)
                            {
                                string h = "";
                                int iCellsCount = nodesHeaders.Count;
                                foreach (var HEADER in nodesHeaders)
                                {
                                    h = h + "," + HEADER.InnerText.Trim('\n', '\r', ' ');
                                }
                                tableSubmissions.Add(h);
                            }
                        }
                        j++;
                    }
                }
                k++;
            }

            //System.IO.File.WriteAllText(@"C:\Temp\UrgentSubmission1.txt", $"Obradjen response i evo tabele: /r /n {tableSubmissions.ToString()} .");
            string chosenRow = tableSubmissions.Find(x => x.Contains(submission_guid));
            //System.IO.File.WriteAllText(@"C:\Temp\UrgentSubmission2.txt", $"Odabrani red je: /r /n {chosenRow} .");
            if (chosenRow.Contains(@"class=""ultraUrgent"""))
                urgent = "VERYURGENT";
            else
            if (chosenRow.Contains(@"class=""urgent"""))
                urgent = "URGENT";
            //System.IO.File.WriteAllText(@"C:\Temp\UrgentSubmission3.txt", $"Vraca se status : /r /n {urgent} .");
            return urgent;
        }
    }

}
