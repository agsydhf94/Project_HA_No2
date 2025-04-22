using HA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrikeController : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;

    private int damage;

    private bool triggered;

    private void Update()
    {
        if(!targetStats)
            return;

        if (triggered)
            return;

        Vector3 targetPosition = targetStats.transform.position + new Vector3(0f, 1f, 0f);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime); ;

        if(Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            triggered = true;
            targetStats.ApplyShock(true);
            Destroy(gameObject, 0.4f);
        }
    }

    public void ThunderStrikeSetup(int _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }
}
