using eTools.Model.DataSources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace eTools.ViewModel.DataSources
{

    public class MarketGroupRegionGalaxyVM : INotifyPropertyChanged
    {
        private RegionGalaxy _regionGalaxy;

        public MarketGroupRegionGalaxyVM()
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
                Global.RegionGalaxyID.MarketGroupRegionID = value.Id.ToString();
            }
        }

        private Galaxy _selectedGalaxy;
        public Galaxy SelectedGalaxy
        {
            get => _selectedGalaxy;
            set
            {
                if (value == null) return;
                _selectedGalaxy = value;
                Global.RegionGalaxyID.MarketGroupGalaxyID = value.Id.ToString();
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
                eTools.Manager.XamlComponentsManager.GetManager().Get<ComboBox>(eTools.Manager.XamlComponentsType.MarketGroupGalaxy).SelectedIndex = 0;
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
    //public class RegionGalaxyVM : INotifyPropertyChanged
    //{
    //    private RegionGalaxy _regionGalaxy;

    //    public RegionGalaxyVM()
    //    {
    //        _regionGalaxy = new RegionGalaxy();
    //    }

    //    private Region _selectedRegion;
    //    public Region SelectedRegion
    //    {
    //        get => _selectedRegion;
    //        set
    //        {
    //            SwitcAstro(value);
    //            _selectedRegion = value;
    //        }
    //    }

    //    private Astro _selectedAstro;
    //    public Astro SelectedAstro
    //    {
    //        get => _selectedAstro;
    //        set
    //        {
    //            _selectedAstro = value;
    //            SiwtchGalaxy(value);
    //        }
    //    }

    //    private Galaxy _selectedGalaxy;
    //    public Galaxy SelectedGalaxy
    //    {
    //        get => _selectedGalaxy;
    //        set=> _selectedGalaxy = value;
    //    }

    //    public List<Region> Regions { get => _regionGalaxy.Regions; set {
    //            _regionGalaxy.Regions = value;
    //            OnPropertyChanged("Regions");
    //            if (value != null && value.Count > 1) SwitcAstro(value[0]);
    //        } }

    //    private List<Astro> _astros;
    //    public List<Astro> Astros { get => _astros; set
    //        {
    //            _astros = value;
    //            OnPropertyChanged("Astros");
    //            if (value != null && value.Count > 1) SiwtchGalaxy(value[0]);
    //        }
    //    }

    //    private List<Galaxy> _galaxy;
    //    public List<Galaxy> Galaxy { get => _galaxy; set {
    //            _galaxy = value;
    //            OnPropertyChanged("Galaxy");
    //        } }


    //    private void SwitcAstro(Region region)
    //    {
    //        var tmp = new List<Astro>();
    //        foreach (var astro in region.Astro.Values)
    //            tmp.Add(astro);
    //    }

    //    private void SiwtchGalaxy(Astro astro)
    //    {
    //        var tmp = new List<Galaxy>();
    //        foreach (var galaxy in astro.Galaxy.Values)
    //            tmp.Add(galaxy);
    //        Galaxy = tmp;
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;
    //    public void OnPropertyChanged(string propertyName) => PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    //}
}
