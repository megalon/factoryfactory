using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationFlatten : Transformation
{
    protected override void DoTransformation()
    {
        transform.localScale = new Vector3(
            transform.localScale.x,
            transform.localScale.y * 0.25f,
            transform.localScale.z
        );
    }

    protected override void UndoDoTransformation()
    {
        transform.localScale = new Vector3(
            transform.localScale.x,
            transform.localScale.y * 4f,
            transform.localScale.z
        );
    }
}
