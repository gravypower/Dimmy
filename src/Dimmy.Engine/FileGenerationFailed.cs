using System;

namespace Dimmy.Engine
{
    public class FileGenerationFailed : Exception
    {
        public FileGenerationFailed(string error) : base(error)
        {
        }
    }
}