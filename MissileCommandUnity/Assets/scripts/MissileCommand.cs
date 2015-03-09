using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods
{
	public static TSource MinBy<TSource, TMin>(this IEnumerable<TSource> source, System.Func<TSource, TMin> selector) where TMin : System.IComparable<TMin>
	{
			var first = source.FirstOrDefault();
			return source.Aggregate(first, (min, current) => selector(current).CompareTo(selector(min)) < 0 ? current : min);
	}
}


public class MissileCommand : MonoBehaviour {

	public GameObject missile_prefab;
	public float left_wall;
	public float right_wall;
	public float ceiling;
	public Base[] bases;

	private List<Missile> missiles = new List<Missile>();

	public class Missile
	{
		public Missile(GameObject prefab, MissileCommand mc, Vector3 start, Vector3 direction)
		{
			com = mc;
			go = (GameObject)Instantiate(prefab, start, Quaternion.identity);
			lr = go.transform.FindChild("Line").GetComponent<LineRenderer>();
			pos = start;
			go.transform.position = pos;
			lr.SetPosition(0, pos);
			lr.SetPosition(1, pos);
			dir = direction;
			rend = lr.GetComponent<Renderer>();
		}
		
		public void Update()
		{
			pos += dir * 500.0f * Time.deltaTime;
			rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, Mathf.Max(0.0f,  rend.material.color.a - Time.deltaTime * 0.6f));
			go.transform.position = pos;
			lr.SetPosition(1, pos);
		}
		
		public bool HasFlownOff()
		{
			return pos.y >= com.ceiling || pos.x <= com.left_wall || pos.x >= com.right_wall;
		}
		
		private Renderer rend;
		public MissileCommand com;
		public GameObject go;
		public LineRenderer lr;
		public Vector3 pos;
		public Vector3 dir;
	}

	void Start () {
	}

	private Base SelectFiringBase(float dx, float dy)
	{
		return bases
			.Where(b => b.missile_count > 0)
			.MinBy(b => (dx-b.transform.position.x)*(dx-b.transform.position.x)+(dy-b.transform.position.y)*(dy-b.transform.position.y))
		;
	}
	
	void Update ()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
			Vector3 e = new Vector3 (ray.origin.x, ray.origin.y, 35);
			Base b = SelectFiringBase(ray.origin.x, ray.origin.y);
			Vector3 s = new Vector3 (b.transform.position.x, b.transform.position.y, 35);
			if (b != null) { 
				missiles.Add(new Missile(missile_prefab, this, s, (e-s).normalized));
			}
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
