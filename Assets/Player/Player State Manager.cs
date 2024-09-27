using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public static bool gameLost = false;

    public static string levelToLoad = "Level";

    public static int currentAmmo;
    public int startingAmmo = 25;

    public static PlayerStateManager selfInstance;

    private void Awake()
    {
        selfInstance = this;
    }

    public static void SetStartingAmmo()
    {
        currentAmmo = selfInstance.startingAmmo;
        UserInterface.UpdateAmmoCount();
    }
}