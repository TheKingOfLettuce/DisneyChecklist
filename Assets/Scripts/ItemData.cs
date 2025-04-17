using UnityEngine;

[CreateAssetMenu(fileName="Item", menuName="ItemData")]
public class ItemData : ScriptableObject {
    public string Name => _name;
    public Activities ActivityType => _activityType;
    public Locations Location => _location;
    public string URL => _url;

    [SerializeField]
    private string _name;
    [SerializeField]
    private Activities _activityType;
    [SerializeField]
    private Locations _location;
    [SerializeField]
    private string _url;
}