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
using System.IO;

namespace PluginLibrary
{
    [DisplayName("Extract all submissions from GLOBAL SUBMISSION LIST")]
    [Description("Grabs a ALL values from the  GLOBAL SUBMISSION LIST.")]
    public class SubmissionsExtractGlobalSubmissionListPlugin : ExtractionRule
    {
        public List<Submission> GLSsubmissions = new List<Submission>();

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
