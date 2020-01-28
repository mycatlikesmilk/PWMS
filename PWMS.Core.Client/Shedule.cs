using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWMS.Core.Client
{
    class SheduleElement
    {
        public DateTime From;
        public DateTime To;
        public string ParentTechID;
        public string ParentTechWorkflowID;
        public string State;
    }
}
