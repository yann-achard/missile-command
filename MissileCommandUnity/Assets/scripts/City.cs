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
	
	}

	public bool IsDead()
	{
		return isDead;
	}

	private void SetMaterial()
	{
		renderer.material = dead_city_mat;
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
