using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : MonoBehaviour {

	public GameObject missile_prefab;
	public float left_wall;
	public float right_wall;
	public float ceiling;

	private List<Missile> missiles = new List<Missile>();

	class Missile {
		public Missile(GameObject prefab, Base b, Vector3 start, Vector3 direction)
		{
			homebase = b;
			go = (GameObject)Instantiate(prefab, start, Quaternion.identity);
			lr = go.transform.FindChild("Line").GetComponent<LineRenderer>();
			pos = start;
			go.transform.position = pos;
			lr.SetPosition(0, pos);
			lr.SetPosition(1, pos);
			dir = direction;
		}
		
		public void Update()
		{
			pos += dir * 500.0f * Time.deltaTime;
			go.transform.position = pos;
			lr.SetPosition(1, pos);
		}
		
		public bool HasFlownOff()
		{
			return pos.y >= homebase.ceiling || pos.x <= homebase.left_wall || pos.x >= homebase.right_wall;
		}
		
		public Base homebase;
		public GameObject go;
		public LineRenderer lr;
		public Vector3 pos;
		public Vector3 dir;
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
			Vector3 e = new Vector3 (ray.origin.x, ray.origin.y, 35);
			Vector3 s = new Vector3 (transform.position.x, transform.position.y, 35);
			missiles.Add(new Missile(missile_prefab, this, s, (e-s).normalized));
		}
		for (int i=0; i<missiles.Count; ++i)
		{
			missiles[i].Update();
			if (missiles[i].HasFlownOff()) {
				Destroy(missiles[i].go);
				missiles.RemoveAt(i);
				--i;
			}
		}
	}
}
