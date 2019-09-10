namespace ArenaShooter.Entities
{

    enum HealableBy
    {
        None,
        Player,
        Enemy,
        Both
    }

    interface IHealable
    {

        HealableBy HealableBy { get; }

        void Heal(HealEvent healEvent);

    }

}
