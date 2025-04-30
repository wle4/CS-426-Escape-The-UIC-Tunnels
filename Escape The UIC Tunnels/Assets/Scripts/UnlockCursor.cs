using UnityEngine;

public class UnlockCursorOnCredits : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;  // Unlock the cursor
        Cursor.visible = true;                   // Make it visible
    }
}
