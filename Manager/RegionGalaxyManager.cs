using eTools.Model.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Manager
{
    public class RegionGalaxyManager
    {
        public static List<Region> Regions { get; set; }

        private static Region _DefaultRegion;
        private static Galaxy _DefaultGalaxy;

        public static Region DefaultRegion
        {
            get
            {
                if (_DefaultRegion == default)
                {
                    foreach (var region in Regions)
                    {
                        if (region.Id == 10000002)
                        {
                            _DefaultRegion = region;
                            break;
                        }
                    }
                }
                return _DefaultRegion;
            }
        }
        public static Galaxy DefaultGalaxy
        {
            get
            {
                if (_DefaultGalaxy == default)
                    DefaultRegion.Galaxy.TryGetValue(30000142, out _DefaultGalaxy);
                return _DefaultGalaxy;
            }
        }
    }

    //public class RegionGalaxyManager
    //{
    //    public static List<Region> Regions { get; set; }

    //    private static Region _DefaultRegion;
    //    private static Astro _DefaultAstro;
    //    private static Galaxy _DefaultGalaxy;

    //    public static Region DefaultRegion
    //    {
    //        get
    //        {
    //            if(_DefaultRegion == default)
    //            {
    //                foreach (var region in Regions)
    //                {
    //                    if (region.Id == 10000002)
    //                    {
    //                        _DefaultRegion = region;
    //                        break;
    //                    }
    //                }
    //            }
    //            return _DefaultRegion;
    //        }
    //    }
    //    public static Astro DefaultAstro
    //    {
    //        get
    //        {
    //            if (_DefaultAstro == default)
    //                DefaultRegion.Astro.TryGetValue(20000020, out _DefaultAstro);
    //            return _DefaultAstro;
    //        }
    //    }
    //    public static Galaxy DefaultGalaxy
    //    {
    //        get
    //        {
    //            if (_DefaultGalaxy == default)
    //                DefaultAstro.Galaxy.TryGetValue(30000142, out _DefaultGalaxy);
    //            return _DefaultGalaxy;
    //        }
    //    }
    //}
}
