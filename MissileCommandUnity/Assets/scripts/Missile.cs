using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {

	public GameObject detonation_prefab;
	public float left_wall;
	public float right_wall;
	public float ceiling;
			
	private Renderer rend;
	private LineRenderer lr;
	private Vector3 dir;
	private bool hasDetonated;
	private float distFuse;
	private static float SPEED = 800.0f;

	void Start () {}

	public void FireAt(Vector3 dest)
	{
		Vector3 trip = dest - transform.position;
		dir = trip.normalized;
		distFuse = trip.magnitude;
		lr = transform.FindChild("Line").GetComponent<LineRenderer>();
		lr.SetPosition(0, transform.position);
		lr.SetPosition(1, transform.position);
		rend = lr.GetComponent<Renderer>();
	}
		
	public void Update()
	{
		if (!hasDetonated) {
			float moveBy = SPEED * Time.deltaTime;
			distFuse -= moveBy;
			if (distFuse <= 0.0f) {
				Detonate();
			} else {
				transform.position += dir * moveBy;
				rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, Mathf.Max(0.0f,  rend.material.color.a - Time.deltaTime * 0.6f));
				lr.SetPosition(1, transform.position);

				if (HasFlownOff()) {
					Destroy(gameObject);
				}
			}
		}
	}
		
	private void Detonate()
	{
		hasDetonated = true;
		Instantiate(detonation_prefab, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
		
	public bool HasFlownOff()
	{
		return transform.position.y >= ceiling || transform.position.x <= left_wall || transform.position.x >= right_wall;
	}
}
