using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotationReset : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
