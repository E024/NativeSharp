﻿// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
//
namespace Il2Native
{
    using System;
    using System.IO;
    using System.Linq;
    using Logic;

    /// <summary>
    /// </summary>
    public class Program
    {
        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        private static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("NativeSharp, https://github.com/0xFireball/NativeSharp");
                Console.WriteLine("Based on Cs2Cpp, https://github.com/ASDAlexander77/cs2cpp.git");
                Console.WriteLine("C# to C++ transpiler");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Usage: CS2CPP [options] file");
                Console.WriteLine(string.Empty);
                Console.WriteLine("file:                     Specifies the file or files to be compiled");
                Console.WriteLine("  .cs                     C# source file");
                Console.WriteLine("  .csproj                 C# project file");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Options:");
                Console.WriteLine("  /release                Release mode");
                Console.WriteLine("  /corelib:<file>         Reference standard library (CoreLib.dll)");
                Console.WriteLine("  /ref:<file|assembly>[;<file|assembly>..]");
                Console.WriteLine("                          Reference assembly by name or file");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Example:");
                Console.WriteLine("  Il2C file1.cs          Compiles one C# file");
                Console.WriteLine("  Il2C proj1.csproj      Compiles C# project");
                Console.WriteLine("  Il2C /ref:System.Core file1.cs file2.cs");
                Console.WriteLine("                          Compiles two C# files using Roslyn compiler");
                return 0;
            }

            var processedArgs =
                args.Select(arg => (arg.StartsWith("/", StringComparison.InvariantCulture) || arg.StartsWith("-", StringComparison.InvariantCulture)) ? arg.Substring(1) : arg).ToArray();
            var sources = args.Where(arg => (!arg.StartsWith("/", StringComparison.InvariantCulture) && !arg.StartsWith("-", StringComparison.InvariantCulture))).ToArray();

            var fileExtension = Path.GetExtension(sources.First());
            if (!sources.All(f => Path.GetExtension(f).Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase)))
            {
                Console.WriteLine("WARNING!");
                Console.WriteLine("You can use only one type of files at a time.");
                return 1;
            }

            if (fileExtension.Equals("csproj", StringComparison.InvariantCultureIgnoreCase) &&
                sources.Count() > 1)
            {
                Console.WriteLine("WARNING!");
                Console.WriteLine("You can use only one CSPROJ file at a time.");
                return 1;
            }

            Console.WriteLine("Building target \"Generate C++ files\"...");
            Il2Converter.Convert(sources, Environment.CurrentDirectory, processedArgs);
            Console.WriteLine("Done.");

            return 0;
        }
    }
}