using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace eTools.Tools
{
    class JsonTools
    {
        public static T ParseTo<T>(string msg)
        {
            T t = default;
            try
            {
                if (!string.IsNullOrEmpty(msg))
                    t = JsonConvert.DeserializeObject<T>(msg);
            }
            catch(Exception e){ MessageBox.Show(e.Message); }
            return t;
        }
    }
}


/*
 # eTools  
# des:  
  针对市场和LP查询、蓝图制造、矿物精炼、合同估价做的小工具  
  eve tools for market&lp query, manufacturing, mineral spirit, contract loot.  
  
# attention:  
  静态数据库在默认工具目录，需要将它挪到你的工作目录下  
  statcic db put in working directory, u need move or copy it to your working directory.  
  
# 引用说明
  1.数据来源ESI、EveMarketer  
  2.插件引用HZH_Controls绘制蓝图制造组织结构图  
# Refer to instructions  
  1.static db from ESI&EveMarket  
  2.plugin used HZH_Controls draw blueprints manufacturing organizational chart.  
  
# 部分测试版功能：  
  1.市场和LP查询  
  2.蓝图制造（部分材料显示，反应图还没找到数据库）  
# part beta function:    
  1.market&lp query.    
  2.manufacturing(part show).  

 
 */
