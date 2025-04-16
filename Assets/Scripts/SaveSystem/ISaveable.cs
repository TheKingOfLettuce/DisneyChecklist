public interface ISaveable {
    string SaveID {get;}

    void LoadDefault();
    void LoadSave(string saveData);
    string GetSave();
}