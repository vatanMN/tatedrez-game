using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasePanel : MonoBehaviour
{
    [SerializeField] protected Button CloseButton;

    public abstract PanelType PanelType { get; }
    public virtual void Show(params object[] parameters)
    {
        gameObject.SetActive(true);
        CloseButton?.onClick.AddListener(Hide);
    }

    public virtual void Hide()
    {
        CloseButton?.onClick.RemoveListener(Hide);
        gameObject.SetActive(false);
        SystemLocator.Instance.PanelController.OnPanelHide.Invoke(PanelType);
    }
}
