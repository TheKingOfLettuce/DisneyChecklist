using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemLongHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private Item _item;
    [SerializeField]
    private float _holdTime;

    private float _initalHoldTime;

    public void OnPointerDown(PointerEventData eventData) {
        _initalHoldTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData) {
        if ((Time.time - _initalHoldTime) >= _holdTime) {
            _item.OpenLink();
        }
    }
}
