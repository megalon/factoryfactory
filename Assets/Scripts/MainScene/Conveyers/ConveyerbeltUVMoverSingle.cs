using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerbeltUVMoverSingle : MonoBehaviour
{
    public static ConveyerbeltUVMoverSingle Instance;

    [SerializeField]
    private Material conveyerMaterial;

    [SerializeField]
    private float _speed = 0.5f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    void Update()
    {
        conveyerMaterial.mainTextureOffset = new Vector2(0, conveyerMaterial.mainTextureOffset.y - (_speed * Time.deltaTime));
    }

    public void SetSpeed(float speed)
    {
        if (Instance._speed == speed)
        {
            return;
        }

        Instance._speed = speed;
    }
}
