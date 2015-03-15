using UnityEngine;
using System.Collections;

public class Detonation : MonoBehaviour {

	public float duration = 5.0f;

	private float timeSeed;
	private float timeLeft;
	private static float lastSoundTime = -42.0f;
	private MeshRenderer rend;
	private Color[] c = new Color[]{
		Color.magenta, // new Vector4(0.8f, 0.8f, 0.1f, 1),
		Color.cyan, // new Vector4(0.1f, 0.8f, 0.8f, 1),
		Color.yellow, // new Vector4(0.1f, 0.8f, 0.8f, 1),
		Color.white, // new Vector4(0.1f, 0.8f, 0.8f, 1),
		Color.red, // new Vector4(1.0f, 0.1f, 0.1f, 1),
		//Color.` new Vector4(0.1f, 1.0f, 0.1f, 1),
	};

	void Start () {
		transform.eulerAngles = new Vector3(90, 0, 0);
		GetComponent<Animator>().SetTrigger("StartTrigger");

		float time = Random.Range(0 , 1000.0f);
		if (time - lastSoundTime >= 0.2f) {
			GetComponent<AudioSource>().Play();
			lastSoundTime = time;
		}
		timeLeft = duration;
		timeSeed = time;

		rend = GetComponent<MeshRenderer>();
//		Material mat = new Material(rend.material);
//		rend.material = mat;
	
		Destroy(gameObject, duration);
		//Destroy(mat, duration);
	}
	
	void Update () {
		timeLeft -= Time.deltaTime;
		int t = (int)(timeSeed + Time.realtimeSinceStartup * 1000.0f);
		int u = (t/50) % c.Length;
		int f = t % 50;
		Color col = Color.Lerp(c[u], c[(u+1) % c.Length], f / 50.0f);
		col.a = Mathf.Clamp01(timeLeft-1.2f);
		rend.material.color = col;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Explosive") { 
			coll.gameObject.SendMessage("Detonate", SendMessageOptions.RequireReceiver);
		}
    }
}
