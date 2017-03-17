using System;

namespace PluginLibrary.Helper
{
    public  class GetValuesFromPDF_LLMR04
    {
        #region PROPERTY
        string pdfText { get; set; }

        private string companynumber;

        public string CompanyNumber
        {
            get { return companynumber; }
            set { companynumber = value; }
        }

        private string personname;

        public string PersonName
        {
            get { return personname; }
            set { personname = value; }
        }

        private string personstreet;

        public string PersonStreet
        {
            get { return personstreet; }
            set { personstreet = value; }
        }
        private string personposttown;
        public string PersonPostTown
        {
            get { return personposttown; }
            set { personposttown = value; }
        }
        private string personbuilding;
        public string PersonBuilding
        {
            get { return personbuilding; }
            set { personbuilding = value; }
        }
        #endregion PROPERTY

        #region CONSTRUCTOR
        public GetValuesFromPDF_LLMR04(string PDFtext)
        {
            pdfText = PDFtext;
            int start = pdfText.IndexOf("LLP number");
            int end = pdfText.IndexOf("Filling in this form");
            companynumber = pdfText.Substring(start + 14, end - start - 14).Replace(" ", String.Empty);

            // Person name
            start = pdfText.IndexOf("Name");
            end = pdfText.IndexOf("Please give the address");
            personname = pdfText.Substring(start + 4, end - start - 4).TrimEnd().TrimStart();

            // Person street
            start = pdfText.IndexOf("Street");
            end = pdfText.IndexOf("Post town");
            personstreet = pdfText.Substring(start + 6, end - start - 6).TrimEnd().TrimStart();

            // Person post town
            start = pdfText.IndexOf("Post town");
            end = pdfText.IndexOf("County/Region");
            personposttown = pdfText.Substring(start + 9, end - start - 9).TrimEnd().TrimStart();
            
            // Person building
            start = pdfText.IndexOf("Building name/number");
            end = pdfText.IndexOf("Street");
            personbuilding = pdfText.Substring(start + 20, end - start - 20).TrimEnd().TrimStart();
        }
        #endregion CONSTRUCTOR

    }
}
