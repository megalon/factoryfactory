using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavmeshPointGizmo : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
#endif
}
