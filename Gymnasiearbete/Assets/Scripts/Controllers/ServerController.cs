namespace ArenaShooter.Controllers
{

    /// <summary>
    /// Controller that only exists on the server (or host).
    /// </summary>
    abstract class ServerController<T> : MonoController where T : ServerController<T>
    {

        #region Public static properties

        public static T Singleton { get; private set; }

        #endregion

        protected override bool BeforeAwake()
        {
            if (!BoltNetwork.IsServer)
            {
                Destroy(this);

                return false;
            }
            else
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
