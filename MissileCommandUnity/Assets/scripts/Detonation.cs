using UnityEngine;
using System.Reflection;
using System.Collections;

public class Detonation : MonoBehaviour {

	public float duration = 5.0f;

	private float timeSeed;
	private float timeLeft;
	private static float lastSoundTime = -42.0f;
	private MeshRenderer rend;
	private Light halo;
	private Color[] c = new Color[]{
		new Vector4(1.0f, 1.0f, 0.0f, 1),
		new Vector4(1.0f, 0.0f, 0.0f, 1),
		new Vector4(1.0f, 1.0f, 1.0f, 1),
		new Vector4(0.0f, 0.0f, 1.0f, 1),
		new Vector4(1.0f, 0.0f, 1.0f, 1),
		new Vector4(0.0f, 1.0f, 1.0f, 1),
		new Vector4(0.0f, 1.0f, 0.0f, 1),
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
		halo = GetComponent<Light>();
	
		Destroy(gameObject, duration);
	}
	
	void Update () {
		timeLeft -= Time.deltaTime;
		int t = (int)(timeSeed + Time.realtimeSinceStartup * 1000.0f);
		int u = (t/80) % c.Length;
		int f = t % 80;
		//Color col = Color.Lerp(c[u], c[(u+1) % c.Length], f / 50.0f);
		Color col = c[u];
		col.a = Mathf.Clamp01(timeLeft-1.2f);
		rend.material.color = col;
		halo.color = col;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Explosive") { 
			coll.gameObject.SendMessage("Detonate", SendMessageOptions.RequireReceiver);
		}
    }
}
