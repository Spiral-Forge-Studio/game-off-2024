using UnityEngine;

public class BuffMenuManager : MonoBehaviour
{
    public BuffMenu buffMenu;

    public void OnResetBuffHistoryButtonPressed()
    {
        if (buffMenu != null)
        {
            buffMenu.InitializeMenu();
        }
        else
        {
            Debug.LogWarning("BuffMenu is not assigned!");
        }
    }
}
