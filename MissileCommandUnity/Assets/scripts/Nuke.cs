using UnityEngine;
using System.Collections;

public class Nuke : MonoBehaviour {

	public GameObject detonation_prefab;
	public float min_duration = 4.0f;
	public float max_duration = 6.0f;
	
	private LineRenderer lr;
	private Vector3 dir;

	void Start () {}

	public void FireAt(Vector3 dest)
	{
		float duration = Random.Range(min_duration, max_duration);
		lr = transform.FindChild("Line").GetComponent<LineRenderer>();
		lr.SetPosition(0, transform.position);
		lr.SetPosition(1, transform.position);
		dir = (dest-transform.position) / duration;
	}
	
	void Update()
	{
		transform.position += dir * Time.deltaTime;
		lr.SetPosition(1, transform.position);
	}

	private void Detonate()
	{
		Instantiate(detonation_prefab, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
