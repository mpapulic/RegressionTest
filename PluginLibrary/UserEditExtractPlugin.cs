using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.WebTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PluginLibrary.Models;
using System.IO;

namespace PluginLibrary
{
    [DisplayName("Extract Value from the Table Users")]
    [Description("Grabs a GUID value from user table.")]
    public class UserEditExtractPlugin : ExtractionRule
    {

        [DisplayName("User mail ")]
        [Description("The mail of the user added in the previous step:")]
        public string UserMail { get; set; }

        [DisplayName("User GUID ")]
        [Description("The GUID of the user added in the previous step:")]
        public string UserGUID { get; set; }

        public override void Extract(object sender, ExtractionEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(UserMail) == true)
            {
                Fail(e, "User name does not have a valid value.");
            }
            else
            {

                //e.WebTest.Context.Add("NewUser", "234");
                System.IO.File.WriteAllText(@"C:\Temp\TestRESPONSE.txt", "Usao u extraction rule, STEP 1");
                GrabValue(e);
            }
        }
        private void GrabValue(ExtractionEventArgs e)
        {

            string textJson = e.Response.BodyString;
                
            UserGUID = UsersProcessing.GetUserByMail(textJson, UserMail);

            System.IO.File.WriteAllText(@"C:\Temp\TestUserGUID.txt", UserGUID);

            e.WebTest.Context.Add("NewUser", UserGUID);
            //GrabValue(e, table);
        }

        private void Fail(ExtractionEventArgs e, string message)
        {
            e.Success = false;
            e.Message = message;
            e.WebTest.Context.Add(ContextParameterName, "(FAIL)");
        }
    }
}
