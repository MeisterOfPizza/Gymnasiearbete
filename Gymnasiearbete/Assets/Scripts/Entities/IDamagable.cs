namespace ArenaShooter.Entities
{

    interface IDamagable
    {

        void TakeDamage(TakeDamageEvent takeDamageEvent);
        void Die();

    }

}
