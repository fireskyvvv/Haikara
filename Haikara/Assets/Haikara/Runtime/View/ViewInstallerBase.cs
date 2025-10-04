namespace Haikara.Runtime.View
{
    public abstract class ViewInstallerBase<T> where T : ViewInstallerBase<T>, new()
    {
        public static void Install()
        {
            new T().InstallInternal();
        }

        protected virtual void InstallInternal()
        {
        }
    }
}