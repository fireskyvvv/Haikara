namespace Haikara.Runtime.Util
{
    public abstract class SimpleSingleton<T> where T : SimpleSingleton<T>, new()
    {
        private static T? _instance;
        public static T Instance => _instance ??= new T();
    }
}