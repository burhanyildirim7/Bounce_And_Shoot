using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour {

    public static Projection instance;

    [SerializeField] private LineRenderer _line;
    [SerializeField] private int _maxPhysicsFrameIterations = 100;
    [SerializeField] private Transform _obstaclesParent;

    public Scene _simulationScene;
    public PhysicsScene _physicsScene;
    public  Dictionary<Transform, Transform> _spawnedObjects = new Dictionary<Transform, Transform>();

	private void Awake()
	{
        if (instance == null) instance = this;
        else Destroy(this);
	}

	private void Start() {
        CreatePhysicsScene();
    }

    public void CreatePhysicsScene() {
        _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _physicsScene = _simulationScene.GetPhysicsScene();

        //foreach (Transform obj in _obstaclesParent) {
        //    var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
        //    ghostObj.GetComponentInChildren<Renderer>().enabled = false;
        //    SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
        //    if (!ghostObj.isStatic) _spawnedObjects.Add(obj, ghostObj.transform);
        //}
    }


    public void AddGhostToScene(Transform obj2)
	{
        var ghostObj2 = Instantiate(obj2.gameObject, obj2.position, obj2.rotation);
		ghostObj2.GetComponentInChildren<Renderer>().enabled = false;
        if (ghostObj2.CompareTag("engel"))
        {

            Renderer[] renderers = ghostObj2.transform.GetChild(0).GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
                r.enabled = false;
            //ghostObj2.transform.GetChild(0).GetComponentInChildren<Renderer>().enabled = false;
            Destroy(ghostObj2.GetComponent<Enemy>());
        }
		if (ghostObj2.CompareTag("tnt"))
		{
            ghostObj2.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            ghostObj2.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
		}
        if(ghostObj2.GetComponent<Collider>() != null)ghostObj2.GetComponent<Collider>().enabled = true;
		SceneManager.MoveGameObjectToScene(ghostObj2, _simulationScene);
		if (!ghostObj2.isStatic) _spawnedObjects.Add(obj2, ghostObj2.transform);

	}

    public void AddMeshCubeToScene()
	{
        foreach (Transform obj in _obstaclesParent)
        {
            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            ghostObj.GetComponentInChildren<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
            if (!ghostObj.isStatic) _spawnedObjects.Add(obj, ghostObj.transform);
        }
    }

    public void ClearForNewScene()
	{      
        foreach (GameObject obj in _simulationScene.GetRootGameObjects())
        {
           Destroy(obj.gameObject);
        }
        _spawnedObjects.Clear();
    }

	private void Update() {
		if (PlayerController.instance.isShootingTime)
		{
            foreach (var item in _spawnedObjects)
            {
                if(item.Value != null)
				{
                    item.Value.position = item.Key.position;
                    item.Value.rotation = item.Key.rotation;
                    if (!item.Key.CompareTag("yansitici") && !item.Key.CompareTag("engel") && !item.Key.CompareTag("yansitmayan") && !item.Key.CompareTag("tnt") && !item.Key.CompareTag("engel") ) item.Value.localScale = item.Key.parent.localScale;
                }         
               
                //if (item.Value != null && item.Key != null) item.Value.localScale = item.Key.parent.localScale;
                //if (!item.Key.CompareTag("yansitici") && !item.Key.CompareTag("engel")) item.Value.localScale = item.Key.parent.localScale;
            }
        }      
    }

    public void SimulateTrajectory(Ball ballPrefab, Vector3 pos, Vector3 velocity) {
        var ghostObj = Instantiate(ballPrefab, pos, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, _simulationScene);

        ghostObj.Init(velocity, true);

        _line.positionCount = _maxPhysicsFrameIterations;

        for (var i = 0; i < _maxPhysicsFrameIterations; i++) {
            _physicsScene.Simulate(Time.fixedDeltaTime);
            if (!ghostObj.transform.CompareTag("etkisiz")) _line.SetPosition(i, ghostObj.transform.position);
            else _line.SetPosition(i, _line.GetPosition(i-1));
        }

        Destroy(ghostObj.gameObject);
    }


    public void DeactivateGhostDuvar()
    {
        foreach (GameObject obj in _simulationScene.GetRootGameObjects())
        {
            if (obj.transform.CompareTag("duvar")) obj.GetComponent<Collider>().enabled = false;
        }
    }


    public void ActivateGhostDuvar()
	{
        foreach (GameObject obj in _simulationScene.GetRootGameObjects())
        {
            if (obj.transform.CompareTag("duvar")) obj.GetComponent<Collider>().enabled = true;
        }
    }
}