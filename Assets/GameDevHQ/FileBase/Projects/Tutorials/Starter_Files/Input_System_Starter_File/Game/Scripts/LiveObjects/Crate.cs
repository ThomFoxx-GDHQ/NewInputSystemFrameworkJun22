using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.LiveObjects
{
    public class Crate : MonoBehaviour
    {
        [SerializeField] private float _punchDelay;
        [SerializeField] private GameObject _wholeCrate, _brokenCrate;
        [SerializeField] private Rigidbody[] _pieces;
        [SerializeField] private BoxCollider _crateCollider;
        [SerializeField] private InteractableZone _interactableZone;
        private bool _isReadyToBreak = false;

        private List<Rigidbody> _brakeOff = new List<Rigidbody>();

        private InputManager _input;
        private bool _currentPunch = false;
        [SerializeField] private int _powerPunchHits = 3;
        private bool _canPunch = true;

        private void OnEnable()
        {
            InteractableZone.onZoneInteractionComplete += InteractableZone_onZoneInteractionComplete;
            _input = GameObject.Find("Managers")?.GetComponent<InputManager>();
        }

        private void InteractableZone_onZoneInteractionComplete(InteractableZone zone)
        {
            
            if (zone.GetZoneID() == 6 && _isReadyToBreak == false && _brakeOff.Count >0)
            {
                _wholeCrate.SetActive(false);
                _brokenCrate.SetActive(true);
                _isReadyToBreak = true;
                _input.EnableCratemap();
            }

            //if (_isReadyToBreak && zone.GetZoneID() == 6) //Crate zone            
            //{
            //    if (_brakeOff.Count > 0)
            //    {
            //        BreakPart();
            //        StartCoroutine(PunchDelay());
            //    }
            //    else if(_brakeOff.Count == 0)
            //    {
            //        _isReadyToBreak = false;
            //        _crateCollider.enabled = false;
            //        _interactableZone.CompleteTask(6);
            //        Debug.Log("Completely Busted");
            //    }
            //}
        }

        public void PunchStart()
        {
            _currentPunch = true;
        }

        public void ShortPunch()
        {
            if (_currentPunch)
            {
                Hit(1);
                _currentPunch = false;
            }
        }

        public void PowerPunch()
        {
            Hit(_powerPunchHits);
            _currentPunch = false;
        }

        private void Hit(int hits)
        {
            for (int i = 0; i < hits; i++)
            {
                if (_isReadyToBreak)
                {
                    if (_brakeOff.Count > 0)
                    {
                        BreakPart();
                        StartCoroutine(PunchDelay());
                    }
                    else if (_brakeOff.Count == 0)
                    {
                        _isReadyToBreak = false;
                        _crateCollider.enabled = false;
                        _interactableZone.CompleteTask(6);
                        _input.EnablePlayerMap();
                        Debug.Log("Completely Busted");
                    }
                }
            }
        }

        private void Start()
        {
            _brakeOff.AddRange(_pieces);            
        }

        public void BreakPart()
        {
            int rng = Random.Range(0, _brakeOff.Count);
            _brakeOff[rng].constraints = RigidbodyConstraints.None;
            _brakeOff[rng].AddForce(Vector3.up *3 , ForceMode.Impulse);
            _brakeOff.Remove(_brakeOff[rng]);            
        }

        IEnumerator PunchDelay()
        {
            _canPunch = false;
            float delayTimer = 0;
            while (delayTimer < _punchDelay)
            {
                yield return new WaitForEndOfFrame();
                delayTimer += Time.deltaTime;
            }
            _canPunch = true;
            //_interactableZone.ResetAction(6);
        }

        private void OnDisable()
        {
            InteractableZone.onZoneInteractionComplete -= InteractableZone_onZoneInteractionComplete;
        }
    }
}
