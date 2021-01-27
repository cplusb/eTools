using eTools.Global;
using eTools.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Datas
{
    [System.Serializable]
    /// <summary>
    /// 新构建的市场组数据
    /// </summary>
    public class NewMarketGroup
    {
        public int ID { get; set; }
        public bool hasTypes { get; set; }
        public int iconID { get; set; }
        public string nameID { get; set; }
        public int parentGroupID { get; set; }

        public NewMarketGroup() { }
        public NewMarketGroup(int id, MarketGroup marketGroup)
        {
            ID = id;
            hasTypes = marketGroup.hasTypes;
            iconID = marketGroup.iconID;
            nameID = marketGroup.nameID.zh;
            parentGroupID = marketGroup.parentGroupID;
        }
    }


    [System.Serializable]
    /// <summary>
    /// market item group contains map item parentGroupID.
    /// </summary>
    public class MarketGroup
    {
        public Multilingual descriptionID { get; set; }
        public bool hasTypes { get; set; }
        public int iconID { get; set; }
        public Multilingual nameID { get; set; }
        public int parentGroupID { get; set; }
    }

    /// <summary>
    /// 市场查询结果显示格式
    /// </summary>
    public class MarketGroupTypeShow
    {
        public MarketGroupTypeShow(string name, double sale, double buy)
        {
            _name = name;
            _priceSale = sale;
            _priceBuy = buy;
            _priceSub = _priceSale * (1 - (Tax.MarketMedian + Tax.MarketSale) / 100f) - _priceBuy * (1 + Tax.MarketMedian / 100f);
        }

        private string _name;
        private double _priceSale;
        private double _priceBuy;
        private double _priceSub;

        public double Sale { get => _priceSale; }
        public double Buy { get => _priceBuy; }
        public double Sub { get => _priceSub; }

        public string Name { get => _name; }
        public string PriceSale { get => Format.SegmentaThree(Format.FormatDecimalThree(_priceSale)); }
        public string PriceBuy { get => Format.SegmentaThree(Format.FormatDecimalThree(_priceBuy)); }
        public string PriceSub { get => Format.SegmentaThree(Format.FormatDecimalThree(_priceSub)); }
        public string PriceRate { get; }
    }
}
