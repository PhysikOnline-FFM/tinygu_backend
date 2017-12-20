[System.Serializable]
public class PropertyIsMissingException : System.Exception
{
    public PropertyIsMissingException() { }
    public PropertyIsMissingException(string message) : base(message) { }
    public PropertyIsMissingException(string message, System.Exception inner) : base(message, inner) { }
    protected PropertyIsMissingException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}