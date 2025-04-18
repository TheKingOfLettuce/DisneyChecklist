using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemLongHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private Item _item;
    [SerializeField]
    private float _holdTime;

    public void OnPointerDown(PointerEventData eventData) {
        CancelInvoke(nameof(OpenLink));
        Invoke(nameof(OpenLink), _holdTime);
    }

    public void OnPointerUp(PointerEventData eventData) {
        CancelInvoke(nameof(OpenLink));
    }

    private void OpenLink() {
        _item.OpenLink();
    }
}
