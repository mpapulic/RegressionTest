using System.Collections.Generic;
using HtmlAgilityPack;
using System.IO;
using PluginLibrary.Models;

namespace PluginLibrary.Helper
{
    public static class TeamsProcessing
    {
        public static string GetTeamByName(HtmlDocument doc, string teamname )
        {

            string teamID = "";
                List<Team> listTeam = new List<Team>();


                listTeam = GetAllTeams(doc);

                int  index = listTeam.FindIndex(x => x.Vrednost.Contains(teamname));
                if (index != -1 )
                    teamID = listTeam[index].ID;
                return teamID;
        }
        public static List<Team>  GetAllTeams(HtmlDocument doc)
        {
            // list of the TEAMs on the page
            List<Team> list = new List<Team>();

            // searching for all tables in the HTM document
            int k = 1;
            HtmlNodeCollection nodesTable = doc.DocumentNode.SelectNodes("//table");

            // passing through all tables in the HTML document
            int iTablesCount = nodesTable.Count;
            if (iTablesCount == 0)
            {
                return list;
            }
            foreach (var TABLE in nodesTable)
            {
                // Searching all rows in the table [k]
                HtmlNodeCollection nodesRows = TABLE.SelectNodes($"//table[{k}]//tr");
                if (nodesRows != null)
                {
                    int j = 1;
                    foreach (var ROW in nodesRows)
                    {
                        HtmlNodeCollection nodesCells = ROW.SelectNodes($"//tr[{j}]//td");
                        if (nodesCells != null)
                        {
                            // row which contains data
                            int iCellsCount = nodesCells.Count;
                            int i = 0;
                            string teamName = "";
                            string teamId = "";
                            // 
                            foreach (var CELL in nodesCells)
                            {
                                // take a regex 
                                string a = CELL.InnerText.Trim('\n', '\r', ' ');
                                string b = CELL.InnerHtml.Trim('\n', '\r', ' ');
                                // the fist column = Team name , the second column contains ID for editing Team, the third column contains info is possible delete Team
                                if (i == 0)
                                {
                                    teamName = a;
                                }
                                if (a.Contains("Edit"))
                                {
                                    teamId = b.Substring(b.IndexOf("Edit") + 5, b.IndexOf("class") - b.IndexOf("Edit") - 7);
                                }
                                i++;
                            }
                            list.Add(new Team { Table = "Team", Vrednost = teamName, ID = teamId });
                        }
                        else
                        {
                            // headers of the columns
                            using (StreamWriter sw = File.AppendText(@"C:\Temp\TestTeam.txt"))
                            {
                                sw.WriteLine($"{j}/{nodesRows.Count} =>  ovaj se nije obradio jer je nodesCell == null ");
                            }
                            HtmlNodeCollection nodesHeaders = ROW.SelectNodes($"//tr[{j}]//th");
                            if (nodesHeaders != null)
                            {
                                int h = 0;
                                int iCellsCount = nodesHeaders.Count;
                                foreach (var HEADER in nodesHeaders)
                                {
                                    string a = HEADER.InnerText.Trim('\n', '\r', ' ');
                                    h++;
                                }
                            }
                        }
                        j++;
                    }
                }
                k++;
            }
            return list;
        }
    }
}
