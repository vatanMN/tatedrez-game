using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    [SerializeField] TetraBoard Board;

    public override PanelType PanelType => PanelType.GamePanel;

    public override void Show(params object[] parameters)
    {
        base.Show(parameters);
        Board.Create();
    }
    public override void Hide()
    {
       // SystemLocator.Instance.PanelController.Show(PanelType.StartPanel);
        Board.ClearBoard(() => { });
        base.Hide();
        SystemLocator.Instance.PanelController.Show(PanelType.GamePanel);
    }
}
