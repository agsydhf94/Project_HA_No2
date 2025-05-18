using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class WeaponInfoUI : MonoBehaviour
    {
        [Header("Meta UI")]
        [SerializeField] private TMP_Text weaponNameText;
        [SerializeField] private Image weaponImage;
        [SerializeField] private TMP_Text weaponTypeText;

        [Header("Ammo UI")]
        [SerializeField] private GameObject gunInfoParent;
        [SerializeField] private TMP_Text magazine_CurrentText;
        [SerializeField] private TMP_Text totalBulletText;

        private GunViewModel viewModel;

       
        public void Bind(GunViewModel vm)
        {
            if (viewModel != null)
                viewModel.OnDataChanged -= UpdateUI;

            viewModel = vm;
            viewModel.OnDataChanged += UpdateUI;

            UpdateUI(); // 최초 값 반영
        }

        private void UpdateUI()
        {
            if (viewModel == null) return;

            // 메타 데이터 반영
            weaponNameText.text = viewModel.WeaponName;
            weaponImage.sprite = viewModel.Icon;
            weaponTypeText.text = viewModel.WeaponType.ToString();

            // 이미지 회전 보정
            weaponImage.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            weaponImage.preserveAspect = true;
            weaponImage.color = Color.blue;

            // 탄약 UI 활성화 여부 결정
            bool isGun = viewModel.WeaponType != WeaponType.Sword;
            gunInfoParent.SetActive(isGun);

            // 탄약 데이터 반영
            magazine_CurrentText.text = viewModel.CurrentAmmo.ToString();
            totalBulletText.text = viewModel.TotalAmmo.ToString();
        }

        public void ClearUI()
        {
            weaponNameText.text = "";
            weaponImage.sprite = null;
            weaponTypeText.text = "";
            magazine_CurrentText.text = "";
            totalBulletText.text = "";
            gunInfoParent.SetActive(false);
        }

        private void OnDestroy()
        {
            if (viewModel != null)
                viewModel.OnDataChanged -= UpdateUI;
        }
    }
}
