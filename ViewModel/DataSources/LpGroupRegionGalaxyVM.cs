using eTools.Model.DataSources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace eTools.ViewModel.DataSources
{
    class LpGroupRegionGalaxyVM : INotifyPropertyChanged
    {
        private RegionGalaxy _regionGalaxy;

        public LpGroupRegionGalaxyVM()
        {
            _regionGalaxy = new RegionGalaxy();
        }

        private Region _selectedRegion;
        public Region SelectedRegion
        {
            get => _selectedRegion;
            set
            {
                SiwtchGalaxy(value);
                _selectedRegion = value;
                Global.RegionGalaxyID.LpQueryRegionID = value.Id.ToString();
                //var s = eTools.Manager.XamlComponentsManager.GetManager().Get<ComboBox>(eTools.Manager.XamlComponentsType.LpGroupGalaxy);
                //s.SelectedIndex = 0;
            }
        }

        private Galaxy _selectedGalaxy;
        public Galaxy SelectedGalaxy
        {
            get => _selectedGalaxy;
            set
            {
                if (value == null)
                    return;
                _selectedGalaxy = value;
                Global.RegionGalaxyID.LpQueryGalaxyID = value.Id.ToString();
            }
        }

        public List<Region> Regions
        {
            get => _regionGalaxy.Regions; set
            {
                _regionGalaxy.Regions = value;
                OnPropertyChanged("Regions");
                if (value != null && value.Count > 0) SiwtchGalaxy(value[0]);
            }
        }

        private List<Galaxy> _galaxy;
        public List<Galaxy> Galaxy
        {
            get => _galaxy; set
            {
                _galaxy = value;
                OnPropertyChanged("Galaxy");
                eTools.Manager.XamlComponentsManager.GetManager().Get<ComboBox>(eTools.Manager.XamlComponentsType.LpGroupGalaxy).SelectedIndex = 0;
            }
        }

        private void SiwtchGalaxy(Region region)
        {
            var tmp = new List<Galaxy>();
            foreach (var galaxy in region.Galaxy.Values)
                tmp.Add(galaxy);
            Galaxy = tmp;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}
