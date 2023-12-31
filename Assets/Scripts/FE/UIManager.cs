using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    private Dictionary<UIPanelType, BasePanel> panels = new Dictionary<UIPanelType, BasePanel>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    private void Start()
    {
        foreach (var panel in panels.Values)
        {
            panel.Deactive();
        }
    }

    public void ResgisterPanel(UIPanelType panelType, BasePanel panel)
    {
        if (!panels.ContainsKey(panelType))
        {
            panels.Add(panelType, panel);
        }
    }

    public BasePanel GetPanel(UIPanelType panelType)
    {
        panels.TryGetValue(panelType, out BasePanel panel);
        return panel;
    }
}
