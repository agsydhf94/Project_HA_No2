using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ElementSkillController : MonoBehaviour
    {
        private float elementTimer;

        private bool canExplode;
        private bool canMove;
        private float moveSpeed;

        private float explodeRadius;
        private LayerMask explodeLayer;


        private VFXManager vfxManager;
        private PlayerCharacter playerCharacter;

        private void Awake()
        {
            vfxManager = VFXManager.Instance;
            playerCharacter = PlayerManager.Instance.playerCharacter;
        }

        private void Update()
        {
            transform.rotation *= Quaternion.Euler(0f, Time.deltaTime * 35f, 0f);

            elementTimer -= Time.deltaTime;
            if (elementTimer < 0)
            {
                FinishElement();
            }
        }

        public void SetupElement(float _duration, bool _canExplode, bool _canMove, float _moveSpeed)
        {
            elementTimer = _duration;
            canExplode = _canExplode;
            canMove = _canMove;
            moveSpeed = _moveSpeed;            
        }

        public void SetupExplode(float _explodeRadius, LayerMask _layer)
        {
            explodeRadius = _explodeRadius;
            explodeLayer = _layer;
        }

        public void FinishElement()
        {
            if(canExplode)
            {
                vfxManager.PlayEffect("mari_ElementExplode", transform.position, transform.rotation, null, 1f);
                AddDamage();
                SelfDestruct();
            }
            else
            {
                SelfDestruct();
            }
        }

        public void SelfDestruct() => Destroy(gameObject);


        public void AddDamage()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius, explodeLayer);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out IDamagable damagable))
                {
                    var target = collider.transform.GetComponent<EnemyStat>();
                    playerCharacter.characterStats.DoDamage(target);
                }
            }
        }
    }
}
