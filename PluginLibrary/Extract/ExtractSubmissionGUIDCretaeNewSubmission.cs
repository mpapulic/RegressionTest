using Microsoft.VisualStudio.TestTools.WebTesting;
using System.IO;
using System.ComponentModel;
using PluginLibrary.Helper;

namespace PluginLibrary.Extract
{
    [DisplayName("Extract Submission GUID ")]
    [Description("Grabs submission GUID from response when NEW submission is created.")]
    public class ExtractSubmissionGUIDCretaeNewSubmission : ExtractionRule
    {

        [DisplayName("SubmissionGUID ")]
        [Description("The GUID of nre created submission:")]
        public string SubmissionGUID { get; set; }


        public override void Extract(object sender, ExtractionEventArgs e)
        {
            using (StreamWriter sw = File.AppendText(@"C:\Temp\CreateMr01.html"))
            {
                //sw.WriteLine($"<!response>");
                //sw.WriteLine($"{e.Response.BodyString}");
                string html = e.Response.BodyString;
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                //sw.WriteLine($"ide i body responsa   : {html}  ");
                doc.LoadHtml(html);
                sw.WriteLine($"KREIRAN HTMLDOC   : {doc.DocumentNode.OuterHtml.ToString()}  ");

                string guid = GetSubmissionGUIDCreateNew.GetSubmissionGUID(doc);
                e.WebTest.Context.Add("SubmissionGUID", guid);
            }
        }

        private void Fail(ExtractionEventArgs e, string message)
        {
            e.Success = false;
            e.Message = message;
            e.WebTest.Context.Add(ContextParameterName, "(FAIL)");
        }
    }
}
