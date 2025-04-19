using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemLongHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private Item _item;
    [SerializeField]
    private float _holdTime;

    public void OnPointerDown(PointerEventData eventData) {
        CancelInvoke(nameof(Check));
        Invoke(nameof(Check), _holdTime);
    }

    public void OnPointerUp(PointerEventData eventData) {
        CancelInvoke(nameof(Check));
    }

    private void Check() {
        _item.ToggleChecked(!_item.IsChecked);
    }
}