using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour {

    public static Projection instance;

    [SerializeField] private LineRenderer _line;
    [SerializeField] private int _maxPhysicsFrameIterations = 100;
    [SerializeField] private Transform _obstaclesParent;

    private Scene _simulationScene;
    public PhysicsScene _physicsScene;
    public  Dictionary<Transform, Transform> _spawnedObjects = new Dictionary<Transform, Transform>();

	private void Awake()
	{
        if (instance == null) instance = this;
        else Destroy(this);
	}

	private void Start() {
        //CreatePhysicsScene();
    }

    public void CreatePhysicsScene() {
        _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _physicsScene = _simulationScene.GetPhysicsScene();

        foreach (Transform obj in _obstaclesParent) {
            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            ghostObj.GetComponentInChildren<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
            if (!ghostObj.isStatic) _spawnedObjects.Add(obj, ghostObj.transform);
        }
    }

    public void AddGhostToScene(Transform obj)
	{

			var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
			ghostObj.GetComponentInChildren<Renderer>().enabled = false;
			SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
			if (!ghostObj.isStatic) _spawnedObjects.Add(obj, ghostObj.transform);
	}

	private void Update() {
        foreach (var item in _spawnedObjects) {
            item.Value.position = item.Key.position;
            item.Value.rotation = item.Key.rotation;
            if(!item.Key.CompareTag("yansitici"))item.Value.localScale = item.Key.parent.localScale;
            //item.Value.GetComponent<BoxCollider>().center = item.Key.GetComponent<BoxCollider>().center;
            //item.Value.GetComponent<BoxCollider>().size = item.Key.GetComponent<BoxCollider>().size;
        }
    }

    public void SimulateTrajectory(Ball ballPrefab, Vector3 pos, Vector3 velocity) {
        var ghostObj = Instantiate(ballPrefab, pos, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, _simulationScene);

        ghostObj.Init(velocity, true);

        _line.positionCount = _maxPhysicsFrameIterations;

        for (var i = 0; i < _maxPhysicsFrameIterations; i++) {
            _physicsScene.Simulate(Time.fixedDeltaTime);
            _line.SetPosition(i, ghostObj.transform.position);
        }

        Destroy(ghostObj.gameObject);
    }

    public void RemoveSpawnedObject(GameObject obj)
	{
        _spawnedObjects.Remove(obj.transform);
	}

 //   public void SetSpawnedObjectsCollider(Transform obj , Vector3 colSet)
	//{
 //       obj.GetComponent<Collider>().
	//}
}