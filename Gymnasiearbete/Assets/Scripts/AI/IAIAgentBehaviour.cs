using ArenaShooter.Entities;
using System.Collections.Generic;

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

        bool                 FilterTarget(IEntity target);
        IEnumerable<IEntity> FilterTargets(IEnumerable<IEntity> targets);
        void                 NoTargetsFound(float searchTimeDelta);

    }

}
