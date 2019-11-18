using ArenaShooter.Entities;
using ArenaShooter.Extensions;
using Bolt;
using System.Collections;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Combat.Utils
{

    class RocketProjectileShot : ProjectileShot
    {

        #region Editor

        [Header("References")]
        [SerializeField] private GameObject model;

        [Space]
        [SerializeField] private ParticleSystem onInitializeEffect;
        [SerializeField] private ParticleSystem onHitEffect;
        [SerializeField] private ParticleSystem passiveEffect;

        [Space]
        [SerializeField] private new Rigidbody rigidbody;

        [Header("Values")]
        [SerializeField] private float flySpeed = 3.0f;

        #endregion

        #region Private variables

        private bool isAlive;
        private bool clientIsShooter;

        private Vector3 origin;

        #endregion

        public override void FireProjectile(bool clientIsShooter)
        {
            isAlive = true;

            this.clientIsShooter = clientIsShooter;

            if (onInitializeEffect != null)
            {
                onInitializeEffect.Play(true);
            }

            if (passiveEffect != null)
            {
                passiveEffect.Play(true);
            }

            if (onHitEffect != null)
            {
                onHitEffect.Clear(true);
                onHitEffect.Stop(true);
            }

            model.SetActive(true);

            StopAllCoroutines();

            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(transform.forward * flySpeed, ForceMode.Force);

            origin = transform.position;
        }

        private void Hit()
        {
            if (clientIsShooter)
            {
                using (var hits = BoltNetwork.OverlapSphereAll(transform.position, weapon.Stats.Range, BoltNetwork.ServerFrame))
                {
                    for (int i = 0; i < hits.count; i++)
                    {
                        var hit = hits.GetHit(i);

                        if (weapon.WeaponHolder.WeaponHitLayerMask.HasLayer(hit.body.gameObject.layer))
                        {
                            var takeDamageEvent         = TakeDamageEvent.Create(GlobalTargets.Everyone, ReliabilityModes.ReliableOrdered);
                            takeDamageEvent.Target      = hit.body.GetComponent<IEntity>().entity;
                            takeDamageEvent.DamageTaken = weapon.CalculateDamage(hit.distance);
                            takeDamageEvent.Send();
                        }
                    }
                }
            }

            isAlive = false;

            if (passiveEffect != null)
            {
                passiveEffect.Stop();
            }

            // Check if the onHitEffect needs to be played before this projectile can be pooled again.
            // Else, pool it instantly.
            if (onHitEffect != null)
            {
                onHitEffect.Play();

                model.SetActive(false);
                StartCoroutine("WaitForOnHitEffect");
            }
            else
            {
                weapon.ProjectileHit(this);
            }

            rigidbody.velocity = Vector3.zero;
        }

        private void FixedUpdate()
        {
            if (Vector3.Distance(origin, transform.position) > weapon.Stats.MaxDistance && isAlive)
            {
                Hit();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (weapon.WeaponHolder.WeaponHitLayerMask.HasLayer(other.gameObject.gameObject.layer) && isAlive)
            {
                Hit();
            }
        }

        /// <summary>
        /// Waits for the <see cref="onHitEffect"/> to stop playing before pooling this projectile.
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitForOnHitEffect()
        {
            while (onHitEffect.isPlaying)
            {
                yield return new WaitForEndOfFrame();
            }

            weapon.ProjectileHit(this);
        }

    }

}
