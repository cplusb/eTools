using eTools.ViewModel.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace eTools.Manager.Notice
{
    class StatusManager
    {
        private static StatusManager _mgr;
        public static StatusManager GetManager() => _mgr = _mgr ?? (_mgr = new StatusManager());

        private StatusMsgVM _statusMsgVM;

        public void ShowMsg(string msg)
        {
            
            _statusMsgVM = _statusMsgVM ?? 
                (_statusMsgVM = XamlComponentsManager.GetManager().Get<Grid>(XamlComponentsType.RootGrid).Resources["statusMsgVM"] as StatusMsgVM);
            _statusMsgVM.MsgStatus = msg;
        }

        public void Clear() => ShowMsg("");
    }
}
