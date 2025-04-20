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

        private Transform closestTarget;
        private float explodeRadius;
        [SerializeField] private LayerMask explodeLayer;
        [SerializeField] private LayerMask enemyLayer;


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

            if(canMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);

                if(Vector3.Distance(transform.position, closestTarget.position) < 1f)
                {
                    FinishElement();
                    canMove = false;
                }
            }
        }

        public void SetupElement(float _duration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestTarget)
        {
            elementTimer = _duration;
            canExplode = _canExplode;
            canMove = _canMove;
            moveSpeed = _moveSpeed;            
            closestTarget = _closestTarget;
        }

        public void ChooseRandomEnemy()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10, enemyLayer);

            if(colliders.Length > 0)
            {
                closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
            }
            
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
