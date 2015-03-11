using UnityEngine;
using System.Collections;

public class City : MonoBehaviour {

	public Material dead_city_mat;
	public GameObject city_explosion;
	public float left;
	public float right;
	public float top;
	public float bottom;

	private bool isDead = false;
	// Use this for initialization
	void Start () {
		BoxCollider2D bc = gameObject.AddComponent("BoxCollider2D") as BoxCollider2D;
		bc.bounds.SetMinMax(new Vector3(left, bottom, transform.position.z), new Vector3(right, (top+bottom)/2.0f, transform.position.z));
	}

	public bool IsDead()
	{
		return isDead;
	}

	private void SetMaterial()
	{
		renderer.material = dead_city_mat;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Explosive") { 
			coll.gameObject.SendMessage("Detonate", SendMessageOptions.RequireReceiver);
		}
		Die();
    }

	public void Die()
	{
		if (!isDead) {
			isDead = true;
			Destroy((GameObject)Instantiate(city_explosion, transform.position + new Vector3(0,0,-2), Quaternion.identity), 3);
			Invoke("SetMaterial", 0.5f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
