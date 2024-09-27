using UnityEngine;

public class ShootOrigin : MonoBehaviour
{
    public Animator muzzleFlashAnimator;
    public Animator playerAnimator;

    void StopFireAnim()
    {
        muzzleFlashAnimator.SetBool("Shooting", Input.GetButton("Fire1") || Input.GetButton("Jump"));
        playerAnimator.SetBool("Shooting", Input.GetButton("Fire1") || Input.GetButton("Jump"));

        if (!Input.GetButton("Fire1") && !Input.GetButton("Jump"))
        {
            GameEventsManager.selfInstance.GunStartStopFiring(false);
        }
    }
}
