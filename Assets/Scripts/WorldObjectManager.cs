using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectManager : MonoBehaviour
{
    public GameObject PrefWO;
    public GameObject Player;
    public List<GameObject> WorldObjects = new List<GameObject>();
    float spawnDistance = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            print("spawning a world object");
            Vector3 spawnPos = new Vector3();
            GameObject cam = Player.transform.GetChild(0).gameObject;

            spawnPos = cam.transform.position + cam.transform.forward * spawnDistance;

            RaycastHit hitInfo;
            Physics.SphereCast(cam.transform.position, 1f, cam.transform.forward, out hitInfo, spawnDistance,
                            Physics.AllLayers, QueryTriggerInteraction.Ignore);

            if (hitInfo.point != Vector3.zero) spawnPos = hitInfo.point;

            GameObject go = Instantiate(PrefWO, spawnPos, Player.transform.rotation);
            WorldObjects.Add(go);
        }
    }
}
