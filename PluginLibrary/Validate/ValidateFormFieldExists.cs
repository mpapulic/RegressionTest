using System;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.WebTesting;




namespace RuleExamples

{
    namespace PluginLibrary
    {
        public class ValidateFormFieldExists : ValidationRule
        {
            private string m_name;
            public string Name
            {
                get { return m_name; }
                set { m_name = value; }
            }
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

