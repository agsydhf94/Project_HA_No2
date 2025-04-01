using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class BlackHoleSkill_AttackIcon : MonoBehaviour
    {
        private KeyCode hotKey;
        private Transform enemy;
        private BlackHoleSkillController blackHole;


        public void SetupHotKey(KeyCode hotKeyInput, Transform enemyTransform, BlackHoleSkillController blackHoleSkillController)
        {
            hotKey = hotKeyInput;
            enemy = enemyTransform;
            blackHole = blackHoleSkillController;
        }

        private void Update()
        {
            if(Input.GetKeyDown(hotKey))
            {
                var enemy = blackHole.detectedTargets.Dequeue();
                blackHole.AddEnemyToList(enemy);
                gameObject.SetActive(false);
            }
        }
    }
}
