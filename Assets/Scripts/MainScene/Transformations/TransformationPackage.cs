using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationPackage : TransformationPhaseChange
{
    protected override Transformable.Phases phase => Transformable.Phases.PACKAGED;
}
