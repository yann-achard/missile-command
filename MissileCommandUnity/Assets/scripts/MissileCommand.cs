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
	
	private Base[] bases;
	private City[] cities;

	void Start ()
	{
		bases = FindObjectsOfType(typeof(Base)) as Base[];
		bases = bases.OrderBy(b => b.transform.position.x).ToArray();
		cities = FindObjectsOfType(typeof(City)) as City[];
	}

	private Base SelectFiringBase(float dx, float dy)
	{
		return bases
			.Where(b => b.missile_count > 0)
			.MinBy(b => b.transform.position.XYSqDistTo(dx, dy))
		;
	}
	
	void DropNuke()
	{
		Vector3 start = new Vector3(Random.Range(-700,700), 540, 0);
		City dest = cities[Random.Range(0,6)];
		Vector3 dst = new Vector3(dest.transform.position.x, dest.transform.position.y, 0);
		((GameObject)Instantiate(nuke_prefab, start, Quaternion.identity)).GetComponent<Nuke>().FireAt(dst);
	}

	private float time = 0;
	void Update ()
	{
		// Add Nuke
		time += Time.deltaTime;
		if (time > Random.Range(1.5f, 2f)) {
			DropNuke();
			time = 0;
		}
	
		// Fire Missile
		Base fb = null;
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			fb = bases[0];
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			fb = bases[1];
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			fb = bases[2];
		}
		if (fb != null) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
			Vector3 dst = new Vector3 (ray.origin.x, ray.origin.y, 35);
			Vector3 s = new Vector3 (fb.transform.position.x, fb.transform.position.y, 35);
			((GameObject)Instantiate(missile_prefab, s, Quaternion.identity)).GetComponent<Missile>().FireAt(dst);
		}
	}
}
