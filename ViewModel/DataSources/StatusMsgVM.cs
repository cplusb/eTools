using eTools.Datas;
using eTools.Manager.Notice;
using eTools.Manager.Web;
using eTools.Model.DataSources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace eTools.ViewModel.DataSources
{
    class StatusMsgVM : INotifyPropertyChanged
    {
        private StatusMsg _msgStatus;

        public StatusMsgVM() => _msgStatus = new StatusMsg();
        public string MsgStatus
        {
            get => _msgStatus.MsgStatus;
            set
            {
                _msgStatus.MsgStatus = value;
                OnPropertyChanged("MsgStatus");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}
