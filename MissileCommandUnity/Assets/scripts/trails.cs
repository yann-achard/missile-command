using UnityEngine;
using System.Collections;
using System;

public class trails : MonoBehaviour {

	public int width = 1920;
	public int height = 1080;

	private int pixSize;
	private Texture2D tex;
	private Material mat;
	private Color[] pixels;

	// Use this for initialization
	void Start () {
		tex = new Texture2D(width, height);
		mat = GetComponent<MeshRenderer>().material;
		mat.mainTexture = tex;
		tex.alphaIsTransparency = true;
		pixels = tex.GetPixels();
		pixSize = width * height;

		Color c = Color.green;
		SetTex(c);
	}

	void SetTex(Color val)
	{
		for (int i=0; i<pixSize; ++i) {
			pixels[i] = val;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
