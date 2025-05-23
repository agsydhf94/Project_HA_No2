using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class EnemySegment : Enemy
    {
        public Transform segmentTransform; // 해당 구획의 Transform (객체 위치)
        public int segmentIndex;
        public BossEnemy boss;

        [Header("Shake Settings")]
        public float shakeDuration = 0.5f; // 흔들리는 지속 시간
        public float shakeStrength = 3f; // 흔들림 강도
        public int shakeVibrato = 10; // 흔들리는 빈도

        [Header("Color Settings")]
        public Color32 originalColor;
        public Color32 hitColor;
        public Color originalEmissionColor;
        public Color hitEmissionColor;
        public float colorLerpingTime;
        public int colorShiftingCycle;
        public float emissionIntensity;
        public float emissionLerpingTime;
        public Material segmentMaterial;
        public Renderer segmentRenderer;
        public SphereCollider sphereCollider;

        [Header("HPBar")]
        public Image hpBarImage;

        [Header("SegmentPrefab")]
        public GameObject segmentPrefab;

        private void Start()
        {
            Material_Instance();
        }


        private IEnumerator HitEffect()
        {
            // 색상 깜빡이기
            if (segmentRenderer != null)
            {
                Debug.Log("히트 효과 진입");
                segmentMaterial.DOColor(hitColor, colorLerpingTime).SetLoops(colorShiftingCycle, LoopType.Yoyo).
                    OnComplete(() => segmentMaterial.color = originalColor);


                segmentMaterial.EnableKeyword("_EMISSION");
                segmentMaterial.DOColor(hitEmissionColor * emissionIntensity, "_EmissionColor", emissionLerpingTime)
                    .SetLoops(colorShiftingCycle, LoopType.Yoyo) // 더 강한 깜빡임을 위해 루프 증가
                    .OnStart(() => segmentMaterial.SetFloat("_Emission", 1f)) // Emission 강도 활성화
                    .OnComplete(() => segmentMaterial.SetColor("_EmissionColor", originalEmissionColor)); // 원래 색상 복구;
            }

            // 위치 흔들림
            if (segmentTransform != null)
            {
                segmentTransform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato);
            }

            yield return new WaitForSeconds(shakeDuration);
        }

        private void Material_Instance()
        {
            if (segmentRenderer != null)
            {
                // 공유된 Material을 인스턴스화하여 각 오브젝트가 독립적인 Material을 가지도록 설정
                segmentMaterial = new Material(segmentRenderer.material);
                segmentRenderer.material = segmentMaterial;

                // 원래 색상 저장
                originalColor = segmentMaterial.color;
            }
        }


        public void UpdateSegmentBar(Image hpBar, float currentHealth, float maxHealth, float originalWidth)
        {
            RectTransform rectTransform = hpBar.GetComponent<RectTransform>();

            // 새로운 width 계산 (체력 비율에 맞게)
            float newWidth = Mathf.Clamp(((currentHealth / maxHealth) * originalWidth), 0f, 175f);

            // UI width 업데이트
            rectTransform.sizeDelta = new Vector2(newWidth, rectTransform.sizeDelta.y);
        }



        private void FirstExplosionStart_AndLastInformation()
        {
            boss.destroySequence_Start -= FirstExplosionStart_AndLastInformation;
            Transform lastMoment_Transform = transform;
            boss.SegmentHide_And_SetDummy(segmentIndex, lastMoment_Transform);
        }

        public IEnumerator FirstExplosion(Transform lastTransform)
        {
            var explosionEffect = ObjectPool.Instance.GetFromPool<ParticleSystem>("ExplosionPrefab");
            explosionEffect.transform.position = lastTransform.position;

            yield return new WaitForSecondsRealtime(1f);

            ObjectPool.Instance.ReturnToPool("ExplosionPrefab", explosionEffect);
        }

        public override void Die()
        {
            base.Die();
        }

        protected override UniTask HitKnockback()
        {
            return base.HitKnockback();
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}
