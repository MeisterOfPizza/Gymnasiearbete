using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                BeforeAwake();
                OnAwake();
            }
        }

        protected abstract void BeforeAwake();

        protected virtual void OnAwake()
        {
            // Leave blank.
        }

    }

}
