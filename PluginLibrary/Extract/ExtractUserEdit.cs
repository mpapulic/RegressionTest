using Microsoft.VisualStudio.TestTools.WebTesting;
using System;
using System.ComponentModel;
using PluginLibrary.Models;

namespace PluginLibrary
{
    [DisplayName("Extract Value from the Table Users")]
    [Description("Grabs a GUID value from user table.")]
    public class UserEditExtractPlugin : ExtractionRule
    {

        [DisplayName("UserMail")]
        [Description("The mail of the user added in the previous step:")]
        public string UserMail { get; set; }

        //[DisplayName("UserGUID")]
        //[Description("The GUID of the user added in the previous step:")]
        //public string UserGUID { get; set; }

        public override void Extract(object sender, ExtractionEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(UserMail) == true)
            {
                Fail(e, "User name does not have a valid value.");
            }
            else
            {

                //e.WebTest.Context.Add("NewUser", "234");
                //System.IO.File.WriteAllText(@"C:\Temp\TestRESPONSE.txt", "Usao u extraction rule, STEP 1");
                System.IO.File.WriteAllText(@"C:\Temp\TestExtract.txt", "Usao u EXTRACT metodu i zovem GrabValue(e)");
                GrabValue(e);
            }
        }
        private void GrabValue(ExtractionEventArgs e)
        {

            string textJson = e.Response.BodyString;
            string userMail = e.WebTest.Context["newUserMail"].ToString() ;
            System.IO.File.WriteAllText(@"C:\Temp\TestGrabValue.txt", "Usao u GrabValue i pozivam GetUserByMail"+ "JSON kao text:" + textJson + "/r a trazi se mail:" + UserMail);
            string userGUID = GetUserID.GetUserByMail(textJson, UserMail);
            System.IO.File.WriteAllText(@"C:\Temp\TestGrabValueIzlaz.txt", "Izlaz iz GrabValue i  GetUserByMail vratio:" + userGUID);


            e.WebTest.Context.Add("UserGUID", userGUID);
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
