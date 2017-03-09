﻿using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.IO;
using System.Linq;
using PluginLibrary.Models;



namespace PluginLibrary.Helper
{
    class GetGLSSubmissions
    {
        public static string GetGLSSubmissionByTitle(HtmlAgilityPack.HtmlDocument doc, string _title)
        {

            string _submissionID = "";
            List<Submission> listSubmissions = new List<Submission>();


            //listSubmissions = GetAllSubmissions(doc, "");

            int index = listSubmissions.FindIndex(x => x.Title.Contains(_title));
            if (index != -1)
                _submissionID = listSubmissions[index].SubmissionGUID;
            return _submissionID;
        }
        public static string GetGLSSubmissionTitleByStatusAndUser(HtmlAgilityPack.HtmlDocument doc, string _status, string _user)
        {
            string _submissionTitle = "";
            List<Submission> listSubmissions = new List<Submission>();
            using (StreamWriter sw = File.AppendText(@"C:\Temp\Lista submission GLS.txt"))
            {
                listSubmissions = GetAllSubmissions(doc);
                foreach(var item in listSubmissions)
                {
                    sw.WriteLine($" {item.Title} - {item.Status} - {item.CreatedBy}");
                }
                int index = listSubmissions.FindIndex(x => x.Status.Contains(_status) && x.CreatedBy.Contains(_user));
                if (index != -1)
                    _submissionTitle = listSubmissions[index].Title;
                sw.WriteLine($"Vracam title  : {_submissionTitle} ");
            }
            return _submissionTitle;
        }
        public static string GetGLSSubmissionByStatus(HtmlAgilityPack.HtmlDocument doc, string _status)
        {

            string _submissionTitle = "";
            List<Submission> listSubmissions = new List<Submission>();

            listSubmissions = GetAllSubmissions(doc);
            using (StreamWriter sw = File.AppendText(@"C:\Temp\GetGLSSubmissionByStatus.txt"))
            {
                sw.WriteLine($"Pronadjenih slogova sa statusm {_status} : {listSubmissions.Count} ");
                int index = listSubmissions.FindIndex(x => x.Status.Contains(_status));
                // takes the first submission Title
                if (index != -1)
                  _submissionTitle = listSubmissions[index].Title;
                sw.WriteLine($"Vracam title  : {_submissionTitle} ");
            }
            return _submissionTitle;
        }

        public static List<Submission> GetAllSubmissions(HtmlAgilityPack.HtmlDocument doc)
        {
            // list of the Submissions on the page
            List<Submission> listSubmission = new List<Submission>();
            HtmlNode TabeleUhtml;
            string uslov;

            using (StreamWriter sw = File.AppendText(@"C:\Temp\GLSSubmissions.txt"))
            {
                // searching for all tables in the HTML document
                uslov = "//table";
                try
                {
                    TabeleUhtml = doc.DocumentNode.SelectSingleNode(uslov);
                    if (TabeleUhtml != null)
                    {
                        // podesavam se na zeljenu poziciju i nakon toga unutar tog noda uzimam sve sledece
                        HtmlNode PrviReduTabeli = doc.DocumentNode.SelectNodes(uslov).First();
                        HtmlNode[] redovi = PrviReduTabeli.SelectNodes($".//tr").ToArray();
                        int brojRedova = redovi.Count();
                        int rbrRed = 1;

                        foreach (HtmlNode ROW in redovi)
                        {
                            if (ROW.SelectSingleNode(".//td") != null)
                            {
                                //HtmlNode PrviReduTabeliPrvaKolonauRedu = ROW.SelectNodes($".//td").First();
                                HtmlNode[] kolone = ROW.SelectNodes($".//td").ToArray();
                                //HtmlNode[] kolone = doc.DocumentNode.SelectNodes("//a").Where(x => x.InnerHtml.Contains("div2")).ToArray();
                                int brojKolona = kolone.Count();
                                string[] fields = new string[brojKolona + 2];
                                int rbrKolona = 0;
                                foreach (HtmlNode CELL in kolone)
                                {
                                    switch (rbrKolona)
                                    {

                                        // prvo polje sadrzi title
                                        // atribut href sadrzi podatka o SubmissionGUID i mora se izvuci
                                        case 0:
                                            fields[0] = CELL.InnerText.Trim('\n', '\r', ' ');
                                            string b = CELL.OuterHtml.Trim('\n', '\r', ' ');
                                            if (b.Contains("Details"))
                                                fields[brojKolona] = b.Substring(b.IndexOf("Details") + 8, 36);
                                            else
                                            if (b.Contains("Edit"))
                                                fields[brojKolona] = b.Substring(b.IndexOf("Edit") + 5, 36);
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
                                                fields[rbrKolona] = CELL.InnerText.Trim('\n', '\r', ' ');
                                            }
                                            break;
                                        // polje 4 sadrzi podataka o charge creation date i ujedno da li je urgent ili ne 
                                        case 4:
                                            fields[4] = CELL.InnerText.Trim('\n', '\r', ' ');
                                            fields[brojKolona + 1] = CELL.GetAttributeValue("class", string.Empty);
                                            break;
                                        default:
                                            break;
                                    }

                                    rbrKolona++;
                                }


                                listSubmission.Add(new Submission
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
                                sw.WriteLine($"{rbrRed}. Title: {fields[0]} - GUID:{fields[brojKolona]} - Status: {fields[7]}");

                                rbrRed++;
                            }
                        }
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