using UnityEngine;
using Bolt;
using ArenaShooter.Player;
using ArenaShooter.Templates.Interactable;
using System.Collections;
using UnityEngine.UI;

namespace ArenaShooter.Combat.Pickup
{
    [RequireComponent(typeof(BoltEntity))]
    class Interactable : EntityEventListener<IInteractableState>
    {
        #region Editor

        [SerializeField] private InteractableTemplate template;
        [SerializeField] private GameObject           model;
        [SerializeField] private MeshRenderer         meshRenderer;
        [SerializeField] private Image                circleCooldownImage;
        [SerializeField] private GameObject           circlePosition3D;

        #endregion

        #region Private Variables

        private bool isAvailable       = true;

        private float degreesPerSecond = 15f;
        private float amplitude        = 0.5f;
        private float frequency        = 1f;

        private Vector3 posOffset;
        private Vector3 tempPos;
        private Vector3 imagePosition3D;

        #endregion

        #region Methods

        private void OnTriggerEnter(Collider other)
        {
            if (entity.IsOwner && other.GetComponentInParent<PlayerController>() is PlayerController player && isAvailable)
            {
                template.Interact(player);
                isAvailable = false;
                StartCoroutine("FadeOutEffect");
                StartCoroutine("SpawnCooldown");
                circleCooldownImage.enabled = true;
                circleCooldownImage.fillAmount = 0f;
            }
        }

        private void Start()
        {
            posOffset                   = model.transform.position;
            imagePosition3D             = circlePosition3D.transform.position;
            circleCooldownImage.enabled = false;
        }

        private void Update()
        {
            model.transform.Rotate(0f, Time.deltaTime * degreesPerSecond, 0f, Space.World);

            tempPos    = posOffset;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            model.transform.position = tempPos;

            circleCooldownImage.gameObject.transform.position = Camera.main.WorldToScreenPoint(imagePosition3D);
        }

        #endregion

        #region Coroutines

        IEnumerator SpawnCooldown()
        {
            float originalTime = template.spawnCooldown;
            float time         = template.spawnCooldown;
            while (time > 0)
            {
                time               -= Time.deltaTime;
                state.SpawnCooldown = time;

                circleCooldownImage.fillAmount = Mathf.Clamp01((originalTime - time) / originalTime);

                yield return new WaitForEndOfFrame();
            }

            isAvailable = true;
            model.SetActive(true);
            StartCoroutine("FadeInEFfect");
            circleCooldownImage.enabled = false;
        }

        IEnumerator FadeOutEffect()
        {
            float time = 1f;
            while (time > 0)
            {
                time -= Time.deltaTime;
                for (int i = 0; i < meshRenderer.materials.Length; i++)
                {
                    Color color                     = meshRenderer.materials[i].color;
                    color.a                         = time;
                    meshRenderer.materials[i].color = color;
                }

                yield return new WaitForEndOfFrame();
            }
            model.SetActive(false);
        }

        IEnumerator FadeInEFfect()
        {
            float time = 0f;
            while (time < 1)
            {
                time += Time.deltaTime;
                for (int i = 0; i < meshRenderer.materials.Length; i++)
                {
                    Color color                     = meshRenderer.materials[i].color;
                    color.a                         = time;
                    meshRenderer.materials[i].color = color;
                }

                yield return new WaitForEndOfFrame();
            }

        }

        #endregion

    }

}