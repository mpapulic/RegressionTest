using System;
using Microsoft.VisualStudio.TestTools.WebTesting;
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
        // _table name - parametar u slucaju da na MyDashboard stranici imaju cetiri panela ( '0 table', '1 table','2 table','3 table'
        // _username - puno ime i prezime tekuceg korisnika sistema
        // _GLSHtmlString - Global Submission List response HTML petvoren u string, koji je zapamcen u Context parametru u nekom od prethodnih web request-a
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
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlDoc);
            switch (_tablename)
            {
                // My Submissions panel kada je u Preferences podeseno da je Optional
                case "0 table":
                    // dobija se lista submissiona za trazeni panel iz HTML responsa za MyDashboard stranicu
                    var MyDashboard_0_submissions = GetMyDashboardSubmissions.GetAllSubmissions(doc, _tablename);
                    // dobija se lista submissiona za tekuceg korisnika iz Global Submission liste
                    var CurrentUserSubmissionsGLS = GLSsubmissions.FindAll(i => i.CreatedBy == _userName);
                    // radi poredjenja GLS lista se pretvara samo u listu koja ima Title i GUID
                    var GLSguid_title = CurrentUserSubmissionsGLS.AsEnumerable()
                      .Select(x =>
                         new { Title = x.Title, GUID = x.SubmissionGUID })
                      .ToList();
                    var MyDashboard = MyDashboard_0_submissions.AsEnumerable()
                      .Select(x =>
                         new { Title = x.Title, GUID = x.SubmissionGUID })
                      .ToList();
                    // provera da li postoje razlike izmedju ove dve liste
                    var firstNotSecond = GLSguid_title.Except(MyDashboard).ToList();
                    var secondNotFirst = MyDashboard.Except(GLSguid_title).ToList();
                    // ako ne postoje razlike onda je sve OK
                    if (firstNotSecond.Count == 0 && secondNotFirst.Count == 0)
                        isValid = true;

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

