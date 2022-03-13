using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScaleMesh : MonoBehaviour
{
	private Vector3 firstPoint, lastPoint;
	LayerMask layerMask;
	public GameObject scalebleCube;

	private void Start()
	{
		layerMask = 1 << LayerMask.NameToLayer("projectile");
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(Ray, out hit, 100f, layerMask))
			{
				if (hit.transform.CompareTag("plane") && PlayerController.instance.isShootingTime)
				{
					firstPoint = lastPoint = hit.point;
					Projection.instance.ActivateGhostDuvar();
					scalebleCube.GetComponentInChildren<MeshRenderer>().enabled = true;
					scalebleCube.GetComponentInChildren<Collider>().enabled = true;
					scalebleCube.transform.position = firstPoint;
				}
			}
		}

		if (Input.GetMouseButton(0))
		{
			var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(Ray, out hit,100f, layerMask))
			{
				if (hit.transform.CompareTag("plane") && scalebleCube != null && PlayerController.instance.isShootingTime)
				{
					lastPoint = hit.point;
					Vector3 direction;
					if (lastPoint.z < firstPoint.z)
					{
						direction = (firstPoint - lastPoint).normalized;
						
					}
					else { 
						direction = (lastPoint - firstPoint).normalized;
						//scalebleCube.transform.localScale = new Vector3(scalebleCube.transform.localScale.x, scalebleCube.transform.localScale.y, (lastPoint.z - firstPoint.z));
						//scalebleCube.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
					}
					scalebleCube.transform.localScale = new Vector3(scalebleCube.transform.localScale.x, scalebleCube.transform.localScale.y, (lastPoint.z - firstPoint.z));
					scalebleCube.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
				}
			}
		}
		if(Input.GetMouseButtonUp(0) && PlayerController.instance.isShootingTime && !PlayerController.instance.isGodMode)
		{
			PlayerController.instance.bulletCount--;
			UIController.instance.SetBulletImages();
			if (PlayerController.instance.bulletCount == 0) StartCoroutine(PlayerController.instance.CheckForLoose());
			Cannon.instance.Fire();
		}
	}
}
