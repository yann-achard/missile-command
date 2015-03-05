using UnityEngine;
using System.Collections;
using System;

public class trails : MonoBehaviour {

	public int width = 1920;
	public int height = 1080;

	private int pixSize;
	private Texture2D tex;
	private Material mat;
	private IntPtr texPtr;

	// Use this for initialization
	void Start () {
		tex = new Texture2D(width, height);
		mat = new ProceduralMaterial();
		mat.mainTexture = tex;
		GetComponent<MeshRenderer> ().material = mat;
		tex.alphaIsTransparency = true;
		texPtr = tex.GetNativeTexturePtr();
		pixSize = width * height;

		SetTex((int)0xff0000ff);
	}

	void SetTex(int val)
	{
		for (int i=0; i<pixSize; ++i) {
			texPtr[i] = val;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
