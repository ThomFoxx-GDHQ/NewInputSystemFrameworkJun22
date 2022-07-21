using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts.Interfaces;

namespace Game.Scripts.LiveObjects
{
    public class Taxi : MonoBehaviour, IDestroyable
    {
        [SerializeField]
        private GameObject _onDestroyed;

        public void DestroyAction()
        {
            _onDestroyed.transform.SetPositionAndRotation(transform.position, transform.rotation);
            _onDestroyed.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}

