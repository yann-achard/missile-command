using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NukeManager : MonoBehaviour {

	public GameObject prefab;

	private List<Nuke> nukes = new List<Nuke>();

	class Nuke {
		public Nuke(GameObject prefab, Vector3 start, Vector3 end, float duration)
		{
			go = (GameObject)Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
			lr = go.transform.FindChild("Line").GetComponent<LineRenderer>();
			pos = start;
			go.transform.position = pos;
			endY = end.y;
			lr.SetPosition(0, pos);
			lr.SetPosition(1, pos);
			dir = (end-start) / duration;
		}

		public void Update()
		{
			pos += dir * Time.deltaTime;
			go.transform.position = pos;
			lr.SetPosition(1, pos);
		}

		public bool HasTouchedDown()
		{
			return pos.y <= endY;
		}

		public float endY;
		public GameObject go;
		public LineRenderer lr;
		public Vector3 pos;
		public Vector3 dir;
	}

	// Use this for initialization
	void Start () {
	}

	private float time = 0;
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time > Random.Range(0.5f, 3f)) {
			AddNuke();
			time = 0;
		}

		for (int i=0; i<nukes.Count; ++i)
		{
			Nuke nuke = nukes[i];
			nuke.Update();
			if (nuke.HasTouchedDown()) {
				Destroy( nuke.go );
				nukes.RemoveAt(i);
				--i;
			}
		}
	}

	void AddNuke() {
		Vector3 start = new Vector3(Random.Range(-700,700), 540, 0);
		Vector3 end = new Vector3(Random.Range(-850,850), -500, 0);
		nukes.Add(new Nuke(prefab, start, end, 5));
	}
}
