using System;

namespace ImageCollection.Exceptions
{
    internal class UnrecognizedImageFormatException : Exception
    {
        public UnrecognizedImageFormatException() : base("Image format not recognized") { }
    }
}