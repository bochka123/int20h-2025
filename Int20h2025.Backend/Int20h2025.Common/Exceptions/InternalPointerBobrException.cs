﻿namespace Int20h2025.Common.Exceptions
{
    public class InternalPointerBobrException : Exception
    {
        public InternalPointerBobrException() { }

        public InternalPointerBobrException(string message) : base(message) { }

        public InternalPointerBobrException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
