using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;


public class exitapp : MonoBehaviour
{
    [Header("Exit Button Settings")]
    public Button exitButton;
    
    void Start()
    {
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(QuitGame);
        }
        else
        {
            Debug.LogError("Exit Button is not assigned and no Button component found on this GameObject!");
        }
    }
    public void QuitGame()
    {
        Debug.Log("Exit button clicked - quitting game");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    void OnDestroy()
    {
        if (exitButton != null)
        {
            exitButton.onClick.RemoveListener(QuitGame);
        }
    }
}
