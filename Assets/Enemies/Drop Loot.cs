using UnityEngine;

public class DropLoot : MonoBehaviour
{
    public LootBox lootBoxPrefab;

    public void CreateLootBox(int minDrop, int maxDrop)
    {
        LootBox spawnedLootBox = Instantiate(lootBoxPrefab, new Vector3(transform.position.x, -0.7f, 0), Quaternion.identity, LevelManager.selfTransform);

        // + 1 to include maxDrop in the range
        int ammoAmountToDrop = Random.Range(minDrop, maxDrop + 1);

        spawnedLootBox.ammoAmountDropped = ammoAmountToDrop;

        spawnedLootBox.ammoAmountText.text = ammoAmountToDrop.ToString();
    }
}
