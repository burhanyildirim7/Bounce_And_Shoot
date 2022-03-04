using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScaleMesh : MonoBehaviour
{
	private Vector3 firstPoint, lastPoint;
	int layerMask;
	public GameObject scalebleCube;

	private void Start()
	{
		layerMask = LayerMask.GetMask("projectile");
	}
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(Ray, out hit, layerMask))
			{
				if (hit.transform.CompareTag("plane"))
				{
					firstPoint = lastPoint = hit.point;
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

			if (Physics.Raycast(Ray, out hit, layerMask))
			{
				if (hit.transform.CompareTag("plane") && scalebleCube != null)
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
		if(Input.GetMouseButtonUp(0) && PlayerController.instance.isShootingTime)
		{
			PlayerController.instance.bulletCount--;
			UIController.instance.SetBulletImages();
			if (PlayerController.instance.bulletCount == 0) PlayerController.instance.LooseEvents();
			Cannon.instance.Fire();
		}
	}
}
