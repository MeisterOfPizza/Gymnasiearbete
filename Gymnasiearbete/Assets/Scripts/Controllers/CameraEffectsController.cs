using System.Collections;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class CameraEffectsController : Controller<CameraEffectsController>
    {

        private Camera effectsCamera;

        public void SetEffectsCamera(Camera effectsCamera)
        {
            this.effectsCamera = effectsCamera;
        }

        #region Effects

        public void PlayShakeCamera(float duration, float magnitude)
        {
            if (effectsCamera != null)
            {
                IEnumerator coroutine = ShakeCamera(duration, magnitude);

                StartCoroutine(coroutine);
            }
        }

        public void PlayFastZoom(float duration, float magnitude)
        {
            if (effectsCamera != null)
            {
                IEnumerator coroutine = FastZoom(duration, magnitude);

                StartCoroutine(coroutine);
            }
        }

        #endregion

        #region Effect enumerators

        private IEnumerator ShakeCamera(float duration, float magnitude)
        {
            Vector3 originalPos = effectsCamera.transform.localPosition;
            float   elapsed     = 0f;

            while (elapsed < duration)
            {
                float f = Mathf.Sin(elapsed * Mathf.PI / duration); // Factor (scale)
                float x = Random.Range(-magnitude, magnitude) * f;
                float y = Random.Range(-magnitude, magnitude) * f;

                effectsCamera.transform.localPosition = new Vector3(x, y, originalPos.z);

                elapsed += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            effectsCamera.transform.localPosition = originalPos;
        }

        private IEnumerator FastZoom(float duration, float magnitude)
        {
            Vector3 originalPos  = effectsCamera.transform.localPosition;
            float   elapsed      = 0f;
            float   originalSize = effectsCamera.orthographic ? effectsCamera.orthographicSize : effectsCamera.fieldOfView;

            while (elapsed < duration)
            {
                float z    = originalPos.z + Mathf.Sin(elapsed * Mathf.PI / duration) * magnitude;
                float size = originalSize + Mathf.Sin(elapsed * Mathf.PI / duration) * magnitude;

                effectsCamera.transform.localPosition = new Vector3(originalPos.x, originalPos.y, z);

                if (effectsCamera.orthographic)
                    effectsCamera.orthographicSize = originalSize;
                else
                    effectsCamera.fieldOfView = originalSize;

                elapsed += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            effectsCamera.transform.localPosition = originalPos;

            if (effectsCamera.orthographic)
                effectsCamera.orthographicSize = originalSize;
            else
                effectsCamera.fieldOfView = originalSize;
        }

        #endregion

    }

}
