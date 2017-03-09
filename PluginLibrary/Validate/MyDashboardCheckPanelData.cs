using System;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.WebTesting;
using HtmlAgilityPack;
using PluginLibrary.Models;
using PluginLibrary.Helper;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace PluginLibrary.Validate
{
    public class MyDashboardCheckPanelData : ValidationRule
    {
        private string table_name;
        public string TableName
        {
            get { return table_name; }
            set { table_name = value; }
        }
        public override void Validate(object sender, ValidationEventArgs e)
        {
            // podaci o tekucem korisniku sistema
            string _userName;
            _userName = e.WebTest.Context["CurrentUser"].ToString();

            // html podatak dobijen extakcijom kada je obradjivana Global submission lista
            string _GLSHtmlDoc;
            _GLSHtmlDoc = e.WebTest.Context["GLSsubmissionsHTMLdoc"].ToString();

            TableName = "0 table";
            if (CompareTable(e.Response, table_name, _userName, _GLSHtmlDoc))
            {
                e.IsValid = true;
            }
            else
            {
                //this resource does not mention extraction in the text, so it’s fine to use here, too
                e.Message = String.Format("Did not find table: { 0}", TableName);
                e.IsValid = false;
            }
        }

        private bool CompareTable(WebTestResponse response, string _tablename, string _userName, string _GLSHtmlString)
        {
            bool isValid = false;
            
            // kreiranje HTML dokumenta za Global submission list
            HtmlAgilityPack.HtmlDocument GLSHtmlDoc = new HtmlAgilityPack.HtmlDocument();
            GLSHtmlDoc.LoadHtml(_GLSHtmlString);
            
            // obrada HTML dokumenta iz GLS i pretvaranje u listu submissiona
            List<Submission> GLSsubmissions = new List<Submission>();
            GLSsubmissions = GetGLSSubmissions.GetAllSubmissions(GLSHtmlDoc);
            
            // obrada MyDashboard HTML responsa
            string htmlDoc = response.BodyString;
            //using (StreamWriter sw = File.AppendText(@"C:\Temp\CompareTable.txt"))
            //    sw.WriteLine($"MYDASHBOARD submissions idu sa sledecim HTML stringom: {htmlDoc}");

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlDoc);
            switch (_tablename)
            {
                // My Submissions
                case "0 table":
                    //List<GlobalSubmissionListRow> MyDashboard_0_submissions = new List<GlobalSubmissionListRow>();
                    var MyDashboard_0_submissions = GetMyDashboardSubmissions.GetAllSubmissions(doc, _tablename);
                    // nepotrebne pretumbacije ali u ovom trenutku resenje
                    // naime kada uzimam submissione iz GLS , uzimam sa svim atributima, kada uzimam submissione sa MyDashboard stranice uzimam samo sa GUID i Title
                    // u narednim redovima to pretvaram u Liste s istim tipom podataka da bih  ih mogao uporediti
                    using (StreamWriter sw = File.AppendText(@"C:\Temp\POREDJENJE.txt"))
                    {
                        Logger.Log($"Lista submissiona na GLS ulazna : ", sw);
                        foreach (var item in GLSsubmissions)
                        {
                            Logger.Log($"{item.Title} - {item.SubmissionGUID} - [{item.CreatedBy}]", sw);

                        }

                        Logger.Log($"Lista submissiona na GLS preciscena za tekuceg usear: [{_userName}] ", sw);
                        var CurrentUserSubmissionsGLS = GLSsubmissions.FindAll(i => i.CreatedBy == _userName);
                        Logger.Log($"Lista submissiona na GLS preciscena za tekuceg usear: ", sw);
                        foreach (var item in CurrentUserSubmissionsGLS)
                        {
                            Logger.Log($"{item.Title} - {item.SubmissionGUID}", sw);
                        }

                        var GLSguid_title = CurrentUserSubmissionsGLS.AsEnumerable()
                          .Select(x =>
                             new { Title = x.Title, GUID = x.SubmissionGUID })
                          .ToList();
                        Logger.Log($"Lista submissiona na GLS preciscena za tekuceg usear - PREBACENA U LISTU: ", sw);
                        foreach (var item in GLSguid_title)
                        {
                            Logger.Log($"{item.Title} - {item.GUID}", sw);
                        }


                        var MyDashboard = MyDashboard_0_submissions.AsEnumerable()
                          .Select(x =>
                             new { Title = x.Title, GUID = x.SubmissionGUID })
                          .ToList();
                        Logger.Log($"Lista submissiona za tekuceg usera na MYDASHBOARD : ", sw);
                        foreach (var item in MyDashboard)
                        {
                            Logger.Log($"{item.Title} - {item.GUID}", sw);

                        }
                        var firstNotSecond = GLSguid_title.Except(MyDashboard).ToList();
                        var secondNotFirst = MyDashboard.Except(GLSguid_title).ToList();
                        if (firstNotSecond.Count == 0 && secondNotFirst.Count == 0)
                            isValid = true;
                    }
                
                    break;
                // Other submission I can approve
                case "1 table":
                    //var MyDashboard_1_submissions = GetSubmissions_HTML.GetAllSubmissions(doc, _tablename);
                    break;
                // Submission I can submit
                case "2 table":
                    //var MyDashboard_2_submissions = GetSubmissions_HTML.GetAllSubmissions(doc, _tablename);
                    break;
                // Submission I have submitted
                case "3 table":
                    //var MyDashboard_3_submissions = GetSubmissions_HTML.GetAllSubmissions(doc, _tablename);
                    break;
                default:
                    break;
            }


            return isValid;
        }

    }
}

