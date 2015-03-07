using UnityEngine;
using System.Collections;

public class City : MonoBehaviour {

	public Material dead_city_mat;
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

	public void Die()
	{
		if (!isDead) {
			isDead = true;
			renderer.material = dead_city_mat;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
