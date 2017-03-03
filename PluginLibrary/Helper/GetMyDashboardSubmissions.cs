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
            //using (StreamWriter sw = File.AppendText(@"C:\Temp\GetSubmissionHTML.txt"))
            //{
            //    sw.WriteLine($"GetSubmissionHTML ulazni parametri: ");
            //    sw.WriteLine($"============================================================================== ");
            //    sw.WriteLine($"Naziv tabele koja se obradjuje: {tableCondition} ");
            //    var brojTabela = doc.DocumentNode.SelectNodes("//table");
            //    sw.WriteLine($"Broj tabela u HTML dokumenta : {brojTabela.Count.ToString()} ");
            //    HtmlNodeCollection trazenaTabela;
            //    HtmlNodeCollection trazeniRedovi;
            //    trazenaTabela = doc.DocumentNode.SelectNodes($"//table[0]");
            //    var node = trazenaTabela[0];
            //    sw.WriteLine($"Sadrzaj u prvoj tabeli HTML tabele : {node.OuterHtml.ToString()}");

            //    if (brojTabela.Count > 0)
            //    {
            //        trazenaTabela = doc.DocumentNode.SelectNodes($"//table[1]");
            //        node = trazenaTabela[1];
            //        sw.WriteLine($"Sadrzaj u u drugoj tabeli HTML tabele : {node.OuterHtml.ToString()} ");
            //        trazenaTabela = doc.DocumentNode.SelectNodes($"//table[2]");
            //        node = trazenaTabela[2];
            //        sw.WriteLine($"Sadrzaj u u trecoj tabeli HTML tabele : {node.OuterHtml.ToString()}");
            //        trazenaTabela = doc.DocumentNode.SelectNodes($"//able[3]");
            //        node = trazenaTabela[3];
            //        sw.WriteLine($"Sadrzaj u u cetvrtoj tabeli HTML tabele : {node.OuterHtml.ToString()} ");
            //    }
            //    sw.WriteLine($"============================================================================== ");

            //}

            // list of the Submissions on the page
            List<Submission> listSubmission = new List<Submission>();
            HtmlNode TabeleUhtml;
            string uslov;


            using (StreamWriter sw = File.AppendText(@"C:\Temp\GetSubmissionHTML.txt"))
            {
                Logger.Log($"Tabela koja se obradjuje : {tableCondition}  ", sw);
                // searching for all tables in the HTML document
                if (tableCondition != "")
                {
                    //TabeleUhtml = doc.DocumentNode.SelectSingleNode($"//table  [@id={idTabele}]");
                    uslov = "//table[@id='" + tableCondition + "']";
                }
                else
                {
                    uslov = "//table";
                }
                Logger.Log($"Uslov pronalaska tabele : {uslov}  ", sw);
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
                                Logger.Log($"Submission: {i}/ {listSubmission.Count.ToString()}   {title} - {guid}  ", sw);
                            }
                        }
                        Logger.Log($"Broj submissiona : {listSubmission.Count.ToString()}  ", sw);
                        i++;
                    }
                }
                catch (InvalidCastException e)
                {
                    Logger.Log($"Greska se javila zbog : {e.Message}  ", sw);
                }
            }
            return listSubmission;
        }

    }
}
