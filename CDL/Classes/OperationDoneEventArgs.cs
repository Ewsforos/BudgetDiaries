using CDL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDL.Classes
{
    public class OperationDoneEventArgs : EventArgs
    {
        public OperationType Type { get; set; }
        public string Message { get; set; }
        public bool Result { get; set; }
    }
}
