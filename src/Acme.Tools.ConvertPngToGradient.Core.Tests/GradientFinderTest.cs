// -----------------------------------------------------------------------
//  <copyright file="GradientFinderTest.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Tools.ConvertPngToGradient.Core.Tests
{
    using System;
    using System.IO;
    using System.Linq;

    using Acme.Tools.ConvertPngToGradient.Core.Workers;

    using Xunit;

    /// <summary>
    /// Test the gradient finder
    /// </summary>
    public class GradientFinderTest
    {
        /// <summary>
        /// Simples the horizontal gradient.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="isHorizontal">if set to <c>true</c> the image is horizontal.</param>
        [Theory]
        [InlineData("vertical-simple.png", false)]
        [InlineData("horizontal-simple.png", true)]
        public void SimpleHorizontalGradient(string fileName, bool isHorizontal)
        {
            var fullPath = this.GetFullPath(fileName);
            var gradientFinder = new GradientFinder(fullPath) { IsHorizontal = isHorizontal };

            gradientFinder.FindGradient();

            Assert.Equal(2, gradientFinder.GradientParts.Count);
        }

        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The full path</returns>
        /// <exception cref="System.NotSupportedException">This filename {fileName} is not found</exception>
        private string GetFullPath(string fileName)
        {
            var filePath = Path.Combine(new FileInfo(typeof(GradientFinderTest).Assembly.Location).Directory?.FullName, "Samples", fileName);

            if (!File.Exists(filePath))
            {
                throw new NotSupportedException($"This filename {fileName} is not found");
            }

            return filePath;
        }
    }
}