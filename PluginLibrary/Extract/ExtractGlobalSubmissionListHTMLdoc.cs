using Microsoft.VisualStudio.TestTools.WebTesting;
using System.Collections.Generic;
using System.ComponentModel;
using PluginLibrary.Models;

namespace PluginLibrary
{
    [DisplayName("Extract GLOBAL SUBMISSION LIST html document")]
    [Description("Grabs a GLOBAL SUBMISSION LIST html document for the lter processing.")]
    public class ExtractGlobalSubmissionListHTMLdoc : ExtractionRule
    {
        public List<Submission> GLSsubmissions = new List<Submission>();

        public override void Extract(object sender, ExtractionEventArgs e)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(e.Response.BodyString);
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
