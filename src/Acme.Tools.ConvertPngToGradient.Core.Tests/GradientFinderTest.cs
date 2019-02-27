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
        /// Test a simple gradient.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="isHorizontal">if set to <c>true</c> the image is horizontal.</param>
        [Theory]
        [InlineData("vertical-simple.png", false)]
        [InlineData("horizontal-simple.png", true)]
        public void SimpleGradient(string fileName, bool isHorizontal)
        {
            var fullPath = this.GetFullPath(fileName);
            var gradientFinder = new GradientFinder(fullPath) { IsHorizontal = isHorizontal };

            gradientFinder.FindGradient();

            Assert.Equal(2, gradientFinder.GradientParts.Count);
        }

        /// <summary>
        /// Test a complex gradient.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="isHorizontal">if set to <c>true</c> the image is horizontal.</param>
        [Theory]
        [InlineData("vertical-complex.png", false)]
        [InlineData("horizontal-complex.png", true)]
        public void ComplexGradient(string fileName, bool isHorizontal)
        {
            var fullPath = this.GetFullPath(fileName);
            var gradientFinder = new GradientFinder(fullPath) { IsHorizontal = isHorizontal };

            gradientFinder.FindGradient();

            Assert.True(gradientFinder.GradientParts.Count > 2);
        }

        /// <summary>
        /// When you increase the tolerance, the number of part should decrease
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="isHorizontal">if set to <c>true</c> the image is horizontal.</param>
        /// <param name="minTolerance">The minimum tolerance.</param>
        /// <param name="maxTolerance">The maximum tolerance.</param>
        [Theory]
        [InlineData("vertical-complex.png", false, 2, 10)]
        [InlineData("vertical-complex.png", false, 5, 10)]
        [InlineData("vertical-complex.png", false, 10, 20)]
        [InlineData("horizontal-complex.png", true, 2, 10)]
        [InlineData("horizontal-complex.png", true, 5, 10)]
        [InlineData("horizontal-complex.png", true, 10, 20)]
        public void ComplexGradientMoreToleranceIsLessPart(string fileName, bool isHorizontal, int minTolerance, int maxTolerance)
        {
            var fullPath = this.GetFullPath(fileName);
            var minToleranceGradientFinder = new GradientFinder(fullPath) { IsHorizontal = isHorizontal, Tolerance = minTolerance };
            var maxToleranceGradientFinder = new GradientFinder(fullPath) { IsHorizontal = isHorizontal, Tolerance = maxTolerance };

            minToleranceGradientFinder.FindGradient();
            maxToleranceGradientFinder.FindGradient();

            Assert.True(minToleranceGradientFinder.GradientParts.Count > maxToleranceGradientFinder.GradientParts.Count);
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