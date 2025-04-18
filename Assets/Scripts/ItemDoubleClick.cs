using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDoubleClick : MonoBehaviour, IPointerClickHandler {
    [SerializeField]
    private Item _item;
    [SerializeField]
    private float _doubleClickTime;

    private float _lastTime;

    public void OnPointerClick(PointerEventData eventData) {
        if ((Time.time - _lastTime) <= _doubleClickTime) {
            _item.OpenLink();
            _lastTime = 0;
            return;
        }

        _lastTime = Time.time;
    }
}
