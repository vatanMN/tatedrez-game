using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SystemLocator : MonoBehaviour
{
    public static SystemLocator Instance
    {
        get {
            if (instance == null || instance.gameObject == null)
            {
                instance = GameObject.FindObjectOfType<SystemLocator>();
                if (!instance.isInit) instance.Init();
            }
            return instance;
        }
    }

    private static SystemLocator instance;
    private bool isInit = false;

    [SerializeField] private PoolCollection PoolCollection;
    public PoolController PoolController;
    public PanelController PanelController;

    public Canvas Canvas;
    public List<BasePanel> Panels;

    private void Init()
    {
        PoolController = new PoolController(PoolCollection);
        PanelController = new PanelController(Canvas, Panels);

        isInit = true;
        PanelController.Show(PanelType.GamePanel);
    }
}
