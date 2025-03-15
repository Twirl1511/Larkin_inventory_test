using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testt : MonoBehaviour
{
    public float height = 10;
    public ItemSo _axe;
    public ItemSo _can;
    public ItemSo _bin;

    async void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            await ItemFactory.Instance.CreateItem(_axe, new Vector3(0, height, 0));
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            await ItemFactory.Instance.CreateItem(_can, new Vector3(0, height, 0));
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            await ItemFactory.Instance.CreateItem(_bin, new Vector3(0, height, 0));
        }
    }
}
