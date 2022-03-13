using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patlayici : MonoBehaviour
{
	public float colliderStartRadius = .5f;
	public float colliderLastRadius = 6f;
	public GameObject explodePrefab;

	private void Start()
	{
		GetComponent<SphereCollider>().radius = colliderStartRadius;
	}

	private void OnTriggerEnter(Collider other)
	{

		if (other.CompareTag("Ball"))
		{
			GetComponent<MeshRenderer>().enabled = false;
			GetComponent<SphereCollider>().radius = colliderLastRadius;
			Instantiate(explodePrefab,transform.position,Quaternion.identity);
			StartCoroutine(DestroyMe());
		}
	}

	IEnumerator DestroyMe()
	{
		yield return new WaitForSeconds(.2f);
		Destroy(gameObject);
	}
}
