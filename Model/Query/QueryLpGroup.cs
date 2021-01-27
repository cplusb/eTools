using eTools.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Model.Query
{
    class QueryLpGroup
    {
        public QueryLpGroup() => _typeShowSources = new List<LpGroupTypeShow>();
        private List<LpGroupTypeShow> _typeShowSources { get; set; }
        public List<LpGroupTypeShow> TypeShowSources { get; set; }
    }
}
