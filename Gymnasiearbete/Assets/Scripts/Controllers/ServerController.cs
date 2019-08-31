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

        protected override void BeforeAwake()
        {
            if (!BoltNetwork.IsServer)
            {
                Destroy(this);
            }
            else
            {
                Singleton = (T)this;
            }
        }

    }

}
