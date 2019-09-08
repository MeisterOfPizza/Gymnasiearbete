using ArenaShooter.Templates.Enemies;
using System.Linq;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class EnemyTemplateController : Controller<EnemyTemplateController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private EnemyTemplate[] enemyTemplates;

        #endregion

        public EnemyTemplate GetEnemyTemplate(ushort id)
        {
            return enemyTemplates.FirstOrDefault(t => t.TemplateId == id);
        }

    }

}
