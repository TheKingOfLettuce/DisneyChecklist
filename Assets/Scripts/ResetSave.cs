using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResetSave : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private float _holdTime;
    [SerializeField]
    private GameObject _confirmWindow;

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
        _confirmWindow.SetActive(true);
        _holdRoutine = null;
    }
}
