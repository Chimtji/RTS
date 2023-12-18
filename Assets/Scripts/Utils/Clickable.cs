using UnityEngine;
using UnityEngine.EventSystems;

namespace Trout.Utils
{
    public abstract class Controllable : MonoBehaviour, IPointerClickHandler
    {
        public abstract GameObject ui { get; }
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            ui.SetActive(true);
        }
    }
}