using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ball : MonoBehaviour {
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private GameObject _poofPrefab;
    public bool _isGhost;

	private void Start()
	{
        StartCoroutine(DestroyMe());
	}

	public void Init(Vector3 velocity, bool isGhost) {
        _isGhost = isGhost;
        _rb.AddForce(velocity, ForceMode.Impulse);
    }

    public void OnCollisionEnter(Collision col) {
        if (_isGhost) {
           
            if (col.transform.CompareTag("yansitmayan")) {
                transform.tag = "etkisiz";
                Debug.Log(transform.tag);
            } 
            return; 
        }
        if (col.transform.CompareTag("yansitmayan")) Destroy(gameObject);
        Instantiate(_poofPrefab, col.contacts[0].point, Quaternion.Euler(col.contacts[0].normal));
        if (col.transform.CompareTag("duvar"))
        {
            col.transform.GetComponentInChildren<MeshRenderer>().enabled = false;
            col.transform.GetComponentInChildren<Collider>().enabled = false;
            Projection.instance.DeactivateGhostDuvar();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
		if (other.CompareTag("engel"))
		{
            other.GetComponent<Collider>().enabled = false;
            Cannon.instance.MakeGreen(other.gameObject);
		}
    }



	IEnumerator DestroyMe()
	{
        yield return new WaitForSeconds(5f);
        if(!_isGhost)Destroy(gameObject);
	}
    
}