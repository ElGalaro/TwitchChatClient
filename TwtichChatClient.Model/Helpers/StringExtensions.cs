namespace TwtichChatClient.Model.Helpers
{
    public static class StringExtensions
    {
        public static T TryParse<T>(this string s, TryParseHandler<T> handler) where T : struct
        {
            if (string.IsNullOrEmpty(s))
                return default(T);
            T temp;
            if (handler(s, out temp))
            {
                return temp;
            }
            return default(T);
        }
    }

    public delegate bool TryParseHandler<T>(string value, out T result);
}
