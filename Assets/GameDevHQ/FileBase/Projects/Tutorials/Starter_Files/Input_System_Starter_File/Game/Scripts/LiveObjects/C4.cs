using Game.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.LiveObjects
{
    public class C4 : MonoBehaviour
    {
        [SerializeField]
        private GameObject _explosionPrefab;        
        private Collider[] hits = new Collider[5];

        public void Explode()
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);


            var count = Physics.OverlapSphereNonAlloc(transform.position, 1, hits);
            
            if (count > 0)
            {
                foreach (var obj in hits)
                {
                    if (obj != null)
                    {
                        if (obj.TryGetComponent<IDestroyable>(out IDestroyable destroyable))
                            destroyable.DestroyAction();
                    }
                }
            }
            
            Destroy(this.gameObject);           
        }

        public void Place(Transform target)
        {
            this.transform.SetPositionAndRotation(target.position, target.rotation);
            this.transform.parent = null;
        }
    
    }
}

