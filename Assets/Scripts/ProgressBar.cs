using TMPro;
using UnityEngine;

public class ProgressBar : MonoBehaviour {
    private static ProgressBar _instance;

    [SerializeField]
    private RectTransform _progressBar;
    [SerializeField]
    private TMP_Text _text;

    private int _total;
    private int _count;

    public void Awake() {
        _instance = this;
    }

    public void SetTotal(int total) {
        _total = total;
        _count = 0;
        UpdateProgress();
    }

    public static void DeltaCount(int delta) {
        _instance._count += delta;
        _instance.UpdateProgress();
    }

    private void UpdateProgress() {
        float ratio = (float)_count / _total;
        _text.text = ratio.ToString("P");
        float width = _progressBar.rect.width;
        float moveAmount = Mathf.Lerp(-width, width, ratio);
        _progressBar.anchoredPosition = Vector3.zero + Vector3.right * moveAmount;
    }
}
