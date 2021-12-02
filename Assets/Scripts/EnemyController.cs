using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed = 500;
    public float size;
    NavMeshAgent enemy;
    //public Transform Player;
    private GameObject[] multipleBoost;
    public Transform closestBoost;
    Boost boostManager;
    karakterKontrol karakter;
    public int enemyScore = 0;
    public string enemyName;

    // Start is called before the first frame update
    void Start()
    {
        closestBoost = null;
        enemy = GetComponent<NavMeshAgent>();
        boostManager = Object.FindObjectOfType<Boost>();
        karakter = Object.FindObjectOfType<karakterKontrol>();
        enemyName = gameObject.name;
        PlayerPrefs.DeleteAll();
    }
    // Update is called once per frame
    void Update()
    {
        closestBoost = getClosestBoost();
        enemy.destination = closestBoost.position;
    }
    public Transform getClosestBoost()
	{
        multipleBoost = GameObject.FindGameObjectsWithTag("Boost");
        float closestDistance = Mathf.Infinity;
        Transform trans = null;

        foreach(GameObject go in multipleBoost)
		{
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, go.transform.position);
            if(currentDistance < closestDistance)
			{
                closestDistance = currentDistance;
                trans = go.transform;
			}
		}
        return trans;
	}

    public void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.tag == "Boost")
        {
            Destroy(trigger.gameObject);
            getClosestBoost();
            boostManager.SpawnBoost();
            boostManager.boostCount--;
            enemyScore = enemyScore + 100;
            PlayerPrefs.SetInt(enemyName, enemyScore);

            if (movementSpeed > 1.5f)
            {
                movementSpeed = movementSpeed - 0.1f;
            }
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            changeScale(size);
        }
        if(trigger.gameObject.tag == "grid")
		{
            Destroy(gameObject);
            karakter.score = karakter.score + 200;
            karakter.scoreText.text = karakter.score.ToString();
            karakter.enemyCounter();
        }
    }

    void changeScale(float scaleChange)
    {
        Vector3 change = new Vector3(scaleChange, scaleChange, scaleChange);
        transform.localScale += change;
    }

}
