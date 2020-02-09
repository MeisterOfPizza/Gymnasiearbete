namespace ArenaShooter.Entities
{

    enum HealableBy : byte
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
