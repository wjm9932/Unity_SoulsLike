using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct SceneData
{
    public string itemName;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}


[System.Serializable]
public struct SceneSaveData
{
    public List<SceneData> sceneData;

    public SceneSaveData(List<SceneData> data)
    {
        sceneData = data;
    }
}
