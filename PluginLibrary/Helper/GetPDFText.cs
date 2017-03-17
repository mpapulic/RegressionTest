using Bytescout.PDFExtractor;
using System.IO;

namespace PluginLibrary.Helper
{
    public static class GetPDFText
    {
        public static string PDFText(string fileName)
        {
            string text = "";
            // PDF fajl se sprema u Downloads folder, i putanja je:
            string downloadsPath = KnownFolders.GetPath(KnownFolder.Downloads);
            // preuzeta vrednost texta iz PDf fajla ce se smestiti u output.txt fajl u isti folder
            string outputfileName = downloadsPath+  @"\output.txt";
            // vrsi se extrakcija teksta iz PDF fajla
            TextExtractor extractor = new TextExtractor();
            extractor.RegistrationName = "demo";
            extractor.RegistrationKey = "demo";

            // Load sample PDF document
            extractor.LoadDocumentFromFile(fileName);

            // Save extracted text to file
            extractor.SaveTextToFile(outputfileName);

            text = System.IO.File.ReadAllText(outputfileName);
            return text;
        }
    }
}
