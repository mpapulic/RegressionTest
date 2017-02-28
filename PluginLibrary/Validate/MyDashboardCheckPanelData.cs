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
        private List<GlobalSubmissionListRow> GLSsubmissions = new List<GlobalSubmissionListRow>();
        private List<GlobalSubmissionListRow> MyDashboard_0_submissions = new List<GlobalSubmissionListRow>();
        private List<GlobalSubmissionListRow> MyDashboard_1_submissions = new List<GlobalSubmissionListRow>();
        private List<GlobalSubmissionListRow> MyDashboard_2_submissions = new List<GlobalSubmissionListRow>();
        private List<GlobalSubmissionListRow> MyDashboard_3_submissions = new List<GlobalSubmissionListRow>();
        private string _userName;

        public override void Validate(object sender, ValidationEventArgs e)
        {
            _userName = e.WebTest.Context["FirstName"] + " " + e.WebTest.Context["LastName"];
            //using (StreamWriter sw = File.AppendText(@"C:\Temp\VALIDATE.txt"))
            //{
            //    sw.WriteLine($" VALIDATE TEKUCEG USERA:{_userName} ");
            //}
            if (CompareTable(e.Response, table_name, _userName))
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

        private bool CompareTable(WebTestResponse response, string _tablename, string _userName)
        {
            bool isValid = false;
            string htmlDoc = response.HtmlDocument.ToString();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlDoc);
            using (StreamWriter sw = File.AppendText(@"C:\Temp\PRE_GLOBALSUBMISSIONLIST.txt"))
            {
                sw.WriteLine($" GLOBAL SUBMISSION LIST {htmlDoc} ");
            }
            GLSsubmissions = GetSubmissions_HTML.GetAllSubmissions(doc, "");
            using (StreamWriter sw = File.AppendText(@"C:\Temp\POSLE_GLOBALSUBMISSIONLIST.txt"))
            {
                sw.WriteLine($" GLOBAL SUBMISSION LIST  /r/n {GLSsubmissions.Count.ToString()} ");
            }
            switch (_tablename)
            {
                // My Submissions
                case "0 table":
                    MyDashboard_0_submissions = GetSubmissions_HTML.GetAllSubmissions(doc, _tablename);
                    //using (StreamWriter sw = File.AppendText(@"C:\Temp\MYDASHBOARD.txt"))
                    //{
                    //    sw.WriteLine($" MYDASHBOARD tekuceg usera :{_userName} /r/n {MyDashboard_0_submissions.ToString()} ");
                    //}
                    // provera da li su svi submissioni iz GLS za tekuceg usera u tabeli MyDashboard
                    var CurrentUserSubmissionsGLS = GLSsubmissions.FindAll(i => i.CreatedBy == _userName);
                    //using (StreamWriter sw = File.AppendText(@"C:\Temp\Tekuci user.txt"))
                    //{
                    //    sw.WriteLine($" FILTER submissiona na tekuceg usera :{_userName} /r/n {CurrentUserSubmissionsGLS.ToString()} ");
                    //}
                    var firstNotSecond = CurrentUserSubmissionsGLS.Except(MyDashboard_0_submissions).ToList();
                    var secondNotFirst = MyDashboard_0_submissions.Except(CurrentUserSubmissionsGLS).ToList();
                    if (firstNotSecond.Count == 0 && secondNotFirst.Count == 0)
                        isValid = true;
                
                    break;
                // Other submission I can approve
                case "1 table":
                    MyDashboard_0_submissions = GetSubmissions_HTML.GetAllSubmissions(doc, _tablename);
                    break;
                // Submission I can submit
                case "2 table":
                    MyDashboard_0_submissions = GetSubmissions_HTML.GetAllSubmissions(doc, _tablename);
                    break;
                // Submission I have submitted
                case "3 table":
                    MyDashboard_0_submissions = GetSubmissions_HTML.GetAllSubmissions(doc, _tablename);
                    break;
                default:
                    break;
            }


            return isValid;
        }

    }
}

