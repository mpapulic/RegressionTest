using Microsoft.VisualStudio.TestTools.WebTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using PluginLibrary.Helper;
using PluginLibrary.Models;
using System.IO;

namespace PluginLibrary
{
    [DisplayName("Extract a submission TITLE by status")]
    [Description("Grabs a submission Title from GLOBAL SUBMISSION LIST by Status.")]
    public class ExtractGetSubmissionTitleByStatus : ExtractionRule
    {
        public List<Submission> GLSsubmissions = new List<Submission>();

        public override void Extract(object sender, ExtractionEventArgs e)
        {
            using (StreamWriter sw = File.AppendText(@"C:\Temp\Extract.txt"))
            {
                try
                {
                    var doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(e.Response.BodyString);
                    string _status = e.WebTest.Context["SubmissionStatus"].ToString();
                    string _currentUser = e.WebTest.Context["CurrentUser"].ToString();
                    sw.WriteLine($"Ide u potragu za submissionom sa statusm {_status} ");
                    string submissionTitle = "";
                    if (_status == "Rejected  by CH")
                         submissionTitle = GetGLSSubmissions.GetGLSSubmissionTitleByStatusAndUser(doc, _status,_currentUser);
                    else
                        submissionTitle = GetGLSSubmissions.GetGLSSubmissionByStatus(doc, _status);
                    if (submissionTitle == "")
                        Fail(e, "Submission with requested status did not find !");
                    e.WebTest.Context.Add("SubmissionTitle", submissionTitle);
                }
                catch (Exception s)
                {
                    Fail(e, "Submission with requested status doesn't exist!");
                }
            }
        }
        private void GrabValue(ExtractionEventArgs e)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(e.Response.BodyString);
            //using (StreamWriter sw = File.AppendText(@"C:\Temp\Extract.txt"))
            //{
            //    sw.WriteLine($"GLS html  : {doc.ToString()}  ");
            //    sw.WriteLine($"GLS html iz bodystringa   : {e.Response.BodyString}  ");
            //}

            e.WebTest.Context.Add("GLSsubmissionsHTMLdoc", e.Response.BodyString);
        }

        private void Fail(ExtractionEventArgs e, string message)
        {
            e.Success = false;
            e.Message = message;
            e.WebTest.Context.Add(ContextParameterName, "(FAIL)");
        }
    }


}
