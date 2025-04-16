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
        if (flag)
            _activityFilter.Add(Activities.RIDE);
        else
            _activityFilter.Remove(Activities.RIDE);

        _manager.FilterItems(_locationFilter, _activityFilter);
    }

    public void OnShopChanged(bool flag) {
        if (flag)
            _activityFilter.Add(Activities.SHOP);
        else
            _activityFilter.Remove(Activities.SHOP);

        _manager.FilterItems(_locationFilter, _activityFilter);
    }

    public void OnFoodChanged(bool flag) {
        if (flag)
            _activityFilter.Add(Activities.FOOD);
        else
            _activityFilter.Remove(Activities.FOOD);

        _manager.FilterItems(_locationFilter, _activityFilter);
    }

    public void OnEntertainmentChanged(bool flag) {
        if (flag)
            _activityFilter.Add(Activities.ENTERTAINMENT);
        else
            _activityFilter.Remove(Activities.ENTERTAINMENT);

        _manager.FilterItems(_locationFilter, _activityFilter);
    }

    public void OnDisneylandChanged(bool flag) {
        if (flag)
            _locationFilter.Add(Locations.DISNEY_LAND);
        else
            _locationFilter.Remove(Locations.DISNEY_LAND);

        _manager.FilterItems(_locationFilter, _activityFilter);
    }

    public void OnCaliforniaChanged(bool flag) {
        if (flag)
            _locationFilter.Add(Locations.CALIFORNIA_ADVENTURE);
        else
            _locationFilter.Remove(Locations.CALIFORNIA_ADVENTURE);

        _manager.FilterItems(_locationFilter, _activityFilter);
    }

    public void OnDowntownChanged(bool flag) {
        if (flag)
            _locationFilter.Add(Locations.DOWNTOWN_DISNEY);
        else
            _locationFilter.Remove(Locations.DOWNTOWN_DISNEY);

        _manager.FilterItems(_locationFilter, _activityFilter);
    }
}
