using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVisitedMap : MonoBehaviour
{
    public GameObject mapMarker;
    Image img;
    PlayerPosition pP;
    Sprite overrideSprite;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        img = GetComponent<Image>();
        pP = mapMarker.GetComponent<PlayerPosition>();
        while (pP.VisitedTex == null) yield return null;
        overrideSprite = Sprite.Create(pP.VisitedTex, new Rect(0.0f, 0.0f, pP.VisitedTex.width, pP.VisitedTex.height), new Vector2(0.5f, 0.5f), 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        img.overrideSprite = overrideSprite;
    }
}
