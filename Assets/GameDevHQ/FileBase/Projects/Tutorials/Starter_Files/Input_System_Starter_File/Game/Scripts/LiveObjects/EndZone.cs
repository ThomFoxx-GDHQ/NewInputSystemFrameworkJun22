using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.LiveObjects
{
    public class EndZone : MonoBehaviour
    {
        private void OnEnable()
        {
            InteractableZone.onZoneInteractionComplete += InteractableZone_onZoneInteractionComplete;
        }

        private void InteractableZone_onZoneInteractionComplete(InteractableZone zone)
        {
            if (zone.GetZoneID() == 7)
            {
                InteractableZone.CurrentZoneID = 0;
                SceneManager.LoadScene(0);
            }
        }

        private void OnDisable()
        {
            InteractableZone.onZoneInteractionComplete -= InteractableZone_onZoneInteractionComplete;
        }
    }
}