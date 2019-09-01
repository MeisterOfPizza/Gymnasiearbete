
using UnityEngine;

namespace ArenaShooter.Templates.Weapons
{
    class Weapon : MonoBehaviour
    {
        BodyTemplate   bodyTemplate;
        StockTemplate  stockTemplate;
        BarrelTemplate barrelTemplate;

        public void Initialize(BodyTemplate bodyTemplate, StockTemplate stockTemplate, BarrelTemplate barrelTemplate)
        {
            this.bodyTemplate = bodyTemplate;
            this.stockTemplate = stockTemplate;
            this.barrelTemplate = barrelTemplate;
        }

      

      
    }
}

