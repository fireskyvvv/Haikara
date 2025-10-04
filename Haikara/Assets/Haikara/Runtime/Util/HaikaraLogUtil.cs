namespace Haikara.Runtime.Util
{
    public static class HaikaraLogUtil
    {
        private const string Prefix = "[Haikara] ";
        public static string GetMessage(object message)
        {
            return $"{Prefix}{message}";
        }
    }
}