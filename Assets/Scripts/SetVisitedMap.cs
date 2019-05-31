using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVisitedMap : MonoBehaviour
{
    public GameObject mapMarker;
    Image img;
    PlayerPosition pP;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        pP = mapMarker.GetComponent<PlayerPosition>();
    }

    // Update is called once per frame
    void Update()
    {
        img.overrideSprite = Sprite.Create(pP.VisitedTex, new Rect(0.0f, 0.0f, pP.VisitedTex.width, pP.VisitedTex.height), new Vector2(0.5f, 0.5f), 1.0f);
    }
}
