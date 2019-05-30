using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetToStart : MonoBehaviour
{
    public GameObject posStart;

    public void Start () {
        Reset();
    }

    public void Reset()
    {
        this.GetComponent<CharacterController>().enabled = false;
        RaycastHit hitInfo;
        Physics.Raycast(posStart.transform.position, Vector3.down, out hitInfo, float.PositiveInfinity,
                        Physics.AllLayers, QueryTriggerInteraction.Ignore);
        transform.position = hitInfo.point;
        this.GetComponent<CharacterController>().enabled = true;
    }
}
