using UnityEngine;
using System.Collections;

public class Detonation : MonoBehaviour {

	private Material mat;

	void Start () {
		transform.eulerAngles = new Vector3(90, 0, 0);
		GetComponent<Animator>().SetTrigger("StartTrigger");
		Destroy(gameObject, 3.2f);
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
