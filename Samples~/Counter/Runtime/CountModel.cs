namespace Haikara.Sample.Counter
{
    public class CountModel
    {
        public int currentCount = 0;
        public string Label => $"CurrentCount:{currentCount}";

        public void DebugLogCurrentCount()
        {
            UnityEngine.Debug.Log(currentCount);
        }
    }
}