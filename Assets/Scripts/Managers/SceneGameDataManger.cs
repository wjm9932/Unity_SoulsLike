using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGameDataManger : MonoBehaviour
{
    public void LoadSceneData(SceneSaveData data)
    {
        List<UX_Item> sceneItem = new List<UX_Item>();
        Dictionary<string, GameObject> refItem = new Dictionary<string, GameObject>();

        foreach (UX_Item item in Object.FindObjectsOfType<UX_Item>(true))
        {
            sceneItem.Add(item);
            refItem.TryAdd(item.data.itemName, item.gameObject);
        }

        for (int i = data.sceneData.Count - 1; i >= 0; i--)
        {
            for (int j = sceneItem.Count - 1; j >= 0; j--)
            {
                if (sceneItem[j].data.itemName == data.sceneData[i].itemName)
                {
                    sceneItem[j].transform.position = data.sceneData[i].position;
                    sceneItem[j].transform.rotation = data.sceneData[i].rotation;
                    sceneItem[j].transform.localScale = data.sceneData[i].scale;

                    data.sceneData.RemoveAt(i);
                    sceneItem.RemoveAt(j);
                    break; 
                }
            }
        }
        for (int i = data.sceneData.Count - 1; i >= 0; i--)
        {
            if (refItem.ContainsKey(data.sceneData[i].itemName) == false)
            {
                //GameObject item = 
            }
            else
            {
                GameObject item = refItem[data.sceneData[i].itemName];
                Instantiate(item, data.sceneData[i].position, data.sceneData[i].rotation);
            }
        }

        for (int i = sceneItem.Count - 1; i >= 0; i--)
        {
            Destroy(sceneItem[i].gameObject);
        }

    }

    public SceneSaveData SaveSceneData()
    {
        List<SceneData> data = new List<SceneData>();

        foreach (UX_Item item in Object.FindObjectsOfType<UX_Item>(true))
        {
            SceneData sceneData = new SceneData();
            sceneData.position = item.transform.position;
            sceneData.rotation = item.transform.rotation;
            sceneData.scale = item.transform.localScale;
            sceneData.itemName = item.data.itemName;

            data.Add(sceneData);
        }

        return new SceneSaveData(data);
    }
}
