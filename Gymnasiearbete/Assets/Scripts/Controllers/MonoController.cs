using ArenaShooter.Extensions.Attributes;
using System;
using UnityEngine;

namespace ArenaShooter.Controllers
{

    /// <summary>
    /// Base class for all controllers.
    /// </summary>
    abstract class MonoController : MonoBehaviour
    {

        #region Private variables

        private bool awakeCalled;

        #endregion

        private void Awake()
        {
            if (!awakeCalled)
            {
                awakeCalled = true;

                if (BeforeAwake())
                {
                    OnAwake();
                }
            }
        }

        protected abstract bool BeforeAwake();

        protected virtual void OnAwake()
        {
            // Leave blank.
        }

        protected bool HasPersistentAttribute<T>() where T : MonoController
        {
            return Attribute.GetCustomAttribute(typeof(T), typeof(PersistentAttribute)) != null;
        }

    }

}
