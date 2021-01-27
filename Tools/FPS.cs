using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Tools
{
    /// <summary>
    /// 文件校验附加模式
    /// 创建，删除，纯校验
    /// </summary>
    public enum FpsModel
    {
        Create,
        Delete,
        Default
    }
    /// <summary>
    /// 文件系统
    /// </summary>
    class FPS
    {
        /// <summary>
        /// 存放资源的根目录路径
        /// </summary>
        public static string ResourceRootPath => AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/");
        /// <summary>
        /// 从根目录开始添加路径，只针对输出路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string Combine(string path) => $"{AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/")}{path}";
        /// <summary>
        /// 目录校验
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="fpsModel"></param>
        /// <returns></returns>
        public static bool CheckDirectory(string dir, FpsModel fpsModel)
        {
            bool result = false;
            try
            {
                result = Directory.Exists(dir);
                switch (fpsModel)
                {
                    case FpsModel.Default:
                        break;
                    case FpsModel.Create:
                        if (!result)
                        {
                            Console.WriteLine($"Directory not exist, do create it.");
                            Directory.CreateDirectory(dir);
                        }
                        break;
                    case FpsModel.Delete:
                        if (result)
                        {
                            Console.WriteLine($"Directory exist, do delete it.");
                            Directory.Delete(dir);
                        }
                        break;
                }
                result = Directory.Exists(dir);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return result;
        }
        /// <summary>
        /// 文件校验
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fpsModel"></param>
        /// <returns></returns>
        public static bool CheckFile(string path, FpsModel fpsModel)
        {
            bool result = false;
            try
            {
                result = File.Exists(path);
                switch (fpsModel)
                {
                    case FpsModel.Default:
                        break;
                    case FpsModel.Create:
                        if (!result)
                        {
                            Console.WriteLine($"File not exist, do create it.");
                            File.Create(path);
                        }
                        break;
                    case FpsModel.Delete:
                        if (result)
                        {
                            Console.WriteLine($"File exist, do delete it.");
                            File.Delete(path);
                        }
                        break;
                }
                result = File.Exists(path);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            return result;
        }
        /// <summary>
        /// 复制文件到指定目录下，若存在则替换
        /// </summary>
        /// <param name="orgPath"></param>
        /// <param name="newDir"></param>
        /// <param name="newPath"></param>
        public static void CopyFile(string srcPath, string aimDir, string aimPath)
        {
            if (CheckFile(srcPath, FpsModel.Default))
            {
                CheckDirectory(aimDir, FpsModel.Create);
                CheckFile(aimPath, FpsModel.Create);
                File.Copy(srcPath, aimPath, true);
            }
        }
        /// <summary>
        /// 将文件移动到另一个目录下，如果参数不同将会修改文件类型,如果对面已有文件，将会替换
        /// 如果目标文件存在，会将其删除
        /// </summary>
        /// <param name="orgPath"></param>
        /// <param name="newPath"></param>
        public static void MoveFile(string srcPath, string aimPath)
        {
            if (CheckFile(srcPath, FpsModel.Default))
            {
                CheckFile(aimPath, FpsModel.Delete);
                File.Move(srcPath, aimPath);
            }
        }

        /// <summary>
        /// 这是一个写文件的静态工具类
        /// 进行文件写操作，直接与文件接触
        /// </summary>
        public static class FileWriter
        {
            /// <summary>
            /// 文件写入操作，支持三种类型，string, byte[], stream
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="path"></param>
            /// <param name="contents"></param>
            public static void Write<T>(string path, T contents)
            {
                if (string.IsNullOrEmpty(path) || !CheckFile(path, FpsModel.Create)) return;
                switch (typeof(T).Name)
                {
                    case "String":
                        WriteStart(path, contents as string);
                        break;
                    case "Byte[]":
                        WriteStart(path, contents as byte[]);
                        break;
                    case "Stream":
                        WriteStart(path, contents as Stream);
                        break;
                }
            }
            /// <summary>
            /// 写入string
            /// </summary>
            /// <param name="path"></param>
            /// <param name="contents"></param>
            private static void WriteStart(string path, string contents)
            {
                try
                {
                    using (StreamWriter fw = new StreamWriter(path, false, Encoding.UTF8))
                    {
                        fw.Write(contents);
                        fw.Flush();
                        fw.Dispose();
                    }
                }
                catch { }
            }
            /// <summary>
            /// 写入byte[]
            /// </summary>
            /// <param name="path"></param>
            /// <param name="contents"></param>
            private static void WriteStart(string path, byte[] contents)
            {
                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        fs.Write(contents, 0, contents.Length);
                        fs.Flush();
                        fs.Close();
                    }
                }
                catch { }
            }
            /// <summary>
            /// stream流的方式写入
            /// </summary>
            /// <param name="path"></param>
            /// <param name="stream"></param>
            private static void WriteStart(string path, Stream stream)
            {
                try
                {
                    FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                    byte[] buffer = new byte[512];
                    int length = stream.Read(buffer, 0, buffer.Length);
                    while (length > 0)
                    {
                        fs.Write(buffer, 0, length);
                        buffer = new byte[512];
                        length = stream.Read(buffer, 0, buffer.Length);
                    }
                    fs.Flush();
                    fs.Close();

                }
                catch { }
            }
        }

        /// <summary>
        /// 这是一个读文件的静态工具类
        /// 进行文件读操作，直接与文件接触
        /// </summary>
        public static class FileReader
        {
            /// <summary>
            /// 读取文件，返回内容，目前支持两种方式 string byte[]
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="path">file path</param>
            /// <param name="contents">results type = string or byte[]</param>
            public static void Read<T>(string path, out T contents) where T : class
            {
                contents = default;
                if (string.IsNullOrEmpty(path) || !CheckFile(path, FpsModel.Default)) return;
                switch (typeof(T).Name)
                {
                    case "String":
                        ReadStart(path, out string resultString);
                        contents = resultString as T;
                        break;
                    case "Byte[]":
                        ReadStart(path, out byte[] resultBytes);
                        contents = resultBytes as T;
                        break;
                }
            }
            /// <summary>
            /// 读取到字符串
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            private static void ReadStart(string path, out string contents)
            {
                contents = default;
                try
                {
                    using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
                    {
                        contents = sr.ReadToEnd();
                        sr.Close();
                    }
                }
                catch { }
            }
            /// <summary>
            /// 读取到缓存区
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            private static void ReadStart(string path, out byte[] contents)
            {
                contents = default;
                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        contents = new byte[fs.Length];
                        fs.Read(contents, 0, (int)fs.Length);
                    }
                }
                catch { }
            }
        }
    }
}
