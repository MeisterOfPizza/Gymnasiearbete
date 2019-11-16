using ArenaShooter.Templates.Enemies;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ArenaShooter.Editor
{

    [CustomEditor(typeof(EnemyTemplate))]
    class EnemyTemplateEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(25f);

            if (GUILayout.Button("Order Weapon Part Item Drops"))
            {
                EnemyTemplate enemyTemplate = target as EnemyTemplate;

                enemyTemplate.weaponPartItemDrops = enemyTemplate.weaponPartItemDrops.OrderBy(t => t.DropChance).ToArray();

                EditorUtility.SetDirty(enemyTemplate);
            }
        }

    }

}
