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

        protected override void BeforeAwake()
        {
            Singleton = (T)this;
        }

    }

}
