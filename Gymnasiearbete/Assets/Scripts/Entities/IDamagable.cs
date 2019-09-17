namespace ArenaShooter.Entities
{

    interface IDamagable
    {

        void TakeDamage(TakeDamageEvent takeDamageEvent);
        void Revive(EntityRevivedEvent @event);
        void Die(EntityDiedEvent @event);

    }

}
