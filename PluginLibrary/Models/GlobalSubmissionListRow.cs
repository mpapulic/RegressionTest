using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginLibrary.Models
{
    public class GlobalSubmissionListRow
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string CreatedBy { get; set; }
        public string Created { get; set; }
        public string Chargedate { get; set; }
        public string Nonusable { get; set; }
        public string Submitted { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public string SubmissionGUID { get; set; }
        public string UrgentType { get; set; }

    }
}
