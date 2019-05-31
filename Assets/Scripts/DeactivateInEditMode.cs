using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS IS AN UGLY HACK TO PREVENT THE TERRAIN MEMORY LEAK IN THE SCENE VIEW FROM HAPPENING

[ExecuteInEditMode]
public class DeactivateInEditMode : MonoBehaviour
{
    Terrain t;

    private void Start () {
        t = GetComponent<Terrain>();
    }

    void Update()
    {
        if (Application.IsPlaying(gameObject)) {
            t.enabled = true;
        }
        else {
            t.enabled = false;
        }
    }
}
