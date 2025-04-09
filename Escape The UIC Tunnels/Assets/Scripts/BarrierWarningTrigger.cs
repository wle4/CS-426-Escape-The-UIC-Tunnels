using UnityEngine;
using TMPro;

public class BarrierWarningTrigger : MonoBehaviour
{
    public TextMeshProUGUI warningText;
    public float displayTime = 3f;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player colliding with barrier");
            warningText.gameObject.SetActive(true);
            CancelInvoke(); // prevent overlapping invokes
            Invoke(nameof(HideWarning), displayTime);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideWarning();
        }
    }

    void HideWarning()
    {
        warningText.gameObject.SetActive(false);
    }
}