using UnityEngine.EventSystems;
using UnityEngine;
public class EventClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private void Awake()
    {
        //
    }

    public void OnPointerDown(PointerEventData data)
    {
        Debug.Log("pointer down");
    }
    public void OnPointerUp(PointerEventData data)
    {
        Debug.Log("pointer up");
    }
    public void OnPointerClick(PointerEventData data)
    {
        Debug.Log("pointer click");
    }
    public void OnPointerEnter(PointerEventData data)
    {
        Debug.Log("pointer enter");
    }
    public void OnPointerExit(PointerEventData data)
    {
        Debug.Log("pointer exit");
    }
}