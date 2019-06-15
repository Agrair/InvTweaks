using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;

namespace InvTweaks
{
    public static class Config
    {
        private static readonly FieldInfo[] fields = typeof(Config).GetFields(BindingFlags.Static | BindingFlags.Public);
        private static readonly string path = Path.Combine(Main.SavePath, "InvTweaksConf.json");

        public static bool cursorFill = true;
        public static bool helmetSlot = true;
        // public static bool shopClick
        public static bool lifeCrystalDevour = true;
        // public static bool saplingPlacer;

        public static void Load()
        {
            if (File.Exists(path))
            {
                string[] array = File.ReadAllLines(path);
                for (int i = 0; i < array.Length; i++)
                {
                    string item = array[i];
                    var split = item.Split(':');
                    var key = split[0].Trim();
                    var value = split.Last().Trim();
                    if (fields.Any(x => x.Name == key) && bool.TryParse(value, out bool result))
                    {
                        fields.FirstOrDefault(x => x.Name == key).SetValue(null, result);
                    }
                }
            }
            Write();
        }

        public static void Write()
        {
            var writer = new StreamWriter(File.Create(path));
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo fld = fields[i];
                writer.WriteLine(fld.Name + " : " + ((bool)fld.GetValue(null) ? bool.TrueString : bool.FalseString));
            }
            writer.Close();
        }
    }
}
