using ArenaShooter.Combat.Utils;
using Bolt;

namespace ArenaShooter.Combat
{

    class ElectricWeapon : Weapon
    {

        #region Private variables

        private ElectricShot electricShot;

        #endregion

        protected override void OnInitialized()
        {
            electricShot = Instantiate(Stats.FirePrefab, transform).GetComponent<ElectricShot>();
            electricShot.Initialize(this);
        }

        protected override void OnFire()
        {
            var targets = electricShot.GetTargets();

            for (int i = 0; i < targets.Count; i++)
            {
                var fireEvent     = WeaponFireEffectEvent.Create(WeaponHolder.entity, EntityTargets.EveryoneExceptOwner);
                fireEvent.Shooter = WeaponHolder.entity;
                fireEvent.Point   = i == 0 ? WeaponHolder.WeaponFirePosition : targets[i - 1].BodyOriginPosition;
                fireEvent.Forward = targets[i].BodyOriginPosition;
                fireEvent.Send();

                OnEvent(fireEvent);

                var takeDamageEvent         = TakeDamageEvent.Create(GlobalTargets.Everyone, ReliabilityModes.ReliableOrdered);
                takeDamageEvent.Target      = targets[i].entity;
                takeDamageEvent.DamageTaken = Damage;
                takeDamageEvent.Send();
            }
        }

        public override void OnEvent(WeaponFireEffectEvent @event)
        {
            electricShot.OnEvent(@event);
        }

    }

}
