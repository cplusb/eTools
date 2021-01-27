using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Model.DataSources
{
    class StatusMsg
    {
        private string _msgStatus = "";
        public string MsgStatus
        {
            get => _msgStatus;
            set=> _msgStatus = value;
        }
    }
}
