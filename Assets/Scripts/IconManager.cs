using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour {
    private static IconManager _instance;

    [SerializeField]
    private Sprite _disneyIcon;
    [SerializeField]
    private Sprite _californiaIcon;
    [SerializeField]
    private Sprite _downtownIcon;
    [SerializeField]
    private Color _rideColor;
    [SerializeField]
    private Color _shopColor;
    [SerializeField]
    private Color _foodColor;
    [SerializeField]
    private Color _entertainmentColor;

    private void Awake() {
        if (_instance != null) {
            Debug.LogError("Cant have more than 1 icon manager");
            Destroy(this);
        }

        _instance = this;
    }

    private void OnDestroy() {
        if (_instance == this) {
            _instance = null;
        }
    }

    public static Sprite GetLocationIcon(Locations location) {
        if (_instance == null) {
            return null;
        }

        switch (location) {
            case Locations.DISNEY_LAND:
                return _instance._disneyIcon;
            case Locations.CALIFORNIA_ADVENTURE:
                return _instance._californiaIcon;
            case Locations.DOWNTOWN_DISNEY:
                return _instance._downtownIcon;
        }

        return null;
    }

    public static Color GetActivityColor(Activities activity) {
        if (_instance == null) {
            return Color.grey;
        }

        switch (activity) {
            case Activities.RIDE:
                return _instance._rideColor;
            case Activities.SHOP:
                return _instance._shopColor;
            case Activities.FOOD:
                return _instance._foodColor;
            case Activities.ENTERTAINMENT:
                return _instance._entertainmentColor;
        }

        return Color.gray;
    }
}
