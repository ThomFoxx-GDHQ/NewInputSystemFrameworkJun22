using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        #region Singleton
        private static UIManager _instance;
        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("UI Manager is NULL.");

                return _instance;
            }
        }
        #endregion

        [SerializeField]
        private Text _interactableZone;
        [SerializeField]
        private Image _inventoryDisplay;
        [SerializeField]
        private RawImage _droneCamView;

        private void Awake()
        {
            _instance = this;
        }

        public void DisplayInteractableZoneMessage(bool showMessage, string message = null)
        {
            _interactableZone.text = message;
            _interactableZone.gameObject.SetActive(showMessage);
        }

        public void UpdateInventoryDisplay(Sprite icon)
        {            
            _inventoryDisplay.sprite = icon;
        }

        public void DroneView(bool Active)
        {
            _droneCamView.enabled = Active;
        }
    }
}

