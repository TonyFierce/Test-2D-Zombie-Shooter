using UnityEngine;

public class NPCController : MonoBehaviour
{
    [HideInInspector] public int creatureID;
    
    private float _playerLocation;
    private float _npcMoveDirection;

    public Animator npcAnimator;
    public Character npcCharacter;

    private void Start()
    {
        SetNPCStats();
        npcAnimator.SetInteger("Zombie ID", creatureID);
        npcAnimator.SetBool("Initialized", true);

        _playerLocation = FindObjectOfType<PlayerController>().transform.position.x;

        _npcMoveDirection = _playerLocation < transform.position.x ? -1 : 1;
    }

    private void Update()
    {
        // Prevent movement when dead and freeze animation
        if (npcCharacter.isDead)
        {
            npcAnimator.speed = 0;
            return;
        }

        npcCharacter.MoveCharacter(_npcMoveDirection);
        
    }

    void SetNPCStats()
    {
        EnemyStats npcStats = EnemyStatsDatabase.GetEnemyStats(creatureID);

        npcCharacter.maxHealth = npcStats.maxHealth;
        npcCharacter.moveSpeed = npcStats.moveSpeed;
        npcCharacter.attackSpeed = npcStats.attackSpeed;
        npcCharacter.minAmmoDrop = npcStats.ammoDropMin;
        npcCharacter.maxAmmoDrop = npcStats.ammoDropMax;
        transform.localScale *= npcStats.sizeScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the layer of the collider is not "Player"
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }

        Health playerHealth = collision.gameObject.GetComponent<Health>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);

            UserInterface.lossStringText = "Zombified";
            UserInterface.InitializeLossInterface();

            playerHealth.GetComponent<PlayerController>().enabled = false;
        }
    }
}
