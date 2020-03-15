using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elsa.Samples.UserRegistration.Web.Models
{
    public enum TaskOrderStaging
    {
        Schedule = 0,
        ToDo = 1,
        Doing = 5,
        Done = 10,
        Pause = 15
    }
}
