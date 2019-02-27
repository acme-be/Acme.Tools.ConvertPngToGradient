// -----------------------------------------------------------------------
//  <copyright file="Program.cs" company="Prism">
//  Copyright (c) Prism. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Tools.ConvertPngToGradient
{
    using System;
    using System.Linq;

    using Acme.Tools.ConvertPngToGradient.Core.Workers;

    /// <summary>
    /// Main class of the program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage : ConvertPngToGradient.exe <filename> <options>");
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
            }

            gradientFinder.FindGradient();
        }
    }
}