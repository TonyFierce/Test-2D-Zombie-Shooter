using UnityEngine;
using UnityEngine.Rendering;

public class HealthBar : MonoBehaviour
{

    public SortingGroup sortingGroup;
    public SpriteRenderer healthBarFill;
    public SpriteRenderer healthBarBackground;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float backgroundWidth = healthBarBackground.sprite.bounds.size.x;

        // Set the width of the health bar background based on max health
        // Each 20 health adds 0.5 units to the width
        float scaledBackgroundWidth = backgroundWidth * (1 + (maxHealth / 20f) * 0.5f);

        // Calculate the fill width based on the current health
        float fillWidth = Mathf.Max(0, (currentHealth / maxHealth) * scaledBackgroundWidth - 0.1f);

        SetSpriteRendererSize(healthBarBackground, scaledBackgroundWidth);

        SetSpriteRendererSize(healthBarFill, fillWidth);

        // Align middle of the bar with the character
        transform.localPosition = new Vector3(-scaledBackgroundWidth / 2, 2, 0);
    }

    private void SetSpriteRendererSize(SpriteRenderer spriteRenderer, float width)
    {
        // Adjust the size of the SpriteRenderer
        Vector3 newSize = spriteRenderer.size;
        newSize.x = width;
        spriteRenderer.size = newSize;
    }
}
