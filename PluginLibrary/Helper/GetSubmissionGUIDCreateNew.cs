using HtmlAgilityPack;

namespace PluginLibrary.Helper
{
    public static class GetSubmissionGUIDCreateNew 
    {
        public static string GetSubmissionGUID(HtmlDocument doc)
        {
            string guid = "";
            HtmlNode inputNode = doc.DocumentNode.SelectSingleNode(@"//input  [@id='Id']");
            if (inputNode != null)
            {
                guid = inputNode.GetAttributeValue("value", "Nije pronadjen").ToString();
            }
            return guid;
        }
    }

}
