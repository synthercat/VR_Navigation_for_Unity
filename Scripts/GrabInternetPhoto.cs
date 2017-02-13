using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabInternetPhoto : MonoBehaviour
{

    IEnumerator Start()
    {
		WWW www;
		yield return www = new WWW("https://vr.google.com/cardboard/images/hero-cardboard-download-mobile.jpg");
		Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = www.texture;
        www.Dispose();
    }

}
