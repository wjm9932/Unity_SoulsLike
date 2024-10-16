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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Cursor.visible == false)
        {
            HideToolTip();
        }
        toolTip.transform.position = Input.mousePosition;
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
