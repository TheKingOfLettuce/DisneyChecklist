using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class ItemSave {
    public bool IsChecked;
}

public class Item : MonoBehaviour {
    public bool IsChecked => _isChecked;

    [SerializeField]
    private Image _icon;
    [SerializeField]
    private Image _bg;
    [SerializeField]
    private TMP_Text _title;
    [SerializeField]
    private TMP_Text _desc;

    private bool _isChecked;
    private ItemData _item;

    public void SetItemData(ItemData data) {
        _item = data;
        _icon.sprite = IconManager.GetLocationIcon(_item.Location);
        _bg.color = IconManager.GetActivityColor(_item.ActivityType);
        _title.text = _item.Name;

        _desc.text = $"{_item.ActivityType}\n{_item.Location}";
    }

    public void SetSaveData(ItemSave save) {
        _isChecked = save.IsChecked;
    }

    public void ToggleChecked(bool flag, bool shouldSave = true) {
        _isChecked = flag;
        if (shouldSave) {
            SaveManager.Instance.SaveToJson();
        }
    }
}
