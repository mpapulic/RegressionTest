using System;
using Microsoft.VisualStudio.TestTools.WebTesting;
using PluginLibrary.Helper;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PluginLibrary.Validate
{
    public class ValidatePDF_LLMR04 : ValidationRule
    {
        private string submissionType;
        public string SubmissionType
        {
            get { return submissionType; }
            set { submissionType = value; }
        }
        public override void Validate(object sender, ValidationEventArgs e)
        {
            // podaci o tekucem korisniku sistema
            string pdfFileName;
            string downloadsPath = KnownFolders.GetPath(KnownFolder.Downloads);
            byte[] textBytes = e.Response.BodyBytes;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, textBytes);
            pdfFileName = downloadsPath + @"\" + e.WebTest.Context["SubmissionTitle"].ToString() + ".pdf";
            System.IO.File.WriteAllBytes(pdfFileName, textBytes);
            string pdfText = GetPDFText.PDFText(pdfFileName);
            switch (SubmissionType)
            {
                case "LLMR04":
                    // PDF podatak dobijen extakcijom iz PDF dokumenta
                    GetValuesFromPDF_LLMR04 llMR04doc = new GetValuesFromPDF_LLMR04(pdfText);
                    if (e.WebTest.Context["Company Number"].ToString() != llMR04doc.CompanyNumber)
                    {
                        e.IsValid = false;
                        e.Message = String.Format($"Company number : { e.WebTest.Context["Company Number"].ToString()} is not matched with PDF : {llMR04doc.CompanyNumber}");
                        //e.IsValid = false;
                        return;
                    }
                    if (e.WebTest.Context["PersonForename"].ToString() != llMR04doc.PersonName)
                    {
                        e.IsValid = false;
                        e.Message = String.Format($"Name on document: : { e.WebTest.Context["PersonForename"].ToString()} is not matched with PDF : {llMR04doc.PersonName}");
                        //e.IsValid = false;
                        return;
                    }
                    if (e.WebTest.Context["BuildingNameOrNumber"].ToString() != llMR04doc.PersonBuilding)
                    {
                        e.IsValid = false;
                        e.Message = String.Format($"Building on document: : { e.WebTest.Context["BuildingNameOrNumber"].ToString()} is not matched with PDF : {llMR04doc.PersonBuilding}");
                        //e.IsValid = false;
                        return;
                    }
                    if (e.WebTest.Context["PersonPostTown"].ToString() != llMR04doc.PersonPostTown)
                    {
                        e.IsValid = false;
                        e.Message = String.Format($"Building on document: : { e.WebTest.Context["PersonPostTown"].ToString()} is not matched with PDF : {llMR04doc.PersonPostTown}");
                        //e.IsValid = false;
                        return;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}

