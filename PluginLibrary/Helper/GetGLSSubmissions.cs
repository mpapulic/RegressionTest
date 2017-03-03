using System;
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
        public static List<Submission> GetAllSubmissions(HtmlAgilityPack.HtmlDocument doc)
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


            using (StreamWriter sw = File.AppendText(@"C:\Temp\GLSGetSubmissions.txt"))
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
                                Console.WriteLine($"{rbrRed}. Title: {fields[0]} - GUID:{fields[brojKolona]} - Creator: {fields[2]}");


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
