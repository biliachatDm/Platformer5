using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSys : MonoBehaviour
{
    private Save sv = new Save();
    private string path;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "Save.json");
#else
        path = Path.Combine(Application.dataPath, "Save,json");
#endif
        if (File.Exists(path))
        {
            sv = JsonUtility.FromJson<Save>(File.ReadAllText(path));
        }
    }

    /*public void CheckName( string name)
    {
        if (!srting.IsNullOrEmpty(name) && name.Length >= 1)
        {
            sv.name = name;

        }
    }*/
    public void CheckVolume(string volume)
    {
        if (!string.IsNullOrEmpty(volume) && volume.Length > 0)
        {
            sv.volume = float.Parse(volume);
        }
    }

    public void CheckName(string Plname)
    {
        if (!string.IsNullOrEmpty(Plname) && Plname.Length > 0)
        {
            sv.name = Plname;
        }
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
        if (pause) File.WriteAllText(path, JsonUtility.ToJson(sv));
    }
#endif
    private void OnApplicationQuit()
    {
        File.WriteAllText(path, JsonUtility.ToJson(sv));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Serializable]
    public class Save
    {
        public string name;
        public float volume;
    }
}
