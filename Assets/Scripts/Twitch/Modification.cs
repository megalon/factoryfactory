using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modification
{
    protected bool _active = false;
    protected ModificationTypes _modificationType = ModificationTypes.SHOW;

    public bool IsActive { get => _active; }
    public ModificationTypes ModificationType { get => _modificationType; }

    public virtual void Activate()
    {
        _active = true;
        OnActivate();
    }

    public virtual void Deactivate()
    {
        _active = false;
        OnDeactivate();
    }

    protected abstract void OnActivate();
    protected abstract void OnDeactivate();

    public enum ModificationTypes
    {
        SCRIPT,
        AUDIO,
        SHOW
    }
}
