using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemLongHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private Item _item;
    [SerializeField]
    private float _holdTime;

    private float _initialHoldTime;

    public void OnPointerDown(PointerEventData eventData) {
        _initialHoldTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData) {
        if ((Time.time - _initialHoldTime) >= _holdTime) {
            _item.ToggleChecked(!_item.IsChecked);
        }
    }
}
