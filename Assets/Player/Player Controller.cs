using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 _muzzleFlashAnimPosition;

    public Animator playerAnimator;
    public Animator muzzleFlashAnimator;
    public Character playerCharacter;
    public SpriteRenderer muzzleFlashSprite;

    private PlayerAnimFrame _animFrameIndex;

    public float playerAnimSpeed = 0.2f;
    public float shootAnimSpeed = 0.4f;

    private float _attackCooldown;  // Tracks when the player can attack next

    public Bullet bulletPrefab;

    enum PlayerAnimFrame
    {
        Idle0,
        Idle1,
        Idle2,
        Idle3,
        Run0,
        Run1,
        Run2,
        Run3
    }

    private void Start()
    {
        PlayerStateManager.SetStartingAmmo();

        playerAnimator.speed = playerAnimSpeed;
        muzzleFlashAnimator.speed = shootAnimSpeed;
        _muzzleFlashAnimPosition = muzzleFlashSprite.transform.localPosition;
    }

    private void Update()
    {
        if (!LevelManager.levelStarted) return;

        MovePlayer();

        FireGun();
    }

    // Control the muzzle flash position with anim frames
    void PositionMuzzleFlashWithAnim(int intFrameIndex)
    {
        _animFrameIndex = (PlayerAnimFrame)intFrameIndex;

        switch (_animFrameIndex)
        {
            case PlayerAnimFrame.Idle0:
                _muzzleFlashAnimPosition = new Vector2(1.49f, 0.48f);
                break;
            case PlayerAnimFrame.Idle1:
                _muzzleFlashAnimPosition = new Vector2(1.46f, 0.48f);
                break;
            case PlayerAnimFrame.Idle2:
                _muzzleFlashAnimPosition = new Vector2(1.41f, 0.48f);
                break;
            case PlayerAnimFrame.Idle3:
                _muzzleFlashAnimPosition = new Vector2(1.48f, 0.48f);
                break;
            case PlayerAnimFrame.Run0:
                _muzzleFlashAnimPosition = new Vector2(1.48f, 0.49f);
                break;
            case PlayerAnimFrame.Run1:
                _muzzleFlashAnimPosition = new Vector2(1.66f, 0.525f);
                break;
            case PlayerAnimFrame.Run2:
                _muzzleFlashAnimPosition = new Vector2(1.565f, 0.54f);
                break;
            case PlayerAnimFrame.Run3:
                _muzzleFlashAnimPosition = new Vector2(1.455f, 0.605f);
                break;
        }

    }

    void FireGun()
    {
        bool pressingShoot = Input.GetButton("Fire1") || Input.GetButton("Jump");

        // Only attack if the fire button is held down and the cooldown period has passed
        if (pressingShoot && Time.time >= _attackCooldown && !UserInterface.isGamePaused)
        {
            _attackCooldown = Time.time + playerCharacter.attackSpeed;

            // Enable the firing animation
            muzzleFlashAnimator.SetBool("Shooting", true);
            playerAnimator.SetBool("Shooting", true);

            // Bullet spawn position
            Vector3 bulletSpawnPosition = muzzleFlashSprite.transform.position;

            // Set rotation based on flip
            Quaternion bulletRotation;
            if (muzzleFlashSprite.flipX)
            {
                bulletRotation = Quaternion.Euler(0, 0, 90); // Flipped (facing left)
            }
            else
            {
                bulletRotation = Quaternion.Euler(0, 0, -90); // Not flipped (facing right)
            }

            Bullet shotBullet = Instantiate(bulletPrefab, bulletSpawnPosition, bulletRotation, LevelManager.selfTransform);

            // Facing left or right
            shotBullet.bulletDirection = muzzleFlashSprite.flipX ? -1 : 1;

            // Decrement current ammo
            PlayerStateManager.currentAmmo--;

            UserInterface.UpdateAmmoCount();

            if (PlayerStateManager.currentAmmo == 0)
            {
                UserInterface.lossStringText = "Out of Ammo";
                UserInterface.InitializeLossInterface();

                // Disable player controller on loss
                enabled = false;
            }

            GameEventsManager.selfInstance.GunShot();
            GameEventsManager.selfInstance.GunStartStopFiring(true);
        }
    }

    void MovePlayer()
    {
        // Get the horizontal input axis (-1 to 1)
        float moveInput = Input.GetAxis("Horizontal");

        playerCharacter.MoveCharacter(moveInput);

        if (moveInput != 0)
        {
            playerAnimator.SetBool("Running", true);

            muzzleFlashSprite.flipX = moveInput < 0;
        }
        else
        {
            playerAnimator.SetBool("Running", false);
        }

        muzzleFlashSprite.transform.localPosition = new Vector3(
            muzzleFlashSprite.flipX ? -_muzzleFlashAnimPosition.x : _muzzleFlashAnimPosition.x, _muzzleFlashAnimPosition.y, 0);
    }

}
