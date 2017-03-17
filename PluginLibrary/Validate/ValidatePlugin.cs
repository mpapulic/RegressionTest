using System;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace PluginLibrary
{
    //-------------------------------------------------------------------------  
    // This class creates a custom validation rule named "Custom Validate Tag"  
    // The custom validation rule is used to check that an HTML tag with a   
    // particular name is found one or more times in the HTML response.  
    // The user of the rule can specify the HTML tag to look for, and the   
    // number of times that it must appear in the response.  
    //-------------------------------------------------------------------------  
    public class UrgentSubmissionValidate : ValidationRule
    {
        /// Specify a name for use in the user interface.  
        /// The user sees this name in the Add Validation dialog box.  
        //---------------------------------------------------------------------  
        public override string RuleName
        {
            get { return "Validation is submission urgent or very urgent on Global submission list."; }
        }

        /// Specify a description for use in the user interface.  
        /// The user sees this description in the Add Validation dialog box.  
        //---------------------------------------------------------------------  
        public override string RuleDescription
        {
            get { return "Validates that the submission is in urgent or very urgent status on Global submission List ."; }
        }


        // Validate is called with the test case Context and the request context.  
        // These allow the rule to examine both the request and the response.  
        //---------------------------------------------------------------------  
        public override void Validate(object sender, ValidationEventArgs e)
        {
            // poziv dolazi samo ako je IsUrgentCheck filed tako da je tada sigurno failed 
            e.IsValid = false;
            e.Message = String.Format("Submission is not coloured on the proper way ! ");
            //System.IO.File.WriteAllText(@"C:\Temp\Validate.txt", $"Usao u UrgentSubmissionValidate  i ovo je rezultat za submission: { e.IsValid .ToString()} .");

            // If the validation fails, set the error text that the user sees  
        }
    }
}