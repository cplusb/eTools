using eTools.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static eTools.Manager.Web.QueryManager;

namespace eTools.Model.Query
{
    class QueryMarketGroup
    {
        public QueryMarketGroup() => _typeShowSources = new List<MarketGroupTypeShow>();
        private List<MarketGroupTypeShow> _typeShowSources { get; set; }
        public List<MarketGroupTypeShow> TypeShowSources { get; set; }

    }
}
