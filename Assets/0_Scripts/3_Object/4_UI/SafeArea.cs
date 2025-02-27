using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private Vector2 minAnchor;
    private Vector2 maxAnchor;


    void Start()
    {
        Rect safeAreaRect = Screen.safeArea;
        RectTransform rectTransform = GetComponent<RectTransform>();

        minAnchor = safeAreaRect.position;
        maxAnchor = safeAreaRect.position + safeAreaRect.size;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;

        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        rectTransform.anchorMin = minAnchor;
        rectTransform.anchorMax = maxAnchor;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
}
