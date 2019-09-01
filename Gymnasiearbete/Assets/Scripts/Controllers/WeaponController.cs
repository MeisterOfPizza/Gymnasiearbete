
using ArenaShooter.Templates.Weapons;
using System.Linq;
using UnityEngine;

namespace ArenaShooter.Controllers
{
    class WeaponController : Controller<WeaponController>
    {
        [SerializeField] private BodyTemplate[]   bodyTemplates;
        [SerializeField] private StockTemplate[]  stockTemplates;
        [SerializeField] private BarrelTemplate[] barrelTemplates;

        public StockTemplate GetStockTemplate(ushort Id)
        {
            return stockTemplates.FirstOrDefault(T => T.TemplateId == Id);
        }

        public BarrelTemplate GetBarrelTemplate(ushort Id)
        {
            return barrelTemplates.FirstOrDefault(T => T.TemplateId == Id);
        }

        public BodyTemplate GetBodyTemplate(ushort Id)
        {
            return bodyTemplates.FirstOrDefault(T => T.TemplateId == Id);
        }


    }
}

