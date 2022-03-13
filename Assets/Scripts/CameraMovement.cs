using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
	#region SINGLETON
	public static CameraMovement instance;
	private void Awake()
	{
        if (instance == null) instance = this;
        else Destroy(this);
	}
	#endregion

	private GameObject Player;

	private void Start()
	{
		DOTween.Init();
	}

	public void MoveCameraToTarget1()
	{
		transform.DOMove(PlayerController.instance.cameraTarget1.position,.5f);
	}

	public void MoveCameraToTarget2()
	{
		transform.DOMove(PlayerController.instance.cameraTarget2.position, .5f);
	}


}
