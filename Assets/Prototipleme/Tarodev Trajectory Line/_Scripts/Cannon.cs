using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

    public static Cannon instance;
    [SerializeField] private Projection _projection;
    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private float _force = 20;
    [SerializeField] private Transform _ballSpawn;
    [SerializeField] private Transform _barrelPivot;
    [SerializeField] private Material redMaterial, greenMaterial;



    private void Awake()
	{
        if (instance == null) instance = this;
        else Destroy(this);
	}
	private void FixedUpdate() {
        _projection.SimulateTrajectory(_ballPrefab, _ballSpawn.position, _ballSpawn.forward * _force);
    }


    public IEnumerator IEMakeRed(GameObject obj)
	{
        yield return new WaitForSeconds(.3f);
        if(obj != null) obj.GetComponent<Collider>().enabled = true;
        GetComponent<LineRenderer>().material = redMaterial;
	}

    public void MakeGreen(GameObject obj)
	{
        GetComponent<LineRenderer>().material = greenMaterial;
        StartCoroutine(IEMakeRed(obj));
    }

    #region Handle Controls




    public void Fire()
	{
        var spawned = Instantiate(_ballPrefab, _ballSpawn.position, _ballSpawn.rotation);
        spawned.Init(_ballSpawn.forward * _force, false);

    }

    #endregion
}