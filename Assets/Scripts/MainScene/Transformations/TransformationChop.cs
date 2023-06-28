using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationChop : TransformationPhaseChange
{
    protected override Transformable.Phases phase => Transformable.Phases.CHOPPED;
}
