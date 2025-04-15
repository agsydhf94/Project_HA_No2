using HA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleDropCount;
    [SerializeField] private ItemDataSO[] possibleDrops; // 드롭할 아이템 후보군
    private List<ItemDataSO> dropList = new List<ItemDataSO>(); // 실제로 드롭할 아이템 리스트 (possibleDrops 에서 뽑아옵니다)

    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private ItemDataSO itemDataSO;

    public void GenerateDropItem()
    {
        for(int i = 0; i < possibleDrops.Length; i++)
        {
            if(Random.Range(0, 100) <= possibleDrops[i].dropChance)
            {
                dropList.Add(possibleDrops[i]);
            }
        }

        for(int i = 0; i < possibleDropCount; i++)
        {
            ItemDataSO randomItem = dropList[Random.Range(0, dropList.Count - 1)];

            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }

    public void DropItem(ItemDataSO itemDataSO)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector3 randomVelocity = new Vector3(Random.Range(-3, 3), Random.Range(6, 8), Random.Range(-3, 3));
        newDrop.GetComponent<ItemObject>().SetUpItem(itemDataSO, randomVelocity);
    }
}
