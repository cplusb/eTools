using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace eTools.Tools
{
    /// <summary>
    /// xml解析工具类，返回解析结果LIST
    /// </summary>
    class XmlTools
    {
        /// <summary>
        /// 解析XML字符串，返回处理后的实例对象
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        //public static List<ItemMarketInfoCollection> LoadXml(string message)
        //{
        //    var collections = new List<ItemMarketInfoCollection>();
        //    try
        //    {
        //        XmlDocument doc = new XmlDocument();
        //        doc.LoadXml(message);
        //        XmlNodeList nodeList = doc.SelectNodes("exec_api/marketstat");
        //        foreach (XmlNode node in nodeList)
        //        {
        //            XmlNodeList dataList = ((XmlElement)node).ChildNodes;
        //            foreach (XmlNode dataNode in dataList)
        //            {
        //                var dataStr = (XmlElement)dataNode;
        //                var itemCollection = new ItemMarketInfoCollection();
        //                itemCollection.ItemKey = dataStr.GetAttribute("id");
        //                foreach (XmlNode cellNode in dataStr.ChildNodes)
        //                {
        //                    var cellStr = (XmlElement)cellNode;

        //                    var cellPart = itemCollection[cellStr.Name];
        //                    foreach (XmlNode cs in cellStr.ChildNodes)
        //                        cellPart.Set(cs.Name, cs.InnerText);
        //                }
        //                collections.Add(itemCollection);
        //            }
        //        }
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //    return collections;
        //}
    }
}
