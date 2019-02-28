// -----------------------------------------------------------------------
//  <copyright file="Program.cs" company="Prism">
//  Copyright (c) Prism. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Tools.ConvertPngToGradient
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Acme.Tools.ConvertPngToGradient.Core.Workers;

    using ImageMagick;

    /// <summary>
    /// Main class of the program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The tolerance regex to match the parameter
        /// </summary>
        private static readonly Regex ToleranceRegex = new Regex("--tolerance(\\d+)", RegexOptions.IgnoreCase);

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage : ConvertPngToGradient.exe <filename> <options>");
                Console.WriteLine("<options> can be :");
                Console.WriteLine("--horizontal : the gradient is not vertical, it's horizontal");
                Console.WriteLine("--tolerance<value> : the tolerance to be used, must be between 1 and 255");
                Console.WriteLine("Sample : ConvertPngToGradient.exe mygradiant.png --horizontal --tolerance10");

                return;
            }

            var gradientFinder = new GradientFinder(args[0]);

            for (var index = 1; index < args.Length; index++)
            {
                var argument = args[index];

                if (argument.Equals("--horizontal", StringComparison.InvariantCultureIgnoreCase))
                {
                    gradientFinder.IsHorizontal = true;
                }

                var toleranceMatch = ToleranceRegex.Match(argument);
                if (toleranceMatch.Success)
                {
                    var tolerance = Convert.ToInt32(toleranceMatch.Groups[1].Value);

                    if (tolerance < 0 || tolerance > 255)
                    {
                        Console.WriteLine("Tolerance must be between 1 and 255");
                        return;
                    }

                    gradientFinder.Tolerance = tolerance;
                }
            }

            gradientFinder.FindGradient();
            DumpToConsole(gradientFinder);
        }

        /// <summary>
        /// Dumps to console.
        /// </summary>
        /// <param name="gradientFinder">The gradient finder.</param>
        private static void DumpToConsole(GradientFinder gradientFinder)
        {
            foreach (var part in gradientFinder.GradientParts)
            {
                var magickColor = MagickColor.FromRgba((byte)part.Red, (byte)part.Green, (byte)part.Blue, (byte)part.Alpha);
                Console.WriteLine($"{(part.Position.ToString("00.00", CultureInfo.InvariantCulture) + "%").PadLeft(7)} - {magickColor}");
            }
        }
    }
}