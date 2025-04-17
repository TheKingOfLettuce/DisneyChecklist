using UnityEngine;

public class IconManager : MonoBehaviour {
    [SerializeField]
    private Color _disneyColor;
    [SerializeField]
    private Color _californiaColor;
    [SerializeField]
    private Color _downtownColor;
    [SerializeField]
    private Sprite _rideIcon;
    [SerializeField]
    private Sprite _shopIcon;
    [SerializeField]
    private Sprite _foodIcon;
    [SerializeField]
    private Sprite _entertainmentIcon;


    public Color GetLocationColor(Locations location) {
        switch (location) {
            case Locations.DISNEY_LAND:
                return _disneyColor;
            case Locations.CALIFORNIA_ADVENTURE:
                return _californiaColor;
            case Locations.DOWNTOWN_DISNEY:
                return _downtownColor;
        }

        return Color.gray;
    }

    public Sprite GetActivityIcon(Activities activity) {
        switch (activity) {
            case Activities.RIDE:
                return _rideIcon;
            case Activities.SHOP:
                return _shopIcon;
            case Activities.FOOD:
                return _foodIcon;
            case Activities.ENTERTAINMENT:
                return _entertainmentIcon;
        }

        return null;
    }
}
