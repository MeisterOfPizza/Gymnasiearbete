using UnityEngine;

namespace ArenaShooter.Extensions
{

    static class Utils
    {

        #region Layers

        public static bool HasLayer(this LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }

        #endregion

    }

}
