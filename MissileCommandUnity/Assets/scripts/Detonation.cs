using UnityEngine;
using System.Collections;

public class Detonation : MonoBehaviour {

	private Material mat;

	void Start () {
		GetComponent<Animator>().SetTrigger("StartTrigger");
		Destroy(gameObject, 4.0f);
	}
	
	void Update () {
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Explosive") { 
			coll.gameObject.SendMessage("Detonate");
		}
    }
}
