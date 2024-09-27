using UnityEngine;
using DG.Tweening;

public class Health : MonoBehaviour
{
    public float hitFlashDuration = 0.3f;

    private Material _hitFlashMaterialInstance;

    public Character healthCharacter;
    public HealthBar healthBar;
    [HideInInspector] public int currentHealth;

    private Tweener _deathAnim;
    private Tweener _hitFlashAnim;

    public DropLoot dropLoot;

    private void Awake()
    {
        SetHitFlashMaterial();
    }

    private void Start()
    {
        // Counteract character scaling, increment sorting layer
        if (healthBar != null)
        {
            healthCharacter.spriteRenderer.sortingOrder = LevelManager.currentSpawnedEnemyIndex;
            healthBar.sortingGroup.sortingOrder = LevelManager.currentSpawnedEnemyIndex;

            LevelManager.currentSpawnedEnemyIndex++;

            Vector3 parentScale = healthCharacter.transform.localScale;
            healthBar.gameObject.transform.localScale = new Vector3(1 / parentScale.x, 1 / parentScale.y, 1 / parentScale.z);
        }

        currentHealth = healthCharacter.maxHealth;
        SetHealthBarValues();
    }

    public void TakeDamage(int damageTaken)
    {

        if (!healthCharacter.isDead)
        {
            HitFlashAnim();
        }

        currentHealth = Mathf.Max(0, currentHealth - damageTaken);

        if (currentHealth == 0 && !healthCharacter.isDead)
        {
            healthCharacter.isDead = true;

            GameEventsManager.selfInstance.Death(healthCharacter);

            healthCharacter.characterRigidbody.simulated = false;
            SpriteFadeDeathAnim();

            if (dropLoot == null) return;
            dropLoot.CreateLootBox(healthCharacter.minAmmoDrop, healthCharacter.maxAmmoDrop);

        }

        SetHealthBarValues();

    }

    void SetHealthBarValues()
    {
        if (healthBar != null)
        {
            if (currentHealth == 0) 
            {
                healthBar.gameObject.SetActive(false);
                return;
            }

            healthBar.UpdateHealthBar(currentHealth, healthCharacter.maxHealth);
        }
    }

    void SpriteFadeDeathAnim()
    {
        _deathAnim = healthCharacter.spriteRenderer.DOFade(0, 1)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }

    void SetHitFlashMaterial()
    {
        _hitFlashMaterialInstance = healthCharacter.spriteRenderer.material;
    }

    void HitFlashAnim()
    {
        _hitFlashAnim?.Kill();

        if (_hitFlashMaterialInstance != null)
        {
            // Set flash intensity and fade it out using DoTween
            _hitFlashMaterialInstance.SetFloat("_Flash_Intensity", 1.0f);
            _hitFlashAnim = _hitFlashMaterialInstance.DOFloat(0.0f, "_Flash_Intensity", hitFlashDuration);
        }
    }

    private void OnDisable()
    {
        _deathAnim?.Kill();
        _hitFlashAnim.Kill();
    }
}