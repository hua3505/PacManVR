using UnityEngine;

public class Ghost : MonoBehaviour {

    public float _scaredTime;
    public float _startBlinkTimeAfterScared;
    public float _blinkInterval;

	private NavMeshAgent _agent;
	private GameObject _pacMan;
    private GameObject _normalGhost;
    private GameObject _scaredGhost;
	private GameObject _scaredGhostBody;
	private GameObject _scaredGhostMouth;
	private GameObject _base;
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
		_normalGhost = transform.FindChild("Ghost").gameObject;
		_scaredGhost = transform.FindChild("ScaredGhost").gameObject;
		_scaredGhostBody = _scaredGhost.transform.FindChild ("Body").gameObject;
		_scaredGhostMouth = _scaredGhost.transform.FindChild ("Mouth").gameObject;
		_base = GameObject.Find ("BaseOfGhost");

    }
	
	// Update is called once per frame
	void Update () {
		if (_agent.remainingDistance <= 0) {
			if (_state == State.NORMAL) {
				_agent.SetDestination (_pacMan.transform.position);
			} else if (_state == State.DEAD) {
				Reborn ();
			}
		}
	}

    public State getState() {
        return _state;
    }

    public void BeScared()
    {
        _state = State.SCARED;
        _normalGhost.SetActive(false);
        _scaredGhost.SetActive(true);
		CancelInvoke ("ToNormalState");
		Invoke("ToNormalState", _scaredTime);
        Invoke("Blink", _startBlinkTimeAfterScared);
        // run away
        if (_base != null) {
			_agent.SetDestination (_base.transform.position);
		}
    }

	public void BeKilled() {
        CancelInvoke("Blink");
        _state = State.DEAD;
		CancelInvoke ("ToNormalState");
		_scaredGhostBody.SetActive (false);
		_scaredGhostMouth.SetActive (false);
		// back to base to reborn
		if (_base != null) {
			_agent.SetDestination (_base.transform.position);
		}	
	}

    void ToNormalState()
    {
        CancelInvoke("Blink");
        print ("ToNormalState");
        _normalGhost.SetActive(true);
        _scaredGhost.SetActive(false);
        _state = State.NORMAL;

    }

    void Blink()
    {
        if (_state == State.SCARED)
        {
            if (_normalGhost.activeInHierarchy)
            {
                _normalGhost.SetActive(false);
                _scaredGhost.SetActive(true);
            }
            else
            {
                _normalGhost.SetActive(true);
                _scaredGhost.SetActive(false);
            }
            Invoke("Blink", _blinkInterval);
        }
    }

	void Reborn() {
		ToNormalState ();
		_scaredGhostBody.SetActive (true);
		_scaredGhostMouth.SetActive (true);
	}
}
