using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PanelController
{
    private Canvas Canvas;
    Dictionary<PanelType, BasePanel> PanelDic = new Dictionary<PanelType, BasePanel>();
    Dictionary<PanelType, BasePanel> CreatedPanels = new Dictionary<PanelType, BasePanel>();

    public PanelEvent OnPanelHide = new PanelEvent();

    public PanelController(Canvas canvas, List<BasePanel> panels)
    {
        Canvas = canvas;
        foreach (var item in panels)
        {
            PanelDic[item.PanelType] = item;
        }
    }

    public void Show(PanelType panelType, params object[] parameters)
    {
        if (!CreatedPanels.ContainsKey(panelType))
        {
            var panel = GameObject.Instantiate(PanelDic[panelType], Canvas.transform);
            CreatedPanels[panelType] = panel;
        }
        CreatedPanels[panelType].Show(parameters);
    }

    public void Hide(PanelType panelType)
    {
        CreatedPanels[panelType].Hide();
    }
}

public class PanelEvent : UnityEvent<PanelType> { };

public enum PanelType
{
    WinPanel,
    GamePanel
}
