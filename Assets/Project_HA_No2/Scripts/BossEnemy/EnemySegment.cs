using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class EnemySegment : Enemy
    {
        public Transform segmentTransform; // �ش� ��ȹ�� Transform (��ü ��ġ)
        public int segmentIndex;
        public BossEnemy boss;

        [Header("Shake Settings")]
        public float shakeDuration = 0.5f; // ��鸮�� ���� �ð�
        public float shakeStrength = 3f; // ��鸲 ����
        public int shakeVibrato = 10; // ��鸮�� ��

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
            // ���� �����̱�
            if (segmentRenderer != null)
            {
                Debug.Log("��Ʈ ȿ�� ����");
                segmentMaterial.DOColor(hitColor, colorLerpingTime).SetLoops(colorShiftingCycle, LoopType.Yoyo).
                    OnComplete(() => segmentMaterial.color = originalColor);


                segmentMaterial.EnableKeyword("_EMISSION");
                segmentMaterial.DOColor(hitEmissionColor * emissionIntensity, "_EmissionColor", emissionLerpingTime)
                    .SetLoops(colorShiftingCycle, LoopType.Yoyo) // �� ���� �������� ���� ���� ����
                    .OnStart(() => segmentMaterial.SetFloat("_Emission", 1f)) // Emission ���� Ȱ��ȭ
                    .OnComplete(() => segmentMaterial.SetColor("_EmissionColor", originalEmissionColor)); // ���� ���� ����;
            }

            // ��ġ ��鸲
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
                // ������ Material�� �ν��Ͻ�ȭ�Ͽ� �� ������Ʈ�� �������� Material�� �������� ����
                segmentMaterial = new Material(segmentRenderer.material);
                segmentRenderer.material = segmentMaterial;

                // ���� ���� ����
                originalColor = segmentMaterial.color;
            }
        }


        public void UpdateSegmentBar(Image hpBar, float currentHealth, float maxHealth, float originalWidth)
        {
            RectTransform rectTransform = hpBar.GetComponent<RectTransform>();

            // ���ο� width ��� (ü�� ������ �°�)
            float newWidth = Mathf.Clamp(((currentHealth / maxHealth) * originalWidth), 0f, 175f);

            // UI width ������Ʈ
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
