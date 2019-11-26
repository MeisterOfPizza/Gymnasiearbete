using ArenaShooter.Combat.Utils;

#pragma warning disable 0649

namespace ArenaShooter.Combat
{

    class SupportWeapon : Weapon
    {

        #region Private variables
        
        private SupportShot supportShot;

        #endregion

        #region Protected properties

        protected override bool WeaponCanFire
        {
            get
            {
                return supportShot.WeaponCanFire;
            }
        }

        #endregion

        protected override void OnInitialized()
        {
            supportShot = Instantiate(Stats.FirePrefab, transform).GetComponent<SupportShot>();
            supportShot.Initialize(this);
        }

        protected override void WeaponUpdate()
        {
            // Check if the weapon does not belong to the local client, and if the weapon has received a target:
            if (WeaponHolder.entity.IsAttached && !WeaponHolder.entity.IsControllerOrOwner && supportShot.HasValidTargets)
            {
                // If so: update the graphics of the effect.
                supportShot.UpdateSupportShot();
            }
        }

        protected override void OnBeginFire()
        {
            supportShot.OnBeginSupport();
        }

        protected override void OnFireFrame()
        {
            if (supportShot.HasValidTargets)
            {
                supportShot.UpdateSupportShot();
            }
        }

        protected override void OnEndFire()
        {
            supportShot.OnEndSupport();
        }

        protected override void OnFire()
        {
            if (supportShot.HasValidTargets)
            {
                supportShot.SupportTargets();
            }
        }

        public override void OnEvent(WeaponSupportBeginFireEffectEvent @event)
        {
            supportShot.OnEvent(@event);
        }

    }

}
