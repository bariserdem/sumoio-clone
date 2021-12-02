using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class karakterKontrol : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed = 500;
    [SerializeField] private float forceMagnitude;
    private Touch _touch;
    private Vector3 _touchDown;
    private Vector3 _touchUp;
    private bool _dragStarted;
    private bool _isMoving;
    Boost boostManager;
    EnemyController enemyControl;
    public float size;
    public Text scoreText;
    public int score = 0;
    private GameObject[] enemyCount;
    public int enemyNumber;
    public Text playerCount;
    public GameObject end;
    public GameObject win;
    public GameObject lose;
    public Text[] hignScores;

    public void enemyCounter()
	{
        enemyNumber = 0;
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var GameObject in enemyCount)
        {
            enemyNumber++;
        }
        playerCount.text = enemyNumber.ToString();
    }


    // Start is called before the first frame update
    void Start()
    {
        boostManager = Object.FindObjectOfType<Boost>();
        playerCount.text = "8";
        //enmeyScoreList();
    }

    void enmeyScoreList()
    {
        for (int i = 1; i < 8; i++)
        {
            hignScores[i].text = "Enemy " + i + " " + PlayerPrefs.GetInt("Enemy_" + i).ToString();
        }
        hignScores[0].text = "Player " + score.ToString();
    }

	// Update is called once per frame
	void Update()
    {
        if(enemyNumber == 1)
		{
            end.SetActive(true);
            win.SetActive(true);
            StartCoroutine(time());
        }

        if(Input.touchCount > 0)
		{
            _touch = Input.GetTouch(0);
			if (_touch.phase == TouchPhase.Began)
			{
                _dragStarted = true;
                _isMoving = true;
                _touchDown = _touch.position;
                _touchUp = _touch.position;
			}
		}
		if (_dragStarted)
		{
			if (_touch.phase == TouchPhase.Moved)
			{
                _touchDown = _touch.position;
			}
			if (_touch.phase == TouchPhase.Ended)
			{
                _touchDown = _touch.position;
                _isMoving = false;
                _dragStarted = false;
            }
            gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, CalculateRotation(), rotationSpeed * Time.deltaTime);
            
		}
        gameObject.transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
        
    }

    Quaternion CalculateRotation()
	{
        Quaternion temp = Quaternion.LookRotation(CalculateDirection(), Vector3.up);
        return temp;
	}

    Vector3 CalculateDirection()
	{
        Vector3 temp = (_touchDown - _touchUp).normalized;
        temp.z = temp.y;
        temp.y = 0;
        return temp;
	}

	private void OnTriggerEnter(Collider trigger)
	{
        if(trigger.gameObject.tag == "Boost")
		{
            Destroy(trigger.gameObject);
            boostManager.SpawnBoost();
            boostManager.boostCount--;
            score = score + 100;
            scoreText.text = score.ToString();
            forceMagnitude = forceMagnitude + 0.1f;
            
            

            if (movementSpeed > 1.5f)
			{
                movementSpeed = movementSpeed - 0.1f;
            }

            transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y,transform.localScale.z);
            changeScale(size);
        }

        if (trigger.gameObject.tag == "grid")
        {
            end.SetActive(true);
            lose.SetActive(true);
            StartCoroutine(time());
        }
    }

    void reloadScene()
    {
        SceneManager.LoadScene("main");
    }

    IEnumerator time()
    {
        enmeyScoreList();
        yield return new WaitForSeconds(3);
        reloadScene();
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
	{
        Rigidbody rigitbody = hit.collider.attachedRigidbody;

        if(rigitbody != null)
		{
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            rigitbody.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
		}
	}


	void changeScale(float scaleChange)
	{
        Vector3 change = new Vector3(scaleChange, scaleChange, scaleChange);
        transform.localScale += change;
	}

}
