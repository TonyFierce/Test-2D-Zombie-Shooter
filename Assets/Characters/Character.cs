using UnityEngine;

public class Character : MonoBehaviour
{
    public int creatureID = 999999;

    // Enemy stats are set on spawn by NPC controller
    public float moveSpeed = 5.5f;
    public float attackSpeed = 0.5f;
    public int maxHealth = 100;
    public int minAmmoDrop = 1;
    public int maxAmmoDrop = 3;

    public Rigidbody2D characterRigidbody;
    public SpriteRenderer spriteRenderer;

    [HideInInspector] public bool isDead = false;

    public void MoveCharacter(float moveDirection)
    {
        if (isDead) return;

        Vector2 moveVelocity = new Vector2(moveDirection * moveSpeed, 0);

        characterRigidbody.velocity = moveVelocity;

        if (moveDirection == 0) return;

        spriteRenderer.flipX = moveDirection < 0;
    }

}
