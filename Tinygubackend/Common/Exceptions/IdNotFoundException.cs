namespace Tinygubackend.Common.Exceptions
{
    [System.Serializable]
    public class IdNotFoundException : System.Exception
    {
        public IdNotFoundException() { }
        public IdNotFoundException(string message) : base(message) { }
        public IdNotFoundException(string message, System.Exception inner) : base(message, inner) { }
        protected IdNotFoundException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}