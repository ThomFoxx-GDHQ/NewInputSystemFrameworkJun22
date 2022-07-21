using System;
using UnityEngine;
using Cinemachine;

namespace Game.Scripts.LiveObjects
{
    public class Forklift : MonoBehaviour
    {
        [SerializeField]
        private GameObject _lift, _steeringWheel, _leftWheel, _rightWheel, _rearWheels;
        [SerializeField]
        private Vector3 _liftLowerLimit, _liftUpperLimit;
        [SerializeField]
        private float _speed = 5f, _liftSpeed = 1f;
        [SerializeField]
        private CinemachineVirtualCamera _forkliftCam;
        [SerializeField]
        private GameObject _driverModel;
        private bool _inDriveMode = false;
        [SerializeField]
        private InteractableZone _interactableZone;

        private InputManager _input;

        private float _move;
        private float _turn;
        private float _liftDirection;

        public static event Action onDriveModeEntered;
        public static event Action onDriveModeExited;

        private void OnEnable()
        {
            InteractableZone.onZoneInteractionComplete += EnterDriveMode;
            _input = GameObject.Find("Managers")?.GetComponent<InputManager>();
        }

        private void EnterDriveMode(InteractableZone zone)
        {
            if (_inDriveMode !=true && zone.GetZoneID() == 5) //Enter ForkLift
            {
                _inDriveMode = true;
                _forkliftCam.Priority = 11;
                onDriveModeEntered?.Invoke();
                _driverModel.SetActive(true);
                _interactableZone.CompleteTask(5);
                _input.EnableForkliftMap();
            }
        }

        public void ExitDriveMode()
        {
            _inDriveMode = false;
            _forkliftCam.Priority = 9;            
            _driverModel.SetActive(false);
            onDriveModeExited?.Invoke();
            _input.EnablePlayerMap();
        }

        public void SetMove(float move)
        {
            _move = move;
        }

        public void SetTurn(float turn)
        {
            _turn = turn;
        }

        public void SetLift(float lift)
        {
            _liftDirection = lift;
        }

        private void Update()
        {
            if (_inDriveMode == true)
            {
                LiftControls();
                CalcutateMovement();
                //if (Input.GetKeyDown(KeyCode.Escape))
                //    ExitDriveMode();
            }

        }

        private void CalcutateMovement()
        {
            //float h = Input.GetAxisRaw("Horizontal");
            //float v = Input.GetAxisRaw("Vertical");
            //var direction = new Vector3(0, 0, v);

            var direction = new Vector3(0, 0, _move);
            var velocity = direction * _speed;

            transform.Translate(velocity * Time.deltaTime);

            //if (Mathf.Abs(v) > 0)
            //{
            //    var tempRot = transform.rotation.eulerAngles;
            //    tempRot.y += h * _speed / 2;
            //    transform.rotation = Quaternion.Euler(tempRot);
            //}

            if (Mathf.Abs(_move) > 0)
            {
                var tempRot = transform.rotation.eulerAngles;
                tempRot.y += _turn * _speed / 2;
                transform.rotation = Quaternion.Euler(tempRot);
            }
        }

        private void LiftControls()
        {
            //if (Input.GetKey(KeyCode.R))
            //    LiftUpRoutine();
            //else if (Input.GetKey(KeyCode.T))
            //    LiftDownRoutine();

            if (_liftDirection > 0)
                LiftUpRoutine();
            else if (_liftDirection < 0)
                LiftDownRoutine();
        }

        private void LiftUpRoutine()
        {
            if (_lift.transform.localPosition.y < _liftUpperLimit.y)
            {
                Vector3 tempPos = _lift.transform.localPosition;
                tempPos.y += Time.deltaTime * _liftSpeed;
                _lift.transform.localPosition = new Vector3(tempPos.x, tempPos.y, tempPos.z);
            }
            else if (_lift.transform.localPosition.y >= _liftUpperLimit.y)
                _lift.transform.localPosition = _liftUpperLimit;
        }

        private void LiftDownRoutine()
        {
            if (_lift.transform.localPosition.y > _liftLowerLimit.y)
            {
                Vector3 tempPos = _lift.transform.localPosition;
                tempPos.y -= Time.deltaTime * _liftSpeed;
                _lift.transform.localPosition = new Vector3(tempPos.x, tempPos.y, tempPos.z);
            }
            else if (_lift.transform.localPosition.y <= _liftUpperLimit.y)
                _lift.transform.localPosition = _liftLowerLimit;
        }

        private void OnDisable()
        {
            InteractableZone.onZoneInteractionComplete -= EnterDriveMode;
        }

    }
}