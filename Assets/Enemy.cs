using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ball") && !other.GetComponent<Ball>()._isGhost) 
		{
			GetComponentInChildren<Animator>().SetTrigger("die");
			PlayerController.instance.IncreaseMovementNo();

			foreach (GameObject obj in Projection.instance._simulationScene.GetRootGameObjects())
			{
				if (Vector3.Distance(obj.transform.position , other.transform.position) <= 2f ) Destroy(obj);
			}
		}
		
	}
}
