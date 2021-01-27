using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Model.DataSources
{
    [System.Serializable]
    class RegionGalaxy
    {
        public List<Region> Regions { get; set; }
    }

    [System.Serializable]
    public class Region
    {
        public Region() { }
        public Region(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public Region(int id, string name, Dictionary<int, Galaxy> galaxy)
        {
            Id = id;
            Name = name;
            Galaxy = galaxy;
        }
        public string Name { get; set; }
        public int Id { get; set; }
        public Dictionary<int, Galaxy> Galaxy { get; set; }
    }

    [System.Serializable]
    public class Galaxy
    {
        public Galaxy() { }
        public Galaxy(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public string Name { get; set; }
        public int Id { get; set; }
    }

    //[System.Serializable]
    //public class Region
    //{
    //    public string Name { get; set; }
    //    public int Id { get; set; }
    //    public Dictionary<int, Astro> Astro { get; set; }
    //}

    //[System.Serializable]
    //public class Astro
    //{
    //    public string Name { get; set; }
    //    public int Id { get; set; }

    //    public Dictionary<int, Galaxy> Galaxy { get; set; }
    //}

    //[System.Serializable]
    //public class Galaxy
    //{
    //    public string Name { get; set; }
    //    public int Id { get; set; }
    //}
}
