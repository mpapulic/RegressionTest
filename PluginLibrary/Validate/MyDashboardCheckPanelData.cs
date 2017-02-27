using System;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.WebTesting;
using HtmlAgilityPack;
using PluginLibrary.Models;
using PluginLibrary.Helper;
using System.Collections.Generic;

namespace PluginLibrary.Validate
    {
        public class MyDashboardCheckPanelData : ValidationRule
        {
            private string m_name;
            public string Name
            {
                get { return m_name; }
                set { m_name = value; }
            }
        public List<GlobalSubmissionListRow> GLSsubmissions = new List<GlobalSubmissionListRow>();
        private List<GlobalSubmissionListRow> MyDashboard_0_submissions = new List<GlobalSubmissionListRow>();
        private List<GlobalSubmissionListRow> MyDashboard_1_submissions = new List<GlobalSubmissionListRow>();
        private List<GlobalSubmissionListRow> MyDashboard_2_submissions = new List<GlobalSubmissionListRow>();
        private List<GlobalSubmissionListRow> MyDashboard_3_submissions = new List<GlobalSubmissionListRow>();
        public override void Validate(object sender, ValidationEventArgs e)
            {

                if (DoesFormFieldExist(e.Response, m_name))
                {
                    e.IsValid = true;
                }
                else
                {
                    //this resource does not mention extraction in the text, so it’s fine to use here, too
                    e.Message = String.Format("Did not find form Field with name { 0}", Name);
                    e.IsValid = false;
                }
            }

            internal static bool DoesFormFieldExist(WebTestResponse response, string formFieldName)
            {
                string htmlDoc = response.HtmlDocument.ToString();
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(htmlDoc);
                GLSsubmissions = GetSubmissions_HTML.GetAllSubmissions(doc, "");

                foreach (HtmlTag tag in response.HtmlDocument.GetFilteredHtmlTags("input"))
                {
                    if (String.Equals(tag.GetAttributeValueAsString("name"), formFieldName, StringComparison.OrdinalIgnoreCase)
                                || String.Equals(tag.GetAttributeValueAsString("id"), formFieldName, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                    return false;
                }

        }

    }
}

