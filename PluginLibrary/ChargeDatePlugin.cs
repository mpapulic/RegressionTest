using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.WebTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PluginLibrary.Helper;
using PluginLibrary.Models;

namespace PluginLibrary
{
    public class ChargeDatePlugin : WebTestRequestPlugin
    {
        [Description("Set charge date")]
        public string ParameterName { get; set; }
        [Description("How many days in past:")]
        public int daysInPast { get; set; }

        public override void PreRequest(object sender, PreRequestEventArgs e)
        {
            base.PreRequest(sender, e);

            DateTime chargeDate = DateTime.Now.AddDays(-daysInPast);

            e.WebTest.Context.Add("ChargeDateDay", chargeDate.Day);
            e.WebTest.Context.Add("ChargeDateMonth", chargeDate.Month);
            e.WebTest.Context.Add("ChargeDateYear", chargeDate.Year);
            System.IO.File.WriteAllText(@"C:\Temp\ChargeDatePlugin.txt", $"Usao u ChargeDatePlugin days in past: {daysInPast}  ChargeDate : {chargeDate} .");
        }
    }
}
