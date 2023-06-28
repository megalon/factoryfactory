using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationBurn : Transformation
{
    private Material _burnMaterial;
    private Material _oldMaterial;
    private Renderer[] _renderers;

    private void Awake()
    {
        _renderers = gameObject.GetComponentsInChildren<Renderer>();

        LoadBurnMaterial();

        _oldMaterial = _renderers[0].material;

    }

    private void OnEnable()
    {
        LoadBurnMaterial();
        DoTransformation();
    }

    private void LoadBurnMaterial()
    {
        if (_burnMaterial == null)
        {
            _burnMaterial = Resources.Load("MegWorkPartBurnedMaterial", typeof(Material)) as Material;
        }

        if (_burnMaterial == null)
        {
            Debug.LogError("Could not load _burnMaterial!");
        }
    }

    protected override void DoTransformation()
    {
        if (_burnMaterial == null)
        {
            Debug.LogError("Could not do transformation because _burnMaterial is null!");
            return;
        }

        for(int i = 0; i < _renderers.Length; ++i)
        {
            Renderer r = _renderers[i];
            if (r.material != _burnMaterial)
            {
                r.material = _burnMaterial;
            }
        }
    }

    protected override void UndoDoTransformation()
    {
        if (_oldMaterial == null)
        {
            return;
        }

        for (int i = 0; i < _renderers.Length; ++i)
        {
            Renderer r = _renderers[i];

            if (r.material != _oldMaterial)
            {
                r.material = _oldMaterial;
            }
        }
    }
}
