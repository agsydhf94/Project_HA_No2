using UnityEngine;

namespace HA
{
    /// <summary>
    /// Represents a melee weapon that deals damage in a radius around the attack origin.
    /// Uses OverlapSphere to detect enemies within range and applies damage using the owner's stats.
    /// </summary>
    public class SwordWeapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private Transform attackOrigin;
        private EquipmentDataSO equipmentDataSO;
        private CharacterStats ownerStats;

        /// <summary>
        /// Initializes the weapon with provided equipment data and retrieves the owner's stats.
        /// </summary>
        /// <param name="_equipmentDataSO">The equipment data for this sword weapon.</param>
        public void InitializeWeaponData(EquipmentDataSO _equipmentDataSO)
        {
            equipmentDataSO = _equipmentDataSO;

            if (ownerStats == null)
            {
                ownerStats = PlayerManager.Instance.playerCharacter.GetComponent<CharacterStats>();
            }
        }


        /// <summary>
        /// Executes the sword attack by detecting enemies within the attack radius
        /// and applying damage and effects to valid targets.
        /// </summary>
        public void Attack()
        {
            Collider[] hits = Physics.OverlapSphere(attackOrigin.position, equipmentDataSO.damageApplyRadius);
            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent(out EnemyStat target))
                {
                    ownerStats.DoDamage(target);
                    WeaponEffect(target.transform);
                }
            }
        }


        /// <summary>
        /// Applies the weapon's effect to the target, such as debuffs, stat modifications, or special conditions.
        /// The effect behavior is defined in the associated EquipmentDataSO.
        /// </summary>
        /// <param name="target">The transform of the enemy receiving the effect.</param>
        public void WeaponEffect(Transform target)
        {
            equipmentDataSO?.ApplyEffect(target);
        }

        
        /// <summary>
        /// Draws a debug gizmo to visualize the attack radius in the editor.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (attackOrigin == null) return;

            Gizmos.color = Color.red;

            if (equipmentDataSO != null)
                Gizmos.DrawWireSphere(attackOrigin.position, equipmentDataSO.damageApplyRadius);
        }

    }
}
