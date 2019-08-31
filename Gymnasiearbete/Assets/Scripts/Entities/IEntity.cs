namespace ArenaShooter.Entities
{

    interface IEntity : IDamagable
    {

        BoltEntity entity { get; set; }

    }

}
