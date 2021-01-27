using eTools.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Datas
{
    public class LpItem
    {
        public int ak_cost { get; set; }
        public double isk_cost { get; set; }
        public int lp_cost { get; set; }
        public int offer_id { get; set; }
        public int quantity { get; set; }
        public List<LpPreItem> required_items { get; set; }
        public int type_id { get; set; }
    }

    public class LpPreItem
    {
        public int quantity { get; set; }
        public int type_id { get; set; }
    }

    public class LpGroupTypeShow
    {
        public LpGroupTypeShow()
        {
            Count = 0;
            LpCpost = 0;
            IskCost = 0;
            PreCost = 0;
            Sale = 0;
            Buy = 0;
        }
        public string Name { get; set; }
        public int Count { get; set; }
        public int LpCpost { get; set; }
        public double IskCost { get; set; }
        public string PreName { get; set; }
        public string PreCount { get; set; }
        public double PreCost { get; set; }
        public double Sale { get; set; }
        public double Buy { get; set; }
        public float SaleRate { get => (float)(_saleAfterTax / (LpCpost == 0 ? 1 : LpCpost) * 1.0f); }
        public float BuyRate { get => (float)(_buyAfterTax / (LpCpost == 0 ? 1 : LpCpost) * 1.0f); }

        private double _saleAfterTax
        {
            get
            {
                var salefterTax = Sale * (100 - Global.Tax.LpSale - Global.Tax.LpMedian) / 100f;
                double preAfterTax = PreCost;
                if (!Global.Signal.LpPreSale)
                    preAfterTax = PreCost * (100 + Global.Tax.LpMedian) / 100f;
                return salefterTax - IskCost - preAfterTax;
            }
        }
        private double _buyAfterTax
        {
            get
            {
                var buyfterTax = Buy * (100 - Global.Tax.LpSale) / 100f;
                double preAfterTax = PreCost;
                if (!Global.Signal.LpPreSale)
                    preAfterTax = PreCost * (100 + Global.Tax.LpMedian) / 100f;
                return buyfterTax - IskCost - preAfterTax;
            }
        }

        public string SaleShow { get => Format.SegmentaThree(Format.FormatDecimalThree(Sale)); }
        public string BuyShow { get => Format.SegmentaThree(Format.FormatDecimalThree(Buy)); }
        public string SaleRateShow { get => Format.SegmentaThree(Format.FormatDecimalThree(SaleRate)); }
        public string BuyRateShow { get => Format.SegmentaThree(Format.FormatDecimalThree(BuyRate)); }
        public string PreCostShow { get => Format.SegmentaThree(Format.FormatDecimalThree(PreCost)); }
        public string IskCostShow { get => Format.SegmentaThree(Format.FormatDecimalThree(IskCost)); }
        public string LpCpostShow { get => Format.SegmentaThree(Format.FormatDecimalThree(LpCpost)); }
        public string CountShow { get => Format.SegmentaThree(Format.FormatDecimalThree(Count)); }
    }
}
