using UnityEngine;

public class Cannon : MonoBehaviour {

    public static Cannon instance;
    [SerializeField] private Projection _projection;


	private void Awake()
	{
        if (instance == null) instance = this;
        else Destroy(this);
	}
	private void Update() {
        _projection.SimulateTrajectory(_ballPrefab, _ballSpawn.position, _ballSpawn.forward * _force);
    }

    #region Handle Controls

    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private float _force = 20;
    [SerializeField] private Transform _ballSpawn;
    [SerializeField] private Transform _barrelPivot;

    /// <summary>
    /// This is absolute spaghetti and should not be look upon for inspiration. I quickly smashed this together
    /// for the tutorial and didn't look back
    /// </summary>
    private void HandleControls() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            var spawned = Instantiate(_ballPrefab, _ballSpawn.position, _ballSpawn.rotation);
            spawned.Init(_ballSpawn.forward * _force, false);
        }
    }

    public void Fire()
	{
        var spawned = Instantiate(_ballPrefab, _ballSpawn.position, _ballSpawn.rotation);
        spawned.Init(_ballSpawn.forward * _force, false);

    }

    #endregion
}