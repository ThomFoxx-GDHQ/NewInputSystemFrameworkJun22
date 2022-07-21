using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.LiveObjects
{
    public class Detonator : MonoBehaviour
    {
        [SerializeField]
        private C4 _c4;
        private bool _c4Placed;
        private MeshRenderer _render;
        [SerializeField]
        private InteractableZone[] _interactableZone;

        private void OnEnable()
        {
            InteractableZone.onZoneInteractionComplete += InteractableZone_onZoneInteractionComplete;
        }

        private void Start()
        {
            _render = GetComponent<MeshRenderer>();
        }

        private void InteractableZone_onZoneInteractionComplete(InteractableZone zone)
        {
            if (_c4Placed != true && zone.GetZoneID() == 1) //placed C4
            {             
                PlaceC4(zone.GetItems()[0].transform);
                _c4Placed = true;                
            }
        }

        public void TriggerExplosion()
        {
            if (_c4Placed == false)
                return;

            _c4.Explode();
            _c4Placed = false;
            _interactableZone[1].CompleteTask(2);
            Destroy(this.gameObject);
        }

        void PlaceC4(Transform target)
        {           
            _c4.Place(target);
            _c4.gameObject.SetActive(true);
            _c4Placed = true;
            _interactableZone[0].CompleteTask(1);
        }

        public void Show()
        {
            _render.enabled = true;
        }

        private void Ondisable()
        {
            InteractableZone.onZoneInteractionComplete -= InteractableZone_onZoneInteractionComplete;
        }
    }
}

