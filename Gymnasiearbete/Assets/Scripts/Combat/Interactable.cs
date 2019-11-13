using UnityEngine;
using Bolt;
using ArenaShooter.Player;
using ArenaShooter.Templates.Interactable;
using System.Collections;
using UnityEngine.UI;
using ArenaShooter.UI;
using ArenaShooter.Controllers;

namespace ArenaShooter.Combat.Pickup
{
    [RequireComponent(typeof(BoltEntity))]
    class Interactable : EntityEventListener<IInteractableState>
    {
        #region Editor

        [SerializeField] private InteractableTemplate template;
        [SerializeField] private GameObject           model;
        [SerializeField] private MeshRenderer         meshRenderer;
        [SerializeField] private GameObject           circlePosition3D;
        [SerializeField] private GameObject           uiInteractablePrefab;

        #endregion

        #region Private Variables

        private bool isAvailable       = true;

        private float degreesPerSecond = 15f;
        private float amplitude        = 0.5f;
        private float frequency        = 1f;

        private Vector3 posOffset;
        private Vector3 tempPos;
        

        private UIInteractable cooldownCircle;

        #endregion

        #region Methods

        private void OnTriggerEnter(Collider other)
        {
            if (entity.IsOwner && other.GetComponentInParent<PlayerController>() is PlayerController player && isAvailable)
            {
                template.Interact(player);
                var interactableEvent = InteractableInteractEvent.Create(entity, EntityTargets.Everyone);
                interactableEvent.IsFading = true;
                interactableEvent.Send();
            }
        }

        private void Start()
        {
            cooldownCircle = Instantiate(uiInteractablePrefab, InteractableController.Singleton.Container).GetComponent<UIInteractable>();
            posOffset                   = model.transform.position;
        }

        public override void OnEvent(InteractableInteractEvent evnt)
        {
            if (evnt.IsFading)
            {
                isAvailable = false;
                StartCoroutine("FadeOutEffect");
                StartCoroutine("SpawnCooldown");
                
            }
        }

        private void Update()
        {
            model.transform.Rotate(0f, Time.deltaTime * degreesPerSecond, 0f, Space.World);

            tempPos    = posOffset;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            model.transform.position = tempPos;

            cooldownCircle.gameObject.transform.position = Camera.main.WorldToScreenPoint(circlePosition3D.transform.position);
          
        }


        #endregion

        #region Coroutines

        IEnumerator SpawnCooldown()
        {
            cooldownCircle.gameObject.SetActive(true);
            float originalTime = template.spawnCooldown;
            float time         = template.spawnCooldown;
            while (time > 0)
            {
                time               -= Time.deltaTime;
                state.SpawnCooldown = time;

                cooldownCircle.CountDown(1 - time / originalTime);

                yield return new WaitForEndOfFrame();
            }

            isAvailable = true;
            model.SetActive(true);
            StartCoroutine("FadeInEFfect"); 
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
            cooldownCircle.gameObject.SetActive(false);
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