using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using YamlDotNet.Serialization;
using eTools.Datas;

namespace eTools.Tools
{
    public class YamlTools
    {
        /// <summary>
        /// 读配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Read<T>(string path)
        {
            path = FPS.Combine(path);
            T t = default;
            try
            {
                Deserializer deserializer = new Deserializer();
                if(FPS.CheckFile(path, FpsModel.Default))
                    using (TextReader reader = File.OpenText(path))
                        t = deserializer.Deserialize<T>(reader);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return t;
        }

        /// <summary>
        /// 写配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="path"></param>
        public static void Write<T>(T t, string path)
        {
            try
            {
                path = FPS.Combine(path);
                Serializer serializer = new Serializer();
                StringWriter strWriter = new StringWriter();
                serializer.Serialize(strWriter, t);
                serializer.Serialize(Console.Out, t);
                using (TextWriter writer = File.CreateText(path))
                    writer.Write(strWriter.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }

    /// <summary>
    /// 过滤器，去除多余数据减少内存
    /// 开发协助，非程序逻辑
    /// </summary>
    public class YamlFilter
    {
        /// <summary>
        /// 消除多余物体属性
        /// </summary>
        /// <param name="collections"></param>
        /// <param name="newTypeID"></param>
        public static void TypeIDs(Dictionary<int, TypeID> collections, out Dictionary<int, NewTypeID> newTypeID)
        {
            newTypeID = new Dictionary<int, NewTypeID>();
            foreach (var kv in collections)
                if (kv.Value.published)
                    newTypeID.Add(kv.Key, new NewTypeID(kv.Key, kv.Value));
        }
        /// <summary>
        /// 修改市场组属性
        /// </summary>
        /// <param name="collections"></param>
        /// <param name="newMarketGroup"></param>
        public static void MarketGroups(Dictionary<int, MarketGroup> collections, out Dictionary<int, NewMarketGroup> newMarketGroup)
        {
            newMarketGroup = new Dictionary<int, NewMarketGroup>();
            foreach (var kv in collections)
                    newMarketGroup.Add(kv.Key, new NewMarketGroup(kv.Key, kv.Value));
        }
    }


}
