# Find the gradient values from an image
This command line can find the gradient from an image, it outputs the gradient value that can be used in css.
## Usage
This tool is used in command line and take an image as input, all image supported by MagickNet is allowed.

    Usage : ConvertPngToGradient.exe <filename> <options>
    <options> can be
    --horizontal : the gradient is not vertical, it's horizontal
    --tolerance<value> : the tolerance to be used, must be between 1 and 255

Sample : ConvertPngToGradient.exe mygradiant.png --horizontal --tolerance10
## Works with

 - All images supported by MagickNet (best result with PNG)
 - Horizontal and Vertical gradients
 - Multi color gradients

## Credits
C# Tools based on https://github.com/bluesmoon/pngtocss to find the gradient values from an image.