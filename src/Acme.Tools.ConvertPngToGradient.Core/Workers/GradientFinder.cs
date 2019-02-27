// -----------------------------------------------------------------------
//  <copyright file="GradientFinder.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Tools.ConvertPngToGradient.Core.Workers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using ImageMagick;

    /// <summary>
    /// Class that find the gradient in the image
    /// </summary>
    public class GradientFinder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GradientFinder" /> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public GradientFinder(string fileName)
        {
            this.FileName = fileName;
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; }

        /// <summary>
        /// Gets or sets the tolerance of the tool
        /// </summary>
        /// <value>
        /// The tolerance.
        /// </value>
        public int Tolerance { get; set; } = 2;

        /// <summary>
        /// Gets or sets the gradient parts.
        /// </summary>
        /// <value>
        /// The gradient parts.
        /// </value>
        public List<GradientPart> GradientParts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is horizontal.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is horizontal; otherwise, <c>false</c>.
        /// </value>
        public bool IsHorizontal { get; set; }

        /// <summary>
        /// Finds the gradient.
        /// </summary>
        public void FindGradient()
        {
            this.GradientParts = new List<GradientPart>();

            using (var image = new MagickImage(this.FileName))
            {
                // For algo simplicity, threat all images as verticals
                if (this.IsHorizontal)
                {
                    this.RotateImage(image);
                }
                
                var pixels = image.GetPixels();

                var start = pixels.GetPixel(0, 0).ToColor();
                this.AddGradientPart(start, 0);

                var end = pixels.GetPixel(0, image.Height - 1).ToColor();
                var midPoint = pixels.GetPixel(0, image.Height / 2).ToColor();

                // We are lucky, this is a simple gradiant !
                if (this.IsGradient(start, end, midPoint))
                {
                    this.AddGradientPart(end, 100);
                    this.EnsurePartsOrder();
                }
            }
        }

        /// <summary>
        /// Ensures the parts order.
        /// </summary>
        private void EnsurePartsOrder()
        {
            this.GradientParts = this.GradientParts?.OrderBy(x => x.Position).ToList();
        }

        /// <summary>
        /// Adds the gradient part.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="position">The position.</param>
        private void AddGradientPart(MagickColor color, decimal position)
        {
            var gradientPart = new GradientPart
                                   {
                                       Position = position,
                                       Red = color.R,
                                       Blue = color.B,
                                       Green = color.G,
                                       Alpha = color.A
                                   };

            this.GradientParts.Add(gradientPart);
            this.GradientParts = this.GradientParts.OrderBy(x => x.Position).ToList();
        }

        /// <summary>
        /// Determines whether the specified color is gradient.
        /// For a specific range, if the average of the two values are equal to the value of the mid point, then it's a gradient
        /// </summary>
        /// <param name="start">The color.</param>
        /// <param name="end">The end.</param>
        /// <param name="midPoint">The mid point.</param>
        /// <returns>
        ///   <c>true</c> if the specified color is gradient; otherwise, <c>false</c>.
        /// </returns>
        private bool IsGradient(MagickColor start, MagickColor end, MagickColor midPoint)
        {
            var averageRed = (start.R + end.R) / 2;

            if (Math.Abs(averageRed - midPoint.R) > this.Tolerance)
            {
                return false;
            }

            var averageGreen = (start.G + end.G) / 2;

            if (Math.Abs(averageGreen - midPoint.G) > this.Tolerance)
            {
                return false;
            }

            var averageBlue = (start.B + end.B) / 2;

            if (Math.Abs(averageBlue - midPoint.B) > this.Tolerance)
            {
                return false;
            }

            var averageAlpha = (start.A + end.A) / 2;

            if (Math.Abs(averageAlpha - midPoint.A) > this.Tolerance)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Rotates the image.
        /// </summary>
        /// <param name="image">The image.</param>
        private void RotateImage(MagickImage image)
        {
            image.Rotate(90);
        }
    }
}