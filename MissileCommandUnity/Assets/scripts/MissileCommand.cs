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

	public static float XYSqDistTo(this Vector3 a, Vector3 b)
	{
		return (a.x-b.x)*(a.x-b.x)+(a.y-b.y)*(a.y-b.y);
	}

	public static float XYSqDistTo(this Vector3 a, float x, float y)
	{
		return (a.x-x)*(a.x-x)+(a.y-y)*(a.y-y);
	}
}


public class MissileCommand : MonoBehaviour {

	public GameObject missile_prefab;
	public GameObject nuke_prefab;
	public GameObject detonation_prefab;
	public Base[] bases;

	private City[] cities;
	private List<Nuke> nukes = new List<Nuke>();

	public class Nuke {
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

	void Start ()
	{
		cities = FindObjectsOfType(typeof(City)) as City[];
	}

	private Base SelectFiringBase(float dx, float dy)
	{
		return bases
			.Where(b => b.missile_count > 0)
			.MinBy(b => transform.position.XYSqDistTo(dx, dy))
		;
	}
	
	void AddNuke() {
		Vector3 start = new Vector3(Random.Range(-700,700), 540, 0);
		City dest = cities[Random.Range(0,6)];
		Vector3 end = new Vector3(dest.transform.position.x, dest.transform.position.y, 0);
		nukes.Add(new Nuke(nuke_prefab, start, end, 3));
	}

	private float time = 0;
	void Update ()
	{
		// Add Nuke
		time += Time.deltaTime;
		if (time > Random.Range(0.5f, 1f)) {
			AddNuke();
			time = 0;
		}
	
		// Fire Missile
		if (Input.GetButtonDown("Fire1"))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
			Vector3 dst = new Vector3 (ray.origin.x, ray.origin.y, 35);
			Base b = SelectFiringBase(ray.origin.x, ray.origin.y);
			Vector3 s = new Vector3 (b.transform.position.x, b.transform.position.y, 35);
			if (b != null) { 
				((GameObject)Instantiate(missile_prefab, s, Quaternion.identity)).GetComponent<Missile>().FireAt(dst);
			}
		}

		for (int i=0; i<nukes.Count; ++i)
		{
			// Nuke movement
			Nuke nuke = nukes[i];
			nuke.Update();
		}

		// Nuke / City
		for (int i=0; i<nukes.Count; ++i)
		{
			Nuke nuke = nukes[i];
			foreach (City city in cities) {
				if (city.top > nuke.pos.y && city.left < nuke.pos.x && city.right > nuke.pos.x && city.bottom < nuke.pos.y) {
					city.Die();
					Destroy( nuke.go );
					nukes.RemoveAt(i);
					--i;
					break;
				}
			}
		}
	}
}
