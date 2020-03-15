using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elsa.Samples.UserRegistration.Web.Models
{
    public class TaskOrder
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string Assignee { get; set; }

        public TaskOrderStaging OrderStaging { get; set; }
    }
}
