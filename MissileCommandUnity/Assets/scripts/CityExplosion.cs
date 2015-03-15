using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CityExplosion : MonoBehaviour {

	public GameObject boom_prefab;
	public int boom_count = 10;
	public float duration = 2;
	public float x_radius;
	public float y_radius;
	
	private float elapsedTime;
	private bool booming;
	private List<float> times;
	// Use this for initialization
	void Start () {
		booming = false;
		Ignite();
	}

	public void Ignite()
	{
		booming = true;
		elapsedTime = 0;
		times = new List<float>();
		for (int i=0; i<boom_count; ++i) {
			times.Add(Random.Range(0,duration));
		}
		times.Sort();
		GetComponent<AudioSource>().Play();
	}

	private void BoomNow()
	{
		Vector3 disp = new Vector3(Random.Range(-x_radius,x_radius), Random.Range(-y_radius,y_radius), 0);
		GameObject go = (GameObject)Instantiate(boom_prefab, transform.position+disp, Quaternion.identity);
		go.GetComponent<Animator>().SetTrigger("StartTrigger");
		Destroy(go, 1);
	}
	
	// Update is called once per frame
	void Update () {
		if (booming) {
			if (times.Count == 0) {
				booming = false;
			} else {
				elapsedTime += Time.deltaTime;
				if (elapsedTime >= times[0]) {
					BoomNow();
					times.RemoveAt(0);
				}
				if (elapsedTime >= duration)
				{
					GetComponent<AudioSource>().Stop();
					Destroy(gameObject);
				}
			}
		}
	}
}
