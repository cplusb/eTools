using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Model.DataSources
{
    class ItemTypeFilter
    {
        private List<string> _filters;
        public List<string> Filters { get => _filters; set => _filters = value; }

        public ItemTypeFilter()
        {
            _filters = new List<string>();
            foreach (var key in Global.Filter.ItemTypeFilterDict.Keys)
                _filters.Add(key);
        }
    }
}
