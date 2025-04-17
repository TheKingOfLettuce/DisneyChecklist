using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemLongHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private Item _item;
    [SerializeField]
    private float _holdTime;

    private IEnumerator _holdRoutine;

    public void OnPointerDown(PointerEventData eventData) {
        if (_holdRoutine != null) {
            StopCoroutine(_holdRoutine);
        }

        _holdRoutine = HoldRoutine();
        StartCoroutine(_holdRoutine);
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (_holdRoutine != null) {
            StopCoroutine(_holdRoutine);
        }

        _holdRoutine = null;
    }

    private IEnumerator HoldRoutine() {
        yield return new WaitForSeconds(_holdTime);
        _item.OpenLink();
        _holdRoutine = null;
    }
}
