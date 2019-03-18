// Copyright (c) 2019 Adam Chappell
// This code is licensed under MIT license (see LICENSE for details)
using SqliteEFScriptGenerator.Core.Configuration;
using SqliteEFScriptGenerator.Core.Generation;
using System;
using System.IO;

namespace SqliteEFScriptGenerator
{
    internal class Program
    {
        /// <summary>
        /// Assembly File to search
        /// </summary>
        public static string AssemblyFilePath
        {
            get;
            set;
        }

        private static void Main(string[] args)
        {
            #region Validation

            // Check config
            if (!SQLiteMappingConfig.ValidateConfig())
            {
                SQLiteMappingConfig.OutputConfigErrorsToConsole();

                Console.WriteLine("Press any key to end program...");
                Console.ReadKey();
                return;
            }

            /*
            // Check arguments
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments specified.");
                return;
            }
            else if (args.Length == 1)
            {
                Console.WriteLine("Missing output path parameter");
                return;
            }*/

            #endregion Validation

            #region Variable Assignment

            //1st argument is expected to be the path of the assembly to investigate
            //AssemblyFilePath = args[0];
            AssemblyFilePath = @".\SQLiteTestAssembly.dll";

            //check the assembly exists
            if (!File.Exists(AssemblyFilePath))
            {
                Console.WriteLine($"File {AssemblyFilePath} does not exist.");
                return;
            }
            /*
            //2nd argument is the output location of the generation script
            string outputPath = args[1];

            //check the directory exists
            if (!Directory.Exists(outputPath))
            {
                Console.WriteLine($"Folder {outputPath} does not exist.");
                return;
            }
            */

            #endregion Variable Assignment

            #region TestCode

            SQLiteGenerator sqLiteGenerator = new SQLiteGenerator(AssemblyFilePath);
            Console.Write(sqLiteGenerator.GenerateNewDatabase());

            #endregion TestCode

            Console.WriteLine("Press any key to end program...");
            Console.ReadKey();
        }
    }
}