using ArenaShooter.Entities;

namespace ArenaShooter.AI
{

    interface IAIAgentBehaviour : IEntity
    {

        EntityTeam SearchTargetTeam { get; }
        float      SearchInterval   { get; }
        float      SearchThreshold  { get; }
        float      StoppingDistance { get; }
        float      MovementSpeed    { get; }
        float      TurnSpeed        { get; }
        Body       Body             { get; }

    }

}
