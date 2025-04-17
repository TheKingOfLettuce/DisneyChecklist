using System.Collections.Generic;
using UnityEngine;

public class ItemManagerSave {
    public List<string> SaveNames;
    public List<ItemSave> SaveData;
}

public class ItemManager : MonoBehaviour, ISaveable {
    public static bool ShowFinished => Instance._showFinished;

    [SerializeField]
    private List<ItemData> _allItems;
    [SerializeField]
    private Transform _itemParent;
    [SerializeField]
    private Item _itemPrefab;
    [SerializeField]
    private IconManager _icons;
    [SerializeField]
    private ProgressBar _progress;

    private Dictionary<string, ItemData> _itemMap;
    private Dictionary<Activities, HashSet<ItemData>> _activityFilter;
    private Dictionary<Locations, HashSet<ItemData>> _locationFilter;
    private Dictionary<string, Item> _items;

    private List<Item> _currentList;
    private static ItemManager Instance;
    private bool _showFinished = true;

    public string SaveID => "ItemManager";

    public string GetSave() {
        List<string> saveNames = new List<string>();
        List<ItemSave> saveData = new List<ItemSave>();

        foreach(Item item in _items.Values) {
            ItemSave itemSave = new ItemSave();
            itemSave.IsChecked = item.IsChecked;
            saveNames.Add(item.Data.Name);
            saveData.Add(itemSave);
        }

        return JsonUtility.ToJson(new ItemManagerSave{SaveNames=saveNames, SaveData=saveData});
    }

    public void LoadDefault() {
        foreach(Item item in _items.Values) {
            item.ToggleChecked(false, false);
        }

    }

    public void LoadSave(string saveData) {
        ItemManagerSave mainSave = JsonUtility.FromJson<ItemManagerSave>(saveData);
        for (int i = 0; i < mainSave.SaveNames.Count; i++) {
            if (!_items.ContainsKey(mainSave.SaveNames[i])) {
                continue;
            }

            _items[mainSave.SaveNames[i]].SetSaveData(mainSave.SaveData[i]);
        }
    }

    private void Awake() {
        _itemMap = new Dictionary<string, ItemData>();
        _activityFilter = new Dictionary<Activities, HashSet<ItemData>>();
        _locationFilter = new Dictionary<Locations, HashSet<ItemData>>();
        foreach(ItemData item in _allItems) {
            if (_itemMap.ContainsKey(item.Name)) {
                Debug.LogError($"Checklist item collision {item.Name}");
                continue;
            }

            _itemMap[item.Name] = item;

            if (!_activityFilter.ContainsKey(item.ActivityType)) {
                _activityFilter[item.ActivityType] = new HashSet<ItemData>();
            }
            _activityFilter[item.ActivityType].Add(item);

            if (!_locationFilter.ContainsKey(item.Location)) {
                _locationFilter[item.Location] = new HashSet<ItemData>();
            }
            _locationFilter[item.Location].Add(item);
        }

        BuildItems();
        _progress.SetTotal(_items.Count);
        Instance = this;
    }

    private void BuildItems() {
        _items = new Dictionary<string, Item>();
        _currentList = new List<Item>();
        foreach(ItemData data in _itemMap.Values) {
            Item itemInstance = Instantiate(_itemPrefab, _itemParent);
            itemInstance.SetItemData(data, _icons);
            _items.Add(data.Name, itemInstance);
            _currentList.Add(itemInstance);
        }
    }

    public void ToggleFinished(bool showFinished) {
        if (_showFinished == showFinished)
            return;

        _showFinished = showFinished;
        foreach(Item item in _currentList) {
            if (item.IsChecked && !showFinished) {
                item.gameObject.SetActive(false);  
            }
            else {
                item.gameObject.SetActive(true);
            }
        }
    }

    public void FilterItems(HashSet<Locations> locationFilter, HashSet<Activities> activityFilter) {
        foreach(Item item in _currentList) {
            item.gameObject.SetActive(false);
        }
        _currentList.Clear();
        foreach(Item item in _items.Values) {
            if (!locationFilter.Contains(item.Data.Location)) {
                continue;
            }
            if (!activityFilter.Contains(item.Data.ActivityType)) {
                continue;
            }

            _currentList.Add(item);
            if (_showFinished || !item.IsChecked)
                item.gameObject.SetActive(true);
        }
    }
}
