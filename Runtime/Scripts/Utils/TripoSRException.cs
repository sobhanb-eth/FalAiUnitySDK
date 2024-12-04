// Runtime/Scripts/Utils/TripoSRException.cs

using System;

namespace TripoSR.SDK
{
    public class TripoSRException : Exception
    {
        public TripoSRException(string message) : base(message) { }
    }
}
