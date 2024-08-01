namespace StoreRopa.Data.utils
{
    [Serializable]
    public class CustomException : Exception
    {
        public CustomException() { }
        public CustomException(string message) : base(message) { }
    }
}
