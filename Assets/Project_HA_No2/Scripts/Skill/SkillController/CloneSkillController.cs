using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class CloneSkillController : MonoBehaviour
    {
        [SerializeField] private float cloneDuration;
        private float cloneTimer;

        private Animator animator;

        [SerializeField] private Transform attackCheck;
        [SerializeField] private float attackCheckRadius;
        private Transform closestEnemy;

        [SerializeField] private SkinnedMeshRenderer smr_Face;
        [SerializeField] private SkinnedMeshRenderer smr_Body;
        [SerializeField] private SkinnedMeshRenderer smr_Hair;
        [SerializeField] private float colorLoosingSpeed;



        private void Awake()
        {
            animator = GetComponent<Animator>();

            SetTransparentAllMaterials(transform, smr_Face);
            SetTransparentAllMaterials(transform, smr_Body);
            SetTransparentAllMaterials(transform, smr_Hair);
        }

        private void Update()
        {
            cloneTimer -= Time.deltaTime;

            if (cloneDuration <= 0)
            {
                ReduceAlphaAllMaterials(smr_Face, colorLoosingSpeed);
                ReduceAlphaAllMaterials(smr_Body, colorLoosingSpeed);
                ReduceAlphaAllMaterials(smr_Hair, colorLoosingSpeed);
            }
        }

        void SetTransparentAllMaterials(Transform trasnform, SkinnedMeshRenderer smr)
        {
            for (int i = 0; i < smr.materials.Length; i++)
            {
                Material mat = new Material(smr.materials[i]); // 개별 인스턴스화
                mat.SetFloat("_AlphaMode", 2);  // 2 = Transparent (수동 설정)
                mat.SetOverrideTag("RenderType", "Transparent");
                mat.renderQueue = 3000; // 투명 오브젝트 렌더링 순서
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);  // 깊이 쓰기 비활성화 (투명 처리 가능)

                smr.materials[i] = mat;  // 적용
            }
        }

        void ReduceAlphaAllMaterials(SkinnedMeshRenderer smr, float fadeSpeed)
        {
            for (int i = 0; i < smr.materials.Length; i++)
            {
                Material mat = smr.materials[i];
                Color color = mat.color;

                color.a -= cloneTimer * fadeSpeed;
                mat.color = color;
            }
        }


        public void SetUpClone(Transform newTransform, bool canAttack, Vector3 offset)
        {
            if (canAttack)
                animator.SetInteger("AttackNumber", Random.Range(1, 3));

            transform.position = newTransform.position + offset;
            cloneTimer = cloneDuration;
        }

        private void AnimationTrigger_On()
        {
            cloneTimer = -0.1f;
        }

        private void AttackTrigger()
        {
            List<Collider> colliders = CharacterBase.ObjectDetection<IDamagable>(attackCheck, attackCheckRadius);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out IDamagable damagable))
                {
                    damagable.ApplyDamage();
                }
            }
        }

        private void FaceClosestTarget()
        {
            List<Collider> coliiders = CharacterBase.ObjectDetection<IDamagable>(transform, 25f);

            float closestDistance = Mathf.Infinity;
            
            foreach(var collider in coliiders)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);

                if (distanceToEnemy < closestDistance)
                    closestEnemy = collider.transform;
            }

            if(closestEnemy != null)
            {
                // To do : Face the clone towards the closest enemy
            }
        }
    }
}
