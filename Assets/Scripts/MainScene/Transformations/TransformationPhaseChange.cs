using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TransformationPhaseChange : Transformation
{
    private Transformable.Phases oldPhase = Transformable.Phases.INVALID;

    protected abstract Transformable.Phases phase { get; }

    private void Awake()
    {
        transformable = GetComponent<Transformable>();
    }

    protected override void DoTransformation()
    {
        // Store the previous phase the first time we do the transformation
        if (oldPhase == Transformable.Phases.INVALID)
        {
            oldPhase = transformable.GetCurrentPhase();
        }

        if (transformable.GetCurrentPhase() != phase)
        {
            transformable.ChangeToPhase(phase);
        }
    }

    protected override void UndoDoTransformation()
    {
        if (oldPhase == Transformable.Phases.INVALID)
        {
            return;
        }

        if (transformable.GetCurrentPhase() != oldPhase)
        {
            transformable.ChangeToPhase(oldPhase);
        }
    }
}
