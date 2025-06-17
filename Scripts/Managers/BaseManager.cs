using CorePublic.Helpers;

namespace CorePublic.Managers
{
    public class BaseManager<T> : Singleton<T> where T : BaseManager<T>
    {

        protected override void Awake()
        {
            dontDestroyOnLoad = true;
            base.Awake();
        }
    }
}