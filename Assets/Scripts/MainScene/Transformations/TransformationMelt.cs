using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationMelt : TransformationPhaseChange
{
    protected override Transformable.Phases phase => Transformable.Phases.POWDER;
}
