using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;

[Serializable]
public class ItemSave {
    public bool IsChecked;
}

public class Item : MonoBehaviour {
    public bool IsChecked => _isChecked;
    public ItemData Data => _item;

    [SerializeField]
    private Image _icon;
    [SerializeField]
    private Image _bg;
    [SerializeField]
    private TMP_Text _title;
    [SerializeField]
    private TMP_Text _desc;
    [SerializeField]
    private float _dragLength;
    [SerializeField]
    private GameObject _selectedOverlay;

    private bool _isChecked;
    private ItemData _item;

    [DllImport("__Internal")]
	private static extern void OpenJSLink(string url);

    public void SetItemData(ItemData data, IconManager icons) {
        _item = data;
        _icon.sprite = icons.GetActivityIcon(_item.ActivityType);
        _bg.color = icons.GetLocationColor(_item.Location);
        _title.text = _item.Name;

        _desc.text = $"{_item.ActivityType}\n{_item.Location}";
    }

    public void SetSaveData(ItemSave save) {
        ToggleChecked(save.IsChecked, false);
    }

    public void ToggleChecked(bool flag, bool shouldSave = true) {
        if (_isChecked == flag)
            return;
        _isChecked = flag;
        ProgressBar.DeltaCount(flag ? 1 : -1);
        _selectedOverlay.SetActive(_isChecked);
        if (shouldSave) {
            SaveManager.Instance.SaveToJson();
        }
        if (!ItemManager.ShowFinished) {
            this.gameObject.SetActive(false);
        }
    }

    public void OpenLink() {
        if (string.IsNullOrEmpty(_item.URL)) {
            return;
        }

        OpenJSLink(_item.URL);
    }
}
