using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Rendering;

public class LootBox : MonoBehaviour
{
    public SortingGroup lootBoxSortingGroup;
    public TextMeshPro ammoAmountText;
    public Rigidbody2D lootRigidbody;
    public int ammoAmountDropped = 1;
    public float lootAnimDuration = 0.5f;

    private Sequence _lootAnim;

    private void Start()
    {
        lootBoxSortingGroup.sortingOrder = LevelManager.currentDroppedLootIndex;

        LevelManager.currentDroppedLootIndex++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the layer of the collider is not "Player"
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }

        GameEventsManager.selfInstance.AmmoLooted();

        lootRigidbody.simulated = false;
        
        LootAnim();

        // Add looted ammo and update UI
        PlayerStateManager.currentAmmo += ammoAmountDropped;

        UserInterface.UpdateAmmoCount();
    }

    void LootAnim()
    {
        _lootAnim?.Kill();

        _lootAnim = DOTween.Sequence()
            .Append(gameObject.transform.DOScale(0, lootAnimDuration))
            .SetEase(Ease.InBack, 2.1f)
            .Join(ammoAmountText.DOFade(0, lootAnimDuration * 0.4f))
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }

}
