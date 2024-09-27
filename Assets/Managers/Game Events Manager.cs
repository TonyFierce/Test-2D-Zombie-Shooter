using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager selfInstance;

    private void Awake()
    {
        selfInstance = this;
    }

    public event Action<Character> onDeath;
    public void Death(Character deadCharacter)
    {
        if (onDeath != null)
        {
            onDeath(deadCharacter);
        }
    }

    public event Action onAmmoLooted;
    public void AmmoLooted()
    {
        if (onAmmoLooted != null)
        {
            onAmmoLooted();
        }
    }

    public event Action onGunShot;
    public void GunShot()
    {
        if (onGunShot != null)
        {
            onGunShot();
        }
    }

    public event Action<bool> onGunStartStopFiring;
    public void GunStartStopFiring(bool startStop)
    {
        if (onGunStartStopFiring != null)
        {
            onGunStartStopFiring(startStop);
        }
    }
}
