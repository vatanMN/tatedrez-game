using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : BasePanel
{
    public override PanelType PanelType => PanelType.WinPanel;
    [SerializeField] TextMeshProUGUI WinnerText;
    [SerializeField] private Button RestartButton;

    public override void Show(params object[] parameters)
    {
        RestartButton.onClick.AddListener(Hide);
        WinnerText.text = parameters[0] as string;
        base.Show(parameters);
    }

    public override void Hide()
    {
        RestartButton.onClick.RemoveListener(Hide);
        base.Hide();
        SystemLocator.Instance.PanelController.Hide(PanelType.GamePanel);
    }
}

