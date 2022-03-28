using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public GameObject kardes;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ball") && !other.GetComponent<Ball>()._isGhost) 
		{
			Destroy(GetComponent<Collider>());
			//GetComponent<Collider>().enabled = false;
			GetComponentInChildren<Animator>().SetTrigger("die");
			if(kardes != null) kardes.GetComponentInChildren<Animator>().SetTrigger("die");
			
			PlayerController.instance.IncreaseMovementNo();
			MoreMountains.NiceVibrations.MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes.MediumImpact);
			GameController.instance.SetScore(50);
			if (kardes != null) GameController.instance.SetScore(50);
			foreach (GameObject obj in Projection.instance._simulationScene.GetRootGameObjects())
			{
				if (Vector3.Distance(obj.transform.position, other.transform.position) <= 2f) Destroy(obj);
			}
		}
		else if (other.CompareTag("tnt"))
		{
			GetComponent<Collider>().enabled = false;
			GetComponentInChildren<Animator>().SetTrigger("die");
			
			PlayerController.instance.IncreaseMovementNo();

			foreach (GameObject obj in Projection.instance._simulationScene.GetRootGameObjects())
			{
				if (Vector3.Distance(obj.transform.position, other.transform.position) <= 5f) Destroy(obj);
			}
		}
		
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("tnt"))
		{
			GetComponentInChildren<Animator>().SetTrigger("die");
			GetComponent<Collider>().enabled = false;
			PlayerController.instance.IncreaseMovementNo();

			foreach (GameObject obj in Projection.instance._simulationScene.GetRootGameObjects())
			{
				if (Vector3.Distance(obj.transform.position, other.transform.position) <= 5f) Destroy(obj);
			}
		}
	}
}
