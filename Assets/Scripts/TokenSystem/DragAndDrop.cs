using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;

    public abstract void OnEndDrag(PointerEventData eventData);
    public abstract void OnBeginDrag(PointerEventData eventData);

    protected virtual void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = FindAnyObjectByType <Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; // Adjust the object's position relative to the canvas
    }

}
