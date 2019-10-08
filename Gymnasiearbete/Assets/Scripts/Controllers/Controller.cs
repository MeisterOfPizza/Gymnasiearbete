namespace ArenaShooter.Controllers
{

    /// <summary>
    /// Controller that exists on each client.
    /// </summary>
    abstract class Controller<T> : MonoController where T : Controller<T>
    {

        #region Public static properties

        public static T Singleton { get; private set; }

        #endregion

        protected override bool BeforeAwake()
        {
            if (HasPersistentAttribute<T>())
            {
                if (Singleton != null)
                {
                    Destroy(this);

                    return false;
                }
                else
                {
                    DontDestroyOnLoad(gameObject);
                }
            }

            Singleton = (T)this;

            return true;
        }

        protected virtual void OnDestroy()
        {
            if (Singleton == this)
            {
                Singleton = null;
            }
        }

    }

}
