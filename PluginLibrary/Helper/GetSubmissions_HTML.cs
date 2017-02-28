using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginLibrary.Models;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace PluginLibrary.Helper
{
    public static class GetSubmissions_HTML
    {
        public static string GetSubmissionByTitle(HtmlAgilityPack.HtmlDocument doc, string _title)
        {

            string _submissionID = "";
            List<GlobalSubmissionListRow> listSubmissions = new List<GlobalSubmissionListRow>();


            listSubmissions = GetAllSubmissions(doc, "");

            int index = listSubmissions.FindIndex(x => x.Title.Contains(_title));
            if (index != -1)
                _submissionID = listSubmissions[index].SubmissionGUID;
            return _submissionID;
        }
        public static List<GlobalSubmissionListRow> GetAllSubmissions(HtmlAgilityPack.HtmlDocument doc, string _tableID)
        {
            // list of the TEAMs on the page
            List<GlobalSubmissionListRow> list = new List<GlobalSubmissionListRow>();

            // searching for all tables in the HTM document
            int k = 0;
            HtmlNodeCollection nodesTable = doc.DocumentNode.SelectNodes("//table");

            // passing through all tables in the HTML document
            int iTablesCount = nodesTable.Count;
            if (iTablesCount == 0)
            {
                return list;
            }
            foreach (var TABLE in nodesTable)
            {
                // petrazuje se samo trazena tabela na celoj HTML stranici, ili sve ako je _tableID prazan string
                using (StreamWriter sw = File.CreateText(@"C:\Temp\GLS-table.txt"))
                {
                    sw.WriteLine(TABLE.OuterHtml);
                    sw.WriteLine("");
                    sw.WriteLine($"Tabela koja se obradjuje je : {_tableID } ");
                }
                if (TABLE.Id.ToString() == _tableID || _tableID == "")
               {

                    HtmlNodeCollection nodesRows = TABLE.SelectNodes($"//table[{k}]//tr");
                    if (nodesRows != null)
                    {
                        int j = 1;
                        foreach (var ROW in nodesRows)
                        {
                            HtmlNodeCollection nodesCells = ROW.SelectNodes($"//tr[{j}]//td");
                            if (nodesCells != null)
                            {
                                // row which contains data
                                int iCellsCount = nodesCells.Count;
                                int i = 0;
                                // preuzimanje svih elemenata iz sloga da bui se na kraju dodali svi u listu, 
                                // i dodatna dva  polja za submissionGUID i da li je urgent ili ne
                                string[] fields = new string[iCellsCount + 2];
                                foreach (var CELL in nodesCells)
                                {
                                    switch (i)
                                    {
                                        // prvo polje sadrzi title
                                        // atribut href sadrzi podatka o SubmissionGUID i mora se izvuci
                                        case 0:
                                            fields[0] = CELL.InnerText.Trim('\n', '\r', ' ');
                                            string b = CELL.OuterHtml.Trim('\n', '\r', ' ');
                                            if (b.Contains("Details"))
                                                fields[iCellsCount] = b.Substring(b.IndexOf("Details") + 8, 36);
                                            else
                                            if (b.Contains("Edit"))
                                                fields[iCellsCount] = b.Substring(b.IndexOf("Edit") + 5, 36);
                                            break;
                                        // sledeca polja sadrze razne podatke
                                        case 1:
                                        case 2:
                                        case 3:
                                        case 5:
                                        case 6:
                                        case 7:
                                        case 8:
                                            {
                                                fields[i] = CELL.InnerText.Trim('\n', '\r', ' ');
                                            }
                                            break;
                                        // polje 4 sadrzi podataka o charge creation date i ujedno da li je urgent ili ne 
                                        case 4:
                                            fields[4] = CELL.InnerText.Trim('\n', '\r', ' ');
                                            fields[iCellsCount + 1] = CELL.GetAttributeValue("class", string.Empty);
                                            break;
                                        default:
                                            break;
                                    }
                                    i++;
                                }
                                list.Add(new GlobalSubmissionListRow
                                {
                                    Title = fields[0],
                                    Type = fields[1],
                                    CreatedBy = fields[2],
                                    Created = fields[3],
                                    Chargedate = fields[4],
                                    Nonusable = fields[5],
                                    Submitted = fields[6],
                                    Status = fields[7],
                                    Comment = fields[8],
                                    SubmissionGUID = fields[9],
                                    UrgentType = fields[10]

                                });
                                // LOS DEBUG
                                //using (StreamWriter sw = File.AppendText(@"C:\Temp\TestTeam.txt"))
                                //{
                                //    sw.WriteLine($"{j}/{nodesRows.Count} =>  Title:{ fields[0] }  GUID:{ fields[9]}  status: { fields[10]}");
                                //}
                                // END LOS DEBUG
                            }
                            else
                            {
                                // headers of the columns
                                //using (StreamWriter sw = File.AppendText(@"C:\Temp\TestGLSheader.txt"))
                                //{
                                //    sw.WriteLine($"{j}/{nodesRows.Count} =>  ovaj se nije obradio jer je nodesCell == null ");
                                //}
                                HtmlNodeCollection nodesHeaders = ROW.SelectNodes($"//tr[{j}]//th");
                                if (nodesHeaders != null)
                                {
                                    int h = 0;
                                    int iCellsCount = nodesHeaders.Count;
                                    foreach (var HEADER in nodesHeaders)
                                    {
                                        string a = HEADER.InnerText.Trim('\n', '\r', ' ');
                                        h++;
                                    }
                                }
                            }
                            j++;
                        }
                    }
                }
                k++;
                // Searching all rows in the table [k]
            }

            return list;
        }

    }
}
