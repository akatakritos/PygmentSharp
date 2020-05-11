using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PygmentSharp.UnitTests
{
    public static class SampleFile
    {
        public static string Load(string filename)
        {
            var fullPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(),
                "../../Fixtures/", filename));

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Could not load fixture file {fullPath}");

            return File.ReadAllText(fullPath);
        }
    }
}
