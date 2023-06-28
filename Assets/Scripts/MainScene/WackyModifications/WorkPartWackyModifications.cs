using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkPartWackyModifications : MonoBehaviour
{
    private void Awake()
    {
        WackyModificationsManager.WorkPartScaleChanged += ScaleChanged;
    }

    private void OnEnable()
    {
        ScaleChanged();
    }

    public void ScaleChanged()
    {
        transform.localScale = WackyModificationsManager.Instance.WackyModificationScale;
    }

    private void OnDestroy()
    {
        WackyModificationsManager.WorkPartScaleChanged -= ScaleChanged;
    }
}
