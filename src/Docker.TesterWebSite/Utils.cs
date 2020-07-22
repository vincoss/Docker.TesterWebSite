using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Docker.TesterWebSite
{
    public class Utils
    {
        public static byte[] ReadResource(string name)
        {
            var path = $"Docker.TesterWebSite.Data.{name}";
            var assembly = typeof(Utils).Assembly;
            using (Stream resFilestream = assembly.GetManifestResourceStream(path))
            {
                if (resFilestream == null) return null;
                byte[] ba = new byte[resFilestream.Length];
                resFilestream.Read(ba, 0, ba.Length);
                return ba;
            }
        }
    }
}
