using UnityEngine;
using System.Collections;

public class nuke : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3[] verts = GetComponent<MeshFilter>().mesh.vertices;
		for(int i=0;i<verts.Length;i++) {
			verts[i].y += 100.0f * Time.deltaTime;
		}
	}
}
