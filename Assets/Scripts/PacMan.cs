using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PacMan : MonoBehaviour {

	public GameObject _camera;
	public GameObject _pacmanModel;
	public GameObject messageCanvas;
	public Text messageText;
	public Text _scoreText;
	public Text _winText;

	private Rigidbody _rigidbody;
	private int _collectedBeansNum = 0;
    private int _killedGhostNum = 0;
	private int _totalBeansNum;

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody> ();
		_totalBeansNum = GameObject.FindGameObjectsWithTag ("Bean").Length;
		print ("total beans: " + _totalBeansNum);

	}
	
	// Update is called once per frame
	void Update () {
		UpdateStatusMessage ();
		transform.rotation = _camera.transform.rotation;
		_camera.transform.position = transform.position;
	}

	void FixedUpdate() {
		//		MoveByGvrController ();
		MoveByKeyboard();
	}

	void MoveByGvrController() {
		if (GvrController.State == GvrConnectionState.Connected) {
			if (GvrController.IsTouching) {
				Vector2 touchPos = GvrController.TouchPos;
				print ("touchPos: " + touchPos.x + "," + touchPos.y);
				Vector3 force = new Vector3 ();
				force.x = touchPos.x;
				force.z = touchPos.y;
				force.y = 0;
				_rigidbody.AddRelativeForce (force);
			}
		}
	}

	void MoveByKeyboard() {
		Vector3 force = new Vector3 ();
		force.z = Input.GetAxis ("Vertical");
		force.x = Input.GetAxis ("Horizontal");
		force.y = 0;
		_rigidbody.AddRelativeForce (force * 10);
	}

	private void UpdateStatusMessage() {
		// This is an example of how to process the controller's state to display a status message.
		switch (GvrController.State) {
		case GvrConnectionState.Connected:
			messageCanvas.SetActive(false);
			break;
		case GvrConnectionState.Disconnected:
			messageText.text = "Controller disconnected.";
			messageText.color = Color.white;
			messageCanvas.SetActive(true);
			break;
		case GvrConnectionState.Scanning:
			messageText.text = "Controller scanning...";
			messageText.color = Color.cyan;
			messageCanvas.SetActive(true);
			break;
		case GvrConnectionState.Connecting:
			messageText.text = "Controller connecting...";
			messageText.color = Color.yellow;
			messageCanvas.SetActive(true);
			break;
		case GvrConnectionState.Error:
			messageText.text = "ERROR: " + GvrController.ErrorDetails;
			messageText.color = Color.red;
			messageCanvas.SetActive(true);
			break;
		default:
			// Shouldn't happen.
			Debug.LogError("Invalid controller state: " + GvrController.State);
			break;
		}
	}

	void OnTriggerEnter(Collider collider) {
		string tag = collider.gameObject.tag;
		print ("OnTriggerEnter " + tag);
        if (tag.Equals("Bean")) {
            
            _collectedBeansNum++;
            updateScoreText();
			if (_collectedBeansNum >= _totalBeansNum) {
				_winText.enabled = true;
			}
			Destroy(collider.gameObject);
        } else if (tag.Equals("PowerBean")) {
			handleEatPowerBean(collider.gameObject);
        } else if (tag.Equals("Ghost")) {
            handleCollideGhost(collider.gameObject);
        }
	}

	void updateScoreText() {
        int score = _collectedBeansNum + _killedGhostNum * 2;
		_scoreText.text = "Score: " + score;
	}

	void handleEatPowerBean(GameObject powerBean) {
		Destroy (powerBean);
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in ghosts)
        {
            ghost.GetComponent<Ghost>().BeScared();
        }
    }

    void handleCollideGhost(GameObject ghost) {
        Ghost ghostController = ghost.GetComponent<Ghost>();
        if (ghostController.getState() == Ghost.State.NORMAL) {
            LossGame();
		} else if (ghostController.getState() == Ghost.State.SCARED) {
            _killedGhostNum++;
			ghostController.BeKilled();
        }
    }

	void LossGame() {
		SceneManager.LoadScene ("GameOver");
	}
}
