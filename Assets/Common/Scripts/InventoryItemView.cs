using System;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Services.Samples
{
    public class InventoryItemView : MonoBehaviour
    {
        public Sprite swordSprite;
        public Sprite shieldSprite;

        [Header("Tarality")]
        public Sprite gandivaGuard;
        public Sprite archersAegis;
        public Sprite dhananjyaDefender;
        public Sprite indrasEnmbrace;

        Image m_IconImage;

        void Awake()
        {
            m_IconImage = GetComponentInChildren<Image>();
        }

        public void SetKey(string key)
        {
            switch (key)
            {
                case "SWORD":
                    m_IconImage.sprite = swordSprite;
                    break;

                case "SHIELD":
                    m_IconImage.sprite = shieldSprite;
                    break;

                case "GANDIVA_GUARD":
                    m_IconImage.sprite = gandivaGuard;
                    break;

                case "ARCHERS_AEGIS":
                    m_IconImage.sprite = archersAegis;
                    break;

                case "DHANANJAYA_DEFENDER":
                    m_IconImage.sprite = dhananjyaDefender;
                    break;

                case "INDRAS_EMBRACE":
                    m_IconImage.sprite = indrasEnmbrace;
                    break;

                default:
                    m_IconImage.sprite = null;
                    break;
            }
        }
    }
}
