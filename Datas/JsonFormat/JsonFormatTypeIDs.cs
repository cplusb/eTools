using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Datas
{
    [System.Serializable]
    public class JsonFormatType
    {
        public JsonFormatTypeStat buy { get; set; }
        public JsonFormatTypeStat sell { get; set; }
    }
    [System.Serializable]
    public class JsonFormatTypeStat
    {
        public JsonFormatForQuery forQuery { get; set; }
        public Int64 volume { get; set; }
        public double wavg { get; set; }
        public double avg { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public double variance { get; set; }
        public double stdDev { get; set; }
        public double median { get; set; }
        public double fivePercent { get; set; }
        public bool highToLow { get; set; }
        public Int64 generated { get; set; }
    }
    [System.Serializable]
    public class JsonFormatForQuery
    {
        public bool bid { get; set; }
        public List<int> types { get; set; }
        public List<int> regions { get; set; }
        public List<int> systems { get; set; }
        public Int64 hours { get; set; }
        public Int64 minq { get; set; }
    }
}
