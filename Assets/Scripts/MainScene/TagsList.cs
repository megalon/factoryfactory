using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagsList : MonoBehaviour
{
    [SerializeField]
    private List<string> _tagsList;

    public List<string> GetTagsList()
    {
        return _tagsList;
    }
}
