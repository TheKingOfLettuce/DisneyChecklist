using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SaveManager : MonoBehaviour {
    public static event Action OnLoadFinished;
    public static SaveManager Instance;

    #if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void JS_FileSystem_Sync();
    #endif

    [Header("Save Settings")]
    [SerializeField]
    private string _jsonFileName;

    private string _filePath;
    private HashSet<ISaveable> _saveObjects = new HashSet<ISaveable>();
    private bool _loaded;

    private void Awake() {
        Instance = this;
        _filePath = FilePath();
        FetchAllSaveables();
    }

    private void Start() {
        #if UNITY_WEBGL
            WebStart();
        #endif 

        SaveDataWrapper data = LoadFromLocalFile();
        
        if (data == null) {
            Debug.Log("No file found, loading defaults");
            LoadDefaults();
        }
        else {
            Debug.Log("File found, loading game from file");
            LoadGame(data);
        }
        _loaded = true;
        OnLoadFinished?.Invoke();
    }

    private void WebStart() {
        Debug.Log("Init game for web");
        if (!Directory.Exists("idbfs/BoinkSaves"))
            Directory.CreateDirectory("idbfs/BoinkSaves");
    }

    private SaveDataWrapper LoadFromLocalFile() {
        Debug.Log("Loading game, looking for local save file");
        Debug.Log($"Looking for file at {_filePath}");
        if (!File.Exists(_filePath)) {
            return null;
        }

        SaveDataWrapper data = JsonUtility.FromJson<SaveDataWrapper>(File.ReadAllText(_filePath));
        return data;
    }

    private void OnApplicationQuit() {
        Debug.Log("Application quitting");
        SaveToJson();
    }

    public void SaveToJson() {
        Debug.Log("Saving game");
        if (!_loaded) {
            Debug.LogWarning("Stopping save to avoid potential overwrite, we have not attempted loading yet");
            return;
        }
        
        SaveDataPair[] saveData = new SaveDataPair[_saveObjects.Count];
        int i = 0;
        foreach(ISaveable save in _saveObjects) {
            saveData[i] = new SaveDataPair{Key = save.SaveID, Data = save.GetSave()};
            Debug.Log($"Generated save data piece {save.SaveID} => {saveData[i].Data}");
            i++;
        }

        SaveDataWrapper finalSave = new SaveDataWrapper {SaveData = saveData};
        Debug.Log("Writing save data to json file");
        string dataString = JsonUtility.ToJson(finalSave);
        File.WriteAllText(_filePath, dataString);
        Debug.Log($"Save data written to {_filePath}");
        Debug.Log("Saving game complete");
        #if UNITY_WEBGL
            JS_FileSystem_Sync();
        #endif
    }

    public void LoadGame(SaveDataWrapper fileData) {
        SaveDataPair[] loadedData = fileData?.SaveData;

        Dictionary<string, string> saveData = new Dictionary<string, string>();
        foreach(SaveDataPair loadData in loadedData) {
            Debug.Log($"Data read and parsed from file {loadData.Key} => {loadData.Data}");
            saveData.Add(loadData.Key, loadData.Data);
        }
        foreach(ISaveable save in _saveObjects) {
            if (saveData.ContainsKey(save.SaveID)) {
                Debug.Log($"Save data match on {save.SaveID}, loading data on {(save as Component).name}");
                save.LoadSave(saveData[save.SaveID]);
            }
            else {
                Debug.Log($"No save data match on {save.SaveID}, loading default on {(save as Component).name}");
                save.LoadDefault();
            }
        }

        Debug.Log("Loading from file complete");
    }

    private string FilePath() {
        #if UNITY_WEBGL 
            return "idbfs/BoinkSaves" + $"/{_jsonFileName}";
        #else
            return Application.persistentDataPath + $"\\{_jsonFileName}";
        #endif
    }

    private void LoadDefaults() {
        foreach(ISaveable save in _saveObjects) {
            Debug.Log($"Loading default for {save.SaveID}");
            save.LoadDefault();
        }
    }

    public void QuitGame() {
        Debug.Log("Game quit requested");
        Application.Quit();
    }

    public void ResetGame() {
        Debug.Log("Reseting game");
        //PlayerPrefs.DeleteAll();
        File.Delete(_filePath);
        Debug.Log("Reloading scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void FetchAllSaveables() {
        Debug.Log("Looking for saveables in current scene");
        GameObject[] roots = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach(GameObject root in roots) {
            foreach(ISaveable save in root.GetComponentsInChildren<ISaveable>(true)) {
                if (string.IsNullOrWhiteSpace(save.SaveID)) continue;

                _saveObjects.Add(save);
                Debug.Log($"Found saveable {save.SaveID} on {(save as Component).name}");
            }
        }
    }
}

[System.Serializable]
public struct SaveDataPair {
    public string Key;
    public string Data;
}

[System.Serializable]
public class SaveDataWrapper {
    public SaveDataPair[] SaveData;
}