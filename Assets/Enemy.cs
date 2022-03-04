using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ball")) 
		{
			Debug.Log("yand�manam..");
			GetComponentInChildren<Animator>().SetTrigger("die");
			PlayerController.instance.IncreaseMovementNo();
		}
		
	}
}
