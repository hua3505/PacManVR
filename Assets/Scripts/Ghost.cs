using UnityEngine;
using System.Collections;

public class Ghost : MonoBehaviour {

	private NavMeshAgent _agent;
	private GameObject _pacMan;

	// Use this for initialization
	void Start () {
		_agent = GetComponent<NavMeshAgent> ();
		_pacMan = GameObject.FindGameObjectWithTag ("PacMan");
	}
	
	// Update is called once per frame
	void Update () {
		if (_agent.remainingDistance <= 0) {
			_agent.SetDestination (_pacMan.transform.position);
		}
	}
}
