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

            UpdateUI(); // ���� �� �ݿ�
        }

        private void UpdateUI()
        {
            if (viewModel == null) return;

            // ��Ÿ ������ �ݿ�
            weaponNameText.text = viewModel.WeaponName;
            weaponImage.sprite = viewModel.Icon;
            weaponTypeText.text = viewModel.WeaponType.ToString();

            // �̹��� ȸ�� ����
            weaponImage.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            weaponImage.preserveAspect = true;
            weaponImage.color = Color.blue;

            // ź�� UI Ȱ��ȭ ���� ����
            bool isGun = viewModel.WeaponType != WeaponType.Sword;
            gunInfoParent.SetActive(isGun);

            // ź�� ������ �ݿ�
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
