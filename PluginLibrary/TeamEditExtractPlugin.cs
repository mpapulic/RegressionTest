using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.WebTesting;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PluginLibrary
{
    class TeamEditExtractPlugin : ExtractionRule
    {
        /// Specify a name for use in the user interface.
        /// The user sees this name in the Add Extraction dialog box.
        //---------------------------------------------------------------------
        public override string RuleName
        {
            get { return "Definisanje imena metode za ekstrakciju"; }
        }

        /// Specify a description for use in the user interface.
        /// The user sees this description in the Add Extraction dialog box.
        //---------------------------------------------------------------------
        public override string RuleDescription
        {
            get { return "Peuzimanje podataka iz liste Team"; }
        }

        // The name of the desired input field
        private string NameValue;
        public string Name
        {
            get { return NameValue; }
            set { NameValue = value; }
        }


        private string regularexpression;
        public string MyRegularExpression
        {
            get
            {
                return this.regularexpression;
            }
            set
            {
                this.regularexpression = value;
            }
        }
        public override void Extract(object sender, ExtractionEventArgs e)
        {
            List<String> lst = new List<String>();
            Regex rg = new Regex(MyRegularExpression);
            MatchCollection mcoll = rg.Matches(e.Response.BodyString, 0);
            for (int i = 0; i < mcoll.Count; i++)
            {
                lst.Add(mcoll[i].ToString());
            }
            e.WebTest.Context.Add(this.ContextParameterName, lst);
        }
    }
}
