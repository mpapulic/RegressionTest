﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegressionTest
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.WebTesting;
    using Microsoft.VisualStudio.TestTools.WebTesting.Rules;
    using PluginLibrary;


    public class WebTest1Coded : WebTest
    {

        public WebTest1Coded()
        {
            this.Context.Add("WebServer1", "http://oyezgateway.test.gowi.rs");
            this.Context.Add("User", "");
            this.PreAuthenticate = true;
            this.Proxy = "default";
        }

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            // Initialize validation rules that apply to all requests in the WebTest
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.Low))
            {
                ValidationRuleResponseTimeGoal validationRule1 = new ValidationRuleResponseTimeGoal();
                validationRule1.Tolerance = 0D;
                this.ValidateResponseOnPageComplete += new EventHandler<ValidationEventArgs>(validationRule1.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.Low))
            {
                ValidateResponseUrl validationRule2 = new ValidateResponseUrl();
                this.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule2.Validate);
            }

            WebTestRequest request1 = new WebTestRequest((this.Context["WebServer1"].ToString() + "/"));
            request1.ExpectedResponseUrl = (this.Context["WebServer1"].ToString() + "/Account/Login");
            ExtractHiddenFields extractionRule1 = new ExtractHiddenFields();
            extractionRule1.Required = true;
            extractionRule1.HtmlDecode = true;
            extractionRule1.ContextParameterName = "1";
            request1.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule1.Extract);
            yield return request1;
            request1 = null;

            WebTestRequest request2 = new WebTestRequest((this.Context["WebServer1"].ToString() + "/Account/Login"));
            request2.Method = "POST";
            request2.ExpectedResponseUrl = (this.Context["WebServer1"].ToString() + "/Admin/Report");
            request2.Headers.Add(new WebTestRequestHeader("Referer", (this.Context["WebServer1"].ToString() + "/Account/Login")));
            FormPostHttpBody request2Body = new FormPostHttpBody();
            request2Body.FormPostParameters.Add("ReturnUrl", this.Context["$HIDDEN1.ReturnUrl"].ToString());
            request2Body.FormPostParameters.Add("UserName", "admin.milan.papulic@gowi.rs");
            request2Body.FormPostParameters.Add("Password", "password");
            request2.Body = request2Body;
            yield return request2;
            request2 = null;

            WebTestRequest request3 = new WebTestRequest("http://oyezgateway.test.gowi.rs/Admin/User/EditDisabled");
            request3.Headers.Add(new WebTestRequestHeader("Referer", "http://oyezgateway.test.gowi.rs/Admin/Report"));
            ExtractAttributeValue extractionRule2 = new ExtractAttributeValue();
            extractionRule2.TagName = "input";
            extractionRule2.AttributeName = "value";
            extractionRule2.MatchAttributeName = "id";
            extractionRule2.MatchAttributeValue = "FirstName";
            extractionRule2.HtmlDecode = true;
            extractionRule2.Required = true;
            extractionRule2.Index = 0;
            extractionRule2.ContextParameterName = "FirstName";
            request3.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule2.Extract);
            ExtractAttributeValue extractionRule3 = new ExtractAttributeValue();
            extractionRule3.TagName = "input";
            extractionRule3.AttributeName = "value";
            extractionRule3.MatchAttributeName = "id";
            extractionRule3.MatchAttributeValue = "LastName";
            extractionRule3.HtmlDecode = true;
            extractionRule3.Required = true;
            extractionRule3.Index = 0;
            extractionRule3.ContextParameterName = "LastName";
            request3.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule3.Extract);
            yield return request3;
            request3 = null;

            WebTestRequest request4 = new WebTestRequest((this.Context["WebServer1"].ToString() + "/Admin/Admin/Preferences"));
            request4.Headers.Add(new WebTestRequestHeader("Referer", (this.Context["WebServer1"].ToString() + "/Admin/Report")));
            ExtractHiddenFields extractionRule4 = new ExtractHiddenFields();
            extractionRule4.Required = true;
            extractionRule4.HtmlDecode = true;
            extractionRule4.ContextParameterName = "1";
            request4.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule4.Extract);
            yield return request4;
            request4 = null;

            WebTestRequest request5 = new WebTestRequest((this.Context["WebServer1"].ToString() + "/Admin/Admin/Preferences"));
            request5.Method = "POST";
            request5.Headers.Add(new WebTestRequestHeader("Referer", (this.Context["WebServer1"].ToString() + "/Admin/Admin/Preferences")));
            FormPostHttpBody request5Body = new FormPostHttpBody();
            request5Body.FormPostParameters.Add("Id", this.Context["$HIDDEN1.Id"].ToString());
            request5Body.FormPostParameters.Add("UseTestEnvironment", this.Context["$HIDDEN1.UseTestEnvironment"].ToString());
            request5Body.FormPostParameters.Add("UserAttempCountLimit", this.Context["$HIDDEN1.UserAttempCountLimit"].ToString());
            request5Body.FormPostParameters.Add("UserPasswordHistoryLimit", this.Context["$HIDDEN1.UserPasswordHistoryLimit"].ToString());
            request5Body.FormPostParameters.Add("CertifiedByInformationOnSubmitToCH", "true");
            request5Body.FormPostParameters.Add("CertifiedByInformationOnSubmitToCH", this.Context["$HIDDEN1.CertifiedByInformationOnSubmitToCH"].ToString());
            request5Body.FormPostParameters.Add("DisplayThreeIdQuestionsOnly", "true");
            request5Body.FormPostParameters.Add("DisplayThreeIdQuestionsOnly", this.Context["$HIDDEN1.DisplayThreeIdQuestionsOnly"].ToString());
            request5Body.FormPostParameters.Add("YesStatement", "Yes, part of the deed has been redacted");
            request5Body.FormPostParameters.Add("NoStatement", "No, none of the deed has been redacted ");
            request5Body.FormPostParameters.Add("UrgentDays", "13");
            request5Body.FormPostParameters.Add("VeryUrgentDays", "5");
            request5Body.FormPostParameters.Add("ArchiveDays", "15");
            request5Body.FormPostParameters.Add("CustomerReferenceEditable", this.Context["$HIDDEN1.CustomerReferenceEditable"].ToString());
            request5Body.FormPostParameters.Add("ClientMatterNumberVisible", this.Context["$HIDDEN1.ClientMatterNumberVisible"].ToString());
            request5Body.FormPostParameters.Add("ClientMatterNumberRequired", this.Context["$HIDDEN1.ClientMatterNumberRequired"].ToString());
            request5Body.FormPostParameters.Add("CompanyHouseWorkingAlertVisible", "true");
            request5Body.FormPostParameters.Add("CompanyHouseWorkingAlertVisible", this.Context["$HIDDEN1.CompanyHouseWorkingAlertVisible"].ToString());
            request5Body.FormPostParameters.Add("SendChApprovedEmailToOrgAdmin", this.Context["$HIDDEN1.SendChApprovedEmailToOrgAdmin"].ToString());
            request5Body.FormPostParameters.Add("SendChRejectedEmailToOrgAdmin", "true");
            request5Body.FormPostParameters.Add("SendChRejectedEmailToOrgAdmin", this.Context["$HIDDEN1.SendChRejectedEmailToOrgAdmin"].ToString());
            request5Body.FormPostParameters.Add("SendInternalApprovedEmailToOrgAdmin", this.Context["$HIDDEN1.SendInternalApprovedEmailToOrgAdmin"].ToString());
            request5Body.FormPostParameters.Add("SendInternalRejectedEmailToOrgAdmin", this.Context["$HIDDEN1.SendInternalRejectedEmailToOrgAdmin"].ToString());
            request5Body.FormPostParameters.Add("AccountManagerEmail", "");
            request5Body.FormPostParameters.Add("ShowSendLinkInviteURL", this.Context["$HIDDEN1.ShowSendLinkInviteURL"].ToString());
            request5Body.FormPostParameters.Add("IsDateFromCreated", "true");
            request5Body.FormPostParameters.Add("IsDateFromCreated", this.Context["$HIDDEN1.IsDateFromCreated"].ToString());
            request5Body.FormPostParameters.Add("SortApproversBy", "2");
            request5Body.FormPostParameters.Add("RestrictedAccessStatus", "1");
            request5Body.FormPostParameters.Add("IsPdfLimit", this.Context["$HIDDEN1.IsPdfLimit"].ToString());
            request5.Body = request5Body;
            yield return request5;
            request5 = null;

            WebTestRequest request6 = new WebTestRequest((this.Context["WebServer1"].ToString() + "/Submission/Index"));
            request6.Headers.Add(new WebTestRequestHeader("Referer", (this.Context["WebServer1"].ToString() + "/Admin/Admin/Preferences")));
            SubmissionsExtractGlobalSubmissionListPlugin extractionRule5 = new SubmissionsExtractGlobalSubmissionListPlugin();
            extractionRule5.ContextParameterName = "SubmissionsGLS";
            request6.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule5.Extract);
            yield return request6;
            request6 = null;

            WebTestRequest request7 = new WebTestRequest((this.Context["WebServer1"].ToString() + "/Dashboard/Index"));
            request7.Headers.Add(new WebTestRequestHeader("Referer", (this.Context["WebServer1"].ToString() + "/Submission/Index")));
            yield return request7;
            request7 = null;

            WebTestRequest request8 = new WebTestRequest((this.Context["WebServer1"].ToString() + "/Account/Logout"));
            request8.ExpectedResponseUrl = (this.Context["WebServer1"].ToString() + "/Account/Login");
            request8.Headers.Add(new WebTestRequestHeader("Referer", (this.Context["WebServer1"].ToString() + "/Dashboard/Index")));
            yield return request8;
            request8 = null;
        }
    }
}
