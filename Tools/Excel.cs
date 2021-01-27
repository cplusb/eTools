//using eTools.Model.DataSources;
//using NPOI.HSSF.UserModel;
//using NPOI.SS.UserModel;
//using NPOI.XSSF.UserModel;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;

//namespace eTools.Tools
//{
//    /// <summary>
//    /// NPOI方式
//    /// NPOI 是 POI 项目的 .NET 版本。POI是一个开源的Java读写Excel、WORD等微软OLE2组件文档的项目。使用 NPOI 你就可以在没有安装 Office 或者
//    /// 相应环境的机器上对 WORD/EXCEL 文档进行读写。
//    /// 优点：读取Excel速度较快，读取方式操作灵活性
//    /// 缺点：需要下载相应的插件并添加到系统引用当中。
//    /// 
//    /// 目的读取EXCEl文件，配置物品数据库
//    /// </summary>
//    public class ExcelTools
//    {
//        /// <summary>
//        /// 内部配置用
//        /// </summary>
//        /// <param name="path"></param>
//        public static void Read(string path)
//        {
//            ReadStart(Path.Combine(Tools.FPS.ResourceRootPath, path), out var lsts);
//            SwitchToJsonDict(lsts);
//            SaveJonsDict();
//        }

//        public static Dictionary<int, Region> Regions;

//        /// <summary>
//        /// 转换数据成json dict
//        /// </summary>
//        /// <param name="dataList"></param>
//        private static void SwitchToJsonDict(List<List<string>> dataList)
//        {
//            Dictionary<int, Region> regions = new Dictionary<int, Region>();
//            MessageBox.Show(dataList.Count.ToString());
//            for (var i = 0; i < dataList.Count; i++)
//            {
//                // 跳过标题
//                if (i == 0) continue;
//                var line = dataList[i];
//                var sysID = int.Parse(line[0]);
//                var sysName = line[1];
//                var astroID = int.Parse(line[2]);
//                var astroName = line[3];
//                var rgID = int.Parse(line[4]);
//                var rgName = line[5];

//                var region = GetReion(regions, rgID, rgName);
//                var astro = GetAstro(region, astroID, astroName);
//                var galaxy = GetGalaxy(astro, sysID, sysName);
//            }
//            int rN = 0, aN = 0, sN = 0;
//            foreach(var rg in regions.Values)
//            {
//                rN++;
//                foreach(var ao in rg.Astro.Values)
//                {
//                    aN++;
//                    foreach (var ga in ao.Galaxy.Values)
//                        sN++;
//                }
//            }
//            MessageBox.Show($"{rN}..{aN}..{sN}");
//            Regions = regions;
//        }


//        private static Region GetReion(Dictionary<int, Region> regions, int id, string Name)
//        {
//            Region region = default;
//            if (regions.ContainsKey(id)) region = regions[id];
//            else
//            {
//                region = new Region();
//                region.Astro = new Dictionary<int, Astro>();
//                regions.Add(id, region);
//            }
//            region.Id = id;
//            region.Name = Name;
//            return region;
//        }

//        private static Astro GetAstro(Region rg, int id, string Name)
//        {
//            Astro astro = default;
//            if (rg.Astro.ContainsKey(id)) astro = rg.Astro[id];
//            else
//            {
//                astro = new Astro();
//                astro.Galaxy = new Dictionary<int, Galaxy>();
//                rg.Astro.Add(id, astro);
//            }
//            astro.Id = id;
//            astro.Name = Name;
//            return astro;
//        }

//        private static Galaxy GetGalaxy(Astro astro, int id, string Name)
//        {
//            Galaxy galaxy = default;
//            if (astro.Galaxy.ContainsKey(id)) galaxy = astro.Galaxy[id];
//            else
//            {
//                galaxy = new Galaxy();
//                astro.Galaxy.Add(id, galaxy);
//            }
//            galaxy.Name = Name;
//            galaxy.Id = id;
//            return galaxy;
//        }



//        /// <summary>
//        /// 保存到本地
//        /// </summary>
//        private static void SaveJonsDict()
//        {
//            List<Region> rr = new List<Region>();
//            foreach (var rg in Regions.Values)
//                rr.Add(rg);
//            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(rr);
//            MessageBox.Show(jsonStr.Length.ToString());
//            MessageBox.Show(Tools.FPS.ResourceRootPath);
//            Tools.FPS.FileWriter.Write(Path.Combine(Tools.FPS.ResourceRootPath, "rag.json"), jsonStr);
//        }
//        /// <summary>
//        /// 加载excel
//        /// </summary>
//        /// <param name="filePath"></param>
//        /// <param name="dbLsts"></param>
//        public static void ReadStart(string filePath, out List<List<string>> dbLsts)
//        {
//            dbLsts = default;
//            DataSet ds = new DataSet();
//            DataTable dt = new DataTable();
//            string fileType = Path.GetExtension(filePath).ToLower();
//            string fileName = Path.GetFileName(filePath).ToLower();
//            try
//            {
//                ISheet sheet = null;
//                int sheetNumber = 0;
//                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

//                if (fileType == ".xlsx")
//                {
//                    // 2007版本
//                    XSSFWorkbook workbook = new XSSFWorkbook(fs);
//                    sheetNumber = workbook.NumberOfSheets;
//                    for (int i = 0; i < sheetNumber; i++)
//                    {
//                        string sheetName = workbook.GetSheetName(i);

//                        sheet = workbook.GetSheet(sheetName);
//                        if (sheet != null && sheetName == "星系列表")
//                        {
//                            ReadSheet(sheet, out dbLsts);
//                        }
//                    }
//                }
//                else if (fileType == ".xls")
//                {
//                    // 2003版本
//                    HSSFWorkbook workbook = new HSSFWorkbook(fs);
//                    sheetNumber = workbook.NumberOfSheets;
//                    for (int i = 0; i < sheetNumber; i++)
//                    {
//                        string sheetName = workbook.GetSheetName(i);
//                        sheet = workbook.GetSheet(sheetName);
//                        if (sheet != null && sheetName == "星系列表")
//                        {
//                            ReadSheet(sheet, out dbLsts);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
//            }
//        }
//        /// <summary>
//        /// 读取sheet
//        /// </summary>
//        /// <param name="sheet"></param>
//        /// <param name="dbLsts"></param>
//        private static void ReadSheet(ISheet sheet, out List<List<string>> dbLsts)
//        {
//            string sheetName = sheet.SheetName;
//            int startIndex = 0;// sheet.FirstRowNum;
//            int lastIndex = sheet.LastRowNum;
//            dbLsts = new List<List<string>>();
//            //数据填充
//            for (int i = startIndex; i <= lastIndex; i++)
//            {
//                IRow row = sheet.GetRow(i);
//                List<string> lst = new List<string>();
//                if (row != null)
//                {
//                    for (int j = row.FirstCellNum; j < row.LastCellNum; ++j)
//                    {
//                        if (row.GetCell(j) != null)
//                        {
//                            ICell cell = row.GetCell(j);
//                            switch (cell.CellType)
//                            {
//                                case CellType.Blank:
//                                    break;
//                                case CellType.Numeric:
//                                    short format = cell.CellStyle.DataFormat;
//                                    //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理 
//                                    if (format == 14 || format == 31 || format == 57 || format == 58)
//                                    { }
//                                    else
//                                    {
//                                        lst.Add(cell.NumericCellValue.ToString());
//                                    }
//                                    if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 188)
//                                    { }
//                                    break;
//                                case CellType.String:
//                                    lst.Add(cell.StringCellValue);
//                                    break;
//                                case CellType.Formula:
//                                    break;
//                                default:

//                                    break;
//                            }
//                        }
//                    }
//                }
//                dbLsts.Add(lst);
//            }
//        }

//        //private static string SaveProcess(StringBuilder data, string fileName)
//        //{
//        //    System.DateTime currentTime = System.DateTime.Now;
//        //    //获取当前日期的前一天转换成ToFileTime
//        //    string strYMD = currentTime.AddDays(-1).ToString("yyyyMMdd");
//        //    //按照日期建立一个文件名
//        //    string FileName = "Setting_" + fileName + ".json";
//        //    //设置目录
//        //    string CurDir = System.AppDomain.CurrentDomain.BaseDirectory + @"SaveDir/";
//        //    //判断路径是否存在
//        //    if (!System.IO.Directory.Exists(CurDir))
//        //    {
//        //        System.IO.Directory.CreateDirectory(CurDir);
//        //    }
//        //    //不存在就创建
//        //    string FilePath = CurDir + FileName;
//        //    //文件覆盖方式添加内容
//        //    System.IO.StreamWriter file = new System.IO.StreamWriter(FilePath, false);
//        //    //保存数据到文件
//        //    file.Write(data);
//        //    //关闭文件
//        //    file.Close();
//        //    //释放对象
//        //    file.Dispose();
//        //    return FilePath;
//        //}
//    }
//}
