using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace PluginLibrary.Helper
{
    public static class GetSubmissionGUIDCreateNew 
    {
        public static string GetSubmissionGUID(HtmlDocument doc)
        {
            using (StreamWriter sw = File.AppendText(@"C:\Temp\GetSubmissionGUID.txt"))
            {
                string guid = "";
                //sw.WriteLine($"HTML  : {htmlFile}  ");
                //HtmlDocument doc = new HtmlDocument();
                //sw.WriteLine($"Sada ide pretvaranje u HTML nodes");

                //doc.Load(htmlFile);
                //sw.WriteLine($"Posle pretvaranja u HTML nodes");
                sw.WriteLine($"KREIRAN HTMLDOC   : {doc.DocumentNode.OuterHtml.ToString()}  ");
                HtmlNode inputNode = doc.DocumentNode.SelectSingleNode(@"//input  [@id='Id']");
                sw.WriteLine($"Odabrani nodovi za pretragu!");
                if (inputNode != null)
                {
                    sw.WriteLine($"guid  : {guid}  ");

                    guid = inputNode.GetAttributeValue("value", "Nije pronadjen").ToString();
                }
                return guid;

            }
        }
    }

}
