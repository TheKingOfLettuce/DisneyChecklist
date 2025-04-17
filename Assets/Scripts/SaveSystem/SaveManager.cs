using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.InteropServices;

public class SaveManager : MonoBehaviour {
    public static event Action OnLoadFinished;
    public static SaveManager Instance;

    #if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void SaveToLocalStorage(string key, string value);

        [DllImport("__Internal")]
        private static extern IntPtr LoadFromLocalStorage(string key);

        private const string SaveKey = "DisneyChecklistSave";
    #endif

    [Header("Save Settings")]
    [SerializeField]
    private string _jsonFileName;

    private string _filePath;
    private HashSet<ISaveable> _saveObjects = new HashSet<ISaveable>();
    private bool _loaded;

    private void Awake() {
        Instance = this;
        #if !UNITY_WEBGL
        _filePath = FilePath();
        #endif
        FetchAllSaveables();
    }

    private void Start() {
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

    private SaveDataWrapper LoadFromLocalFile() {
        Debug.Log("Loading game, looking for local save file");
        SaveDataWrapper data;
        #if UNITY_WEBGL
        IntPtr ptr = LoadFromLocalStorage(SaveKey);
        if (ptr == IntPtr.Zero) {
            return null;
        }
        string json = Marshal.PtrToStringUTF8(ptr);
        Marshal.FreeHGlobal(ptr);
        data = JsonUtility.FromJson<SaveDataWrapper>(json);
        #else
        Debug.Log($"Looking for file at {_filePath}");
        if (!File.Exists(_filePath)) {
            return null;
        }

        data = JsonUtility.FromJson<SaveDataWrapper>(File.ReadAllText(_filePath));
        #endif
        
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
        #if UNITY_WEBGL
        SaveToLocalStorage(SaveKey, dataString);
        #else
        File.WriteAllText(_filePath, dataString);
        Debug.Log($"Save data written to {_filePath}");
        #endif
        
        Debug.Log("Saving game complete");
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
        return Application.persistentDataPath + $"\\{_jsonFileName}";
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
        #if UNITY_WEBGL
        SaveToLocalStorage(SaveKey, "{}");
        #else
        File.Delete(_filePath);
        #endif
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