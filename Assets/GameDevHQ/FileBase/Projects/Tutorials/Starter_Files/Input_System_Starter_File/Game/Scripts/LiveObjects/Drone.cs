using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Game.Scripts.UI;

namespace Game.Scripts.LiveObjects
{
    public class Drone : MonoBehaviour
    {
        //private enum Tilt
        //{
        //    NoTilt, Forward, Back, Left, Right
        //}

        [SerializeField]
        private Rigidbody _rigidbody;
        [SerializeField]
        private float _speed = 5f;
        private bool _inFlightMode = false;
        [SerializeField]
        private Animator _propAnim;
        [SerializeField]
        private CinemachineVirtualCamera _droneCam;
        [SerializeField]
        private InteractableZone _interactableZone;

        private InputManager _input;

        private float _rotation;
        private Vector2 _tilt;
        private int _lift;

        public static event Action OnEnterFlightMode;
        public static event Action onExitFlightmode;

        private void OnEnable()
        {
            InteractableZone.onZoneInteractionComplete += EnterFlightMode;
            _input = GameObject.Find("Managers")?.GetComponent<InputManager>();
        }

        private void EnterFlightMode(InteractableZone zone)
        {
            if (_inFlightMode != true && zone.GetZoneID() == 4) // drone Scene
            {
                _propAnim.SetTrigger("StartProps");
                _droneCam.Priority = 11;
                _inFlightMode = true;
                OnEnterFlightMode?.Invoke();
                UIManager.Instance.DroneView(true);
                _interactableZone.CompleteTask(4);
                _input.EnableDroneMap();
            }
        }

        private void ExitFlightMode()
        {            
            _droneCam.Priority = 9;
            _inFlightMode = false;
            UIManager.Instance.DroneView(false);            
        }

        public void SetRotation(float rot)
        {
            _rotation = rot;
        }

        public void SetTilt(Vector2 tilt)
        {
            _tilt = tilt;
        }

        public void SetLift(int lift)
        {
            _lift = lift;
        }

        private void Update()
        {
            if (_inFlightMode)
            {
                CalculateTilt();
                CalculateMovementUpdate();

                //if (Input.GetKeyDown(KeyCode.Escape))
                //{
                //    _inFlightMode = false;
                //    onExitFlightmode?.Invoke();
                //    ExitFlightMode();
                //}
            }
        }

        public void EndFlight()
        {
            _inFlightMode = false;
            onExitFlightmode?.Invoke();
            ExitFlightMode();
        }

        private void FixedUpdate()
        {
            _rigidbody.AddForce(transform.up * (9.81f), ForceMode.Acceleration);
            if (_inFlightMode)
                CalculateMovementFixedUpdate();
        }

        private void CalculateMovementUpdate()
        {
            //if (Input.GetKey(KeyCode.LeftArrow))
            //{
            //    var tempRot = transform.localRotation.eulerAngles;
            //    tempRot.y -= _speed / 3;
            //    transform.localRotation = Quaternion.Euler(tempRot);
            //}
            //if (Input.GetKey(KeyCode.RightArrow))
            //{
            //    var tempRot = transform.localRotation.eulerAngles;
            //    tempRot.y += (_speed / 3) * _rotation;
            //    transform.localRotation = Quaternion.Euler(tempRot);
            //}

            var tempRot = transform.localRotation.eulerAngles;
            tempRot.y += (_speed / 3) * _rotation;
            transform.localRotation = Quaternion.Euler(tempRot);
        }

        private void CalculateMovementFixedUpdate()
        {
            
            //if (Input.GetKey(KeyCode.Space))
            //{
            //    _rigidbody.AddForce(transform.up * _speed, ForceMode.Acceleration);
            //}
            //if (Input.GetKey(KeyCode.V))
            //{
            //    _rigidbody.AddForce(-transform.up * _speed, ForceMode.Acceleration);
            //}

            _rigidbody.AddForce(_lift * transform.up * _speed, ForceMode.Acceleration);
        }

        private void CalculateTilt()
        {
            //if (Input.GetKey(KeyCode.A)) 
            //    transform.rotation = Quaternion.Euler(00, transform.localRotation.eulerAngles.y, 30);
            //else if (Input.GetKey(KeyCode.D))
            //    transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, -30);
            //else if (Input.GetKey(KeyCode.W))
            //    transform.rotation = Quaternion.Euler(30, transform.localRotation.eulerAngles.y, 0);
            //else if (Input.GetKey(KeyCode.S))
            //    transform.rotation = Quaternion.Euler(-30, transform.localRotation.eulerAngles.y, 0);
            //else 
            //    transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);

            switch (_tilt)
            {
                case Vector2 t when t.Equals(Vector2.left):
                    transform.rotation = Quaternion.Euler(00, transform.localRotation.eulerAngles.y, 30);
                    break;
                case Vector2 t when t.Equals(Vector2.right):
                    transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, -30);
                    break;
                case Vector2 t when t.Equals(Vector2.up):
                    transform.rotation = Quaternion.Euler(30, transform.localRotation.eulerAngles.y, 0);
                    break;
                case Vector2 t when t.Equals(Vector2.down):
                    transform.rotation = Quaternion.Euler(-30, transform.localRotation.eulerAngles.y, 0);
                    break;
                default:
                    transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);
                    break;
            }
        }

        private void OnDisable()
        {
            InteractableZone.onZoneInteractionComplete -= EnterFlightMode;
        }
    }
}
