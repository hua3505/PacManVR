using UnityEngine;

public class Ghost : MonoBehaviour {

    public int _scaredTime;

	private NavMeshAgent _agent;
	private GameObject _pacMan;
    private GameObject _normalGhost;
    private GameObject _scaredGhost;
    private State _state = State.NORMAL;

    public enum State
    {
        NORMAL,
        SCARED,
        DEAD,
    }

	// Use this for initialization
	void Start () {
		_agent = GetComponent<NavMeshAgent> ();
		_pacMan = GameObject.FindGameObjectWithTag ("PacMan");
        _normalGhost = transform.GetChild(0).gameObject;
        _scaredGhost = transform.GetChild(1).gameObject;

    }
	
	// Update is called once per frame
	void Update () {
		if (_agent.remainingDistance <= 0) {
			_agent.SetDestination (_pacMan.transform.position);
		}
	}

    public State getState() {
        return _state;
    }

    public void beScared()
    {
        _state = State.SCARED;
        _normalGhost.SetActive(false);
        _scaredGhost.SetActive(true);
        Invoke("toNormalState", _scaredTime);
        // run away
    }

    void toNormalState()
    {
        _normalGhost.SetActive(true);
        _scaredGhost.SetActive(false);
        _state = State.NORMAL;

    }
}
