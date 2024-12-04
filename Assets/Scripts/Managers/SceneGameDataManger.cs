using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGameDataManger : MonoBehaviour
{
    public void LoadSceneData(SceneSaveData data)
    {
        List<UX_Item> sceneItem = new List<UX_Item>();

        foreach (UX_Item item in Object.FindObjectsOfType<UX_Item>(true))
        {
            sceneItem.Add(item);
        }

        for (int i = data.sceneData.Count - 1; i >= 0; i--)
        {
            for (int j = sceneItem.Count - 1; j >= 0; j--)
            {
                if (sceneItem[j].data.itemName != data.sceneData[i].itemName) continue;
                if (sceneItem[j].numOfItem != data.sceneData[i].count) continue;

                sceneItem[j].transform.position = data.sceneData[i].position;
                sceneItem[j].transform.rotation = data.sceneData[i].rotation;
                sceneItem[j].transform.localScale = data.sceneData[i].scale;
                sceneItem[j].numOfItem = data.sceneData[i].count;

                data.sceneData.RemoveAt(i);
                sceneItem.RemoveAt(j);
                break;
            }
        }


        if (data.sceneData.Count > 0)
        {
            UX_Item[] itemPrefabs = Resources.LoadAll<UX_Item>("ItemPrefabs");

            Dictionary<string, GameObject> refItem = new Dictionary<string, GameObject>();

            for (int i = 0; i < itemPrefabs.Length; i++)
            {
                refItem.Add(itemPrefabs[i].data.itemName, itemPrefabs[i].gameObject);
            }

            for (int i = data.sceneData.Count - 1; i >= 0; i--)
            {
                var item = Instantiate(refItem[data.sceneData[i].itemName], data.sceneData[i].position, data.sceneData[i].rotation).GetComponent<UX_Item>();
                item.numOfItem = data.sceneData[i].count;
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
            sceneData.count = item.numOfItem;

            data.Add(sceneData);
        }

        return new SceneSaveData(data);
    }
}
