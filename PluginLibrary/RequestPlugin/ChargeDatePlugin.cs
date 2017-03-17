using Microsoft.VisualStudio.TestTools.WebTesting;
using System;
using System.ComponentModel;

namespace PluginLibrary
{
    public class ChargeDatePlugin : WebTestRequestPlugin
    {
        //[Description("Set charge date")]
        //public string ParameterName { get; set; }
        [Description("Charge creation date , the number of days in past, from today:")]
        public int daysInPast { get; set; }

        public override void PreRequest(object sender, PreRequestEventArgs e)
        {
            base.PreRequest(sender, e);

            DateTime chargeDate = DateTime.Now.AddDays(-daysInPast);

            e.WebTest.Context.Add("ChargeDateDay", chargeDate.Day);
            e.WebTest.Context.Add("ChargeDateMonth", chargeDate.Month);
            e.WebTest.Context.Add("ChargeDateYear", chargeDate.Year);
            e.WebTest.Context.Add("ChargeDate", chargeDate);
        }
    }
}
