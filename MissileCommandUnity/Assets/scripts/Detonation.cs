using UnityEngine;
using System.Collections;

public class Detonation : MonoBehaviour {

	private Material mat;

	private static float lastSoundTime = -42.0f;

	void Start () {
		transform.eulerAngles = new Vector3(90, 0, 0);
		GetComponent<Animator>().SetTrigger("StartTrigger");

		float time = Time.realtimeSinceStartup;
		if (time - lastSoundTime >= 0.2f) { 
			GetComponent<AudioSource>().Play();
			lastSoundTime = time;
		}
		Destroy(gameObject, 5.0f);
	}
	
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Explosive") { 
			coll.gameObject.SendMessage("Detonate", SendMessageOptions.RequireReceiver);
		}
    }
}
