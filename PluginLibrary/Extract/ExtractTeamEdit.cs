using Microsoft.VisualStudio.TestTools.WebTesting;
using System;
using System.ComponentModel;
using PluginLibrary.Helper;

namespace PluginLibrary
{
    [DisplayName("Extract Value From Column In Table Teams")]
    [Description("Grabs a ID value from team table.")]
    public class TeamEditExtractPlugin : ExtractionRule
    {

        [DisplayName("Team name ")]
        [Description("The name of the team added in the previous step:")]
        public string TeamName { get; set; }

        [DisplayName("Team ID ")]
        [Description("The ID of the team added in the previous step:")]
        public string TeamID { get; set; }

        public override void Extract(object sender, ExtractionEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(TeamName) == true)
            {
                Fail(e, "Team name does not have a valid value.");
            }
            else
            {

                //e.WebTest.Context.Add("NewTeam", "234");
                GrabValue(e);
            }
        }
        private void GrabValue(ExtractionEventArgs e)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(e.Response.BodyString);


            TeamID = TeamsProcessing.GetTeamByName(doc, TeamName);
            //System.IO.File.WriteAllText(@"C:\Temp\TestPARAMETRI.txt", $"Team name: {TeamName}  team ID : {TeamID} .");
            //e.WebTest.Context.Add(ContextParameterName, "Pretraga se vrsi za:" + TeamName);
            //e.WebTest.Context.Add(ContextParameterName, "Odabrani team WEBTEST 002 ima ID:" + TeamID);

            e.WebTest.Context.Add("NewTeam", TeamID);
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
