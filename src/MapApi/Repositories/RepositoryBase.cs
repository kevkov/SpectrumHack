namespace MapApi.Repositories
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public abstract class RepositoryBase
    {
        protected static List<T> ReadData<T>(string filename)
        {
            var path = GetFilePath(filename);
            var json = File.ReadAllText(path);
            var items = JsonConvert.DeserializeObject<List<T>>(json);

            return items;
        }

        protected static void WriteData<T>(string filename, List<T> items)
        {
            var path = GetFilePath(filename);
            var json = JsonConvert.SerializeObject(items);
            File.WriteAllText(path, json);
        }

        private static string GetFilePath(string filename)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\", filename);
            return path;
        }
    }
}
