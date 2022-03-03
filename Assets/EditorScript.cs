using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorScript : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		if(transform.CompareTag("targetpoint")) Gizmos.color = Color.blue;
		else if(transform.CompareTag("firstmovepoint")) Gizmos.color = Color.green;
		else if(transform.CompareTag("secondmovepoint")) Gizmos.color = Color.yellow;
		else if(transform.CompareTag("thirdmovepoint")) Gizmos.color = Color.red;

		Gizmos.DrawSphere(transform.position, 1);
	}
}
