using Microsoft.VisualStudio.TestTools.WebTesting;
using System.ComponentModel;
using PluginLibrary.Helper;

namespace PluginLibrary
{
    [DisplayName("Urgent submission")]
    [Description("Check does submission is urgent.")]
    public class CheckUrgentPlugIn : ExtractionRule
    {
        [DisplayName("Urgent type, TRUE - very urgent , FALSE - urgent")]
        [Description("Very urgent submission:")]
        public string IsUrgentSubmission { get; set; }
        public override void Extract(object sender, ExtractionEventArgs e)
        {

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(e.Response.BodyString);
            string typeOfSubmission = CheckUrgent.UrgentSubmission(doc, e.WebTest.Context["submissionGUID"].ToString());
            
            if (
                ((e.WebTest.Context["IsUrgentSubmission"].ToString().ToUpper() == "FALSE" && typeOfSubmission == "URGENT") ||
                (e.WebTest.Context["IsUrgentSubmission"].ToString().ToUpper() == "TRUE" && typeOfSubmission == "VERYURGENT"))
                )
                e.WebTest.Context.Add("IsUrgentCheck", "PASS");
            else
                e.WebTest.Context.Add("IsUrgentCheck", "FAILED");
            System.IO.File.WriteAllText(@"C:\Temp\PreRequest.txt", $"Usao u plugin URGENT i ovo je rezultat za submission: {typeOfSubmission} .");
        }
    }
}
