using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{
    private static ToolTipManager _instance;
    public static ToolTipManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<ToolTipManager>();
            return _instance;
        }
    }

    [SerializeField] private GameObject toolTip;
    [SerializeField] private TextMeshProUGUI toolTipText;
    private RectTransform toolTipRectTransform;
    private void Awake()
    {
        toolTipRectTransform = toolTip.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Cursor.visible == false)
        {
            HideToolTip();
        }
        else
        {
            Vector3 newPosition = Input.mousePosition;
            Vector3[] canvasCorners = new Vector3[4];
            RectTransform canvasRect = toolTipRectTransform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            canvasRect.GetWorldCorners(canvasCorners);

            float pivotOffsetX = toolTipRectTransform.rect.width * toolTipRectTransform.pivot.x * toolTipRectTransform.lossyScale.x;
            float pivotOffsetY = toolTipRectTransform.rect.height * toolTipRectTransform.pivot.y * toolTipRectTransform.lossyScale.y;

            float minX = canvasCorners[0].x + pivotOffsetX;
            float maxX = canvasCorners[2].x - (toolTipRectTransform.rect.width * toolTipRectTransform.lossyScale.x - pivotOffsetX);
            float minY = canvasCorners[0].y + pivotOffsetY;
            float maxY = canvasCorners[2].y - (toolTipRectTransform.rect.height * toolTipRectTransform.lossyScale.y - pivotOffsetY);

            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            toolTip.transform.position = newPosition;
        }
    }

    public void SetToolTip(string text)
    {
        toolTip.SetActive(true);
        toolTipText.text = text;
    }
    public void HideToolTip()
    {
        toolTip.SetActive(false);
    }
}
