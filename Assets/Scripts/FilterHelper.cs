using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterHelper : MonoBehaviour {
    [SerializeField]
    private ItemManager _manager;

    private HashSet<Activities> _activityFilter;
    private HashSet<Locations> _locationFilter;

    private void Awake() {
        _activityFilter = new HashSet<Activities> {
            Activities.RIDE,
            Activities.SHOP,
            Activities.FOOD,
            Activities.ENTERTAINMENT
        };

        _locationFilter = new HashSet<Locations> {
            Locations.DISNEY_LAND,
            Locations.CALIFORNIA_ADVENTURE,
            Locations.DOWNTOWN_DISNEY
        };
    }

    public void OnRideChanged(bool flag) {
        DeltaActivity(Activities.RIDE, flag);

        _manager.FilterItems(_locationFilter, _activityFilter);
    }

    public void OnShopChanged(bool flag) {
        DeltaActivity(Activities.SHOP, flag);

        _manager.FilterItems(_locationFilter, _activityFilter);
    }

    public void OnFoodChanged(bool flag) {
        DeltaActivity(Activities.FOOD, flag);

        _manager.FilterItems(_locationFilter, _activityFilter);
    }

    public void OnEntertainmentChanged(bool flag) {
        DeltaActivity(Activities.ENTERTAINMENT, flag);

        _manager.FilterItems(_locationFilter, _activityFilter);
    }

    public void OnDisneylandChanged(bool flag) {
        DeltaLocation(Locations.DISNEY_LAND, flag);

        _manager.FilterItems(_locationFilter, _activityFilter);
    }

    public void OnCaliforniaChanged(bool flag) {
        DeltaLocation(Locations.CALIFORNIA_ADVENTURE, flag);

        _manager.FilterItems(_locationFilter, _activityFilter);
    }

    public void OnDowntownChanged(bool flag) {
        DeltaLocation(Locations.DOWNTOWN_DISNEY, flag);

        _manager.FilterItems(_locationFilter, _activityFilter);
    }

    private void DeltaLocation(Locations location, bool flag) {
        if (flag)
            _locationFilter.Add(location);
        else
            _locationFilter.Remove(location);
    }

    private void DeltaActivity(Activities activity, bool flag) {
        if (flag)
            _activityFilter.Add(activity);
        else
            _activityFilter.Remove(activity);
    }
}
