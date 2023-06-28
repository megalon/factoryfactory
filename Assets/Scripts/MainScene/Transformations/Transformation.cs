using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Transformation : MonoBehaviour
{
    public int index;
    protected abstract void DoTransformation();
    protected abstract void UndoDoTransformation();

    protected Transformable transformable;

    private void OnEnable()
    {
        DoTransformation();
    }

    private void OnDisable()
    {
        UndoDoTransformation();
    }

    private void OnDestroy()
    {
        UndoDoTransformation();
    }

}
