using Microsoft.VisualStudio.TestTools.WebTesting;
using System.ComponentModel;
using PluginLibrary.Helper;


namespace PluginLibrary.Validate
{
    public class ValidateUrgentType : ValidationRule
    {
        private string submissionType;
        [DisplayName("Type of urgent period")]
        [Description("Definition of urgent type, could be or URGENT or VERYURGENT period")]
        public string SubmissionType
        {
            get { return submissionType; }
            set { submissionType = value; }
        }
        public override void Validate(object sender, ValidationEventArgs e)
        {
            Logger.Log($"Trazeni tip submissiona: {SubmissionType}");
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(e.Response.BodyString);
            string typeOfSubmission = GetGLSSubmissions.GetGLSSubmissionUrgentStatusByGUID(doc, e.WebTest.Context["SubmissionGUID"].ToString());
            Logger.Log($"Tip submissiona: {typeOfSubmission}");
            // check is test for URGENT submission
            if (submissionType.ToUpper() == "URGENT")
            {
                if (typeOfSubmission == "urgent")
                    e.IsValid = true;
                else
                    e.IsValid = false;
            }
            else
            {
                if (submissionType.ToUpper() == "VERYURGENT")
                {
                    if (typeOfSubmission == "ultraUrgent")
                        e.IsValid = true;
                    else
                        e.IsValid = false;
                }
                else
                    e.IsValid = false;
            }

        }
    }
}
