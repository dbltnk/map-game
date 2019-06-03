using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVisitedMap : MonoBehaviour
{
    public GameObject MapGenGO;
    Image img;
    Sprite overrideSprite;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        img = GetComponent<Image>();
        VisitedMapManager vMM = MapGenGO.GetComponent<VisitedMapManager>();
        while (vMM.VisitedTex == null) yield return null;
        overrideSprite = Sprite.Create(vMM.VisitedTex, new Rect(0.0f, 0.0f, vMM.VisitedTex.width, vMM.VisitedTex.height), new Vector2(0.5f, 0.5f), 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        img.overrideSprite = overrideSprite;
    }
}
