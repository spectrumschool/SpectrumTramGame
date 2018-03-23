using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingItem : MonoBehaviour
{
	void Update ()
    {
        float yTranslation = Time.deltaTime * -1 * GameManager.instance.tramSpeed;
        transform.Translate(0, yTranslation,0);
	}
}
