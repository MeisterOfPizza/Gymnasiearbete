using ArenaShooter.Controllers;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Combat
{

    /// <summary>
    /// Physical weapon part used in the scene.
    /// </summary>
    sealed class WeaponPart : MonoBehaviour
    {

        #region Private static variables

        private static Color HighlightColor = new Color(0.25f, 0.25f, 0.25f, 1f);

        #endregion

        #region Editor

        [Header("References")]
        [SerializeField] private MeshRenderer meshRenderer;

        [Header("Values")]
        [SerializeField] private string[] highlightMaterialNames;
        [SerializeField] private string[] changableMaterialNames;

        [Space]
        [Help(@"Offsets for different weapon parts. If this is the:
Stock, use beginConnectPoint;
Body, use beginConnectPoint AND endConnectPoint;
Barrel, use endConnectPoint.

The beginConnectPoint is forwards and endConnectPoint is backwards.")]
        [SerializeField] private Vector3 beginConnectPoint;
        [SerializeField] private Vector3 endConnectPoint;

        #endregion

        #region Public properties

        public WeaponPartItemWrapper WeaponPartItem
        {
            get
            {
                return weaponPartItem;
            }
        }

        #endregion

        #region Private variables

        private WeaponPartItemWrapper weaponPartItem;
        private bool                  shouldHighlight;

        #endregion

        public void Initialize(WeaponPartItemWrapper weaponPartItem, bool shouldHighlight)
        {
            this.weaponPartItem  = weaponPartItem;
            this.shouldHighlight = shouldHighlight;

            if (shouldHighlight)
            {
                SetupMaterialsForHighlights();
            }
        }

        public void AttachStock(WeaponPart stockPart)
        {
            stockPart.OffsetPart(endConnectPoint);
        }

        public void AttachBarrel(WeaponPart barrelPart)
        {
            barrelPart.OffsetPart(beginConnectPoint);
        }

        private void OffsetPart(Vector3 connectPoint)
        {
            if (weaponPartItem.BaseTemplate.Type == Templates.Weapons.WeaponPartTemplateType.Stock)
            {
                transform.localPosition = beginConnectPoint - transform.localPosition + connectPoint;
            }
            else if (weaponPartItem.BaseTemplate.Type == Templates.Weapons.WeaponPartTemplateType.Barrel)
            {
                transform.localPosition = endConnectPoint - transform.localPosition + connectPoint;
            }
        }

        private void SetupMaterialsForHighlights()
        {
            if (meshRenderer.materials.Length > 1)
            {
                for (int i = 0; i < meshRenderer.materials.Length; i++)
                {
                    for (int j = 0; j < highlightMaterialNames.Length; j++)
                    {
                        if (meshRenderer.materials[i].name == highlightMaterialNames[j])
                        {
                            meshRenderer.materials[i].EnableKeyword("_EMISSION");
                        }
                    }
                }
            }
            else
            {
                meshRenderer.material.EnableKeyword("_EMISSION");
            }
        }

        private void OnMouseEnter()
        {
            if (shouldHighlight)
            {
                if (meshRenderer.materials.Length > 1)
                {
                    for (int i = 0; i < meshRenderer.materials.Length; i++)
                    {
                        for (int j = 0; j < highlightMaterialNames.Length; j++)
                        {
                            if (meshRenderer.materials[i].name == highlightMaterialNames[j])
                            {
                                meshRenderer.materials[i].SetColor("_EmissionColor", HighlightColor);
                            }
                        }
                    }
                }
                else
                {
                    meshRenderer.material.SetColor("_EmissionColor", HighlightColor);
                }
            }
        }

        private void OnMouseDown()
        {
            if (shouldHighlight)
            {
                // TODO: Check if this works on mobile.
                UILoadoutController.Singleton.FocusWeaponPart(this);
            }
        }

        private void OnMouseExit()
        {
            if (shouldHighlight)
            {
                if (meshRenderer.materials.Length > 1)
                {
                    for (int i = 0; i < meshRenderer.materials.Length; i++)
                    {
                        for (int j = 0; j < highlightMaterialNames.Length; j++)
                        {
                            if (meshRenderer.materials[i].name == highlightMaterialNames[j])
                            {
                                meshRenderer.materials[i].SetColor("_EmissionColor", Color.clear);
                            }
                        }
                    }
                }
                else
                {
                    meshRenderer.material.SetColor("_EmissionColor", Color.clear);
                }
            }
        }

    }

}
