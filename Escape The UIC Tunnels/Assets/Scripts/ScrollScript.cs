using UnityEngine;
using UnityEngine.UI;

public class ResetScrollPosition : MonoBehaviour
{
    public ScrollRect scrollRect;

    void Start()
    {
        // Force it to start at the top
        scrollRect.verticalNormalizedPosition = 1f;
    }
}
