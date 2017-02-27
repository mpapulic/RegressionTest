using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.WebTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PluginLibrary.Helper;
using PluginLibrary.Models;

namespace PluginLibrary
{
    [DisplayName("Extract all submissions from GLOBAL SUBMISSION LIST")]
    [Description("Grabs a ALL values from the  GLOBAL SUBMISSION LIST.")]
    public class SubmissionsExtractGlobalSubmissionListPlugin : ExtractionRule
    {
        public List<GlobalSubmissionListRow> GLSsubmissions = new List<GlobalSubmissionListRow>();

        public override void Extract(object sender, ExtractionEventArgs e)
        {

            //if (String.IsNullOrWhiteSpace(SubmissionTitle) == true)
            //{
            //    Fail(e, "Submission title  does not have a valid value.");
            //}
            //else
            //{
            //    //e.WebTest.Context.Add("NewTeam", "234");
                GrabValue(e);
            //}
        }
        private void GrabValue(ExtractionEventArgs e)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(e.Response.BodyString);
            string tableID = "0 table";
            GLSsubmissions = GetSubmissions_HTML.GetAllSubmissions(doc, tableID);
            //SubmissionGUID = GetSubmissions_HTML.GetSubmissionByTitle(doc, SubmissionTitle);
            //System.IO.File.WriteAllText(@"C:\Temp\TestPARAMETRI.txt", $"Team name: {TeamName}  team ID : {TeamID} .");
            //e.WebTest.Context.Add(ContextParameterName, "Pretraga se vrsi za:" + TeamName);
            //e.WebTest.Context.Add(ContextParameterName, "Odabrani team WEBTEST 002 ima ID:" + TeamID);

            e.WebTest.Context.Add("GLSsubmissions", GLSsubmissions);
            //GrabValue(e, table);
        }

        private void Fail(ExtractionEventArgs e, string message)
        {
            e.Success = false;
            e.Message = message;
            e.WebTest.Context.Add(ContextParameterName, "(FAIL)");
        }
    }


}
