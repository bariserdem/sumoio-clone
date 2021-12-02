using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    private Rigidbody rb;
    public int boostCount;

    [SerializeField] private GameObject boostObj;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SpawnBoost();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SpawnBoost()
	{
		if (boostCount < 4)
		{
            for (int i = 0; i < 3; i++)
            {
                Vector3 boostPosition = new Vector3(Random.Range(-9f, 9f), 0.25f, Random.Range(9f, -9f));
                if ((boostPosition - transform.position).magnitude < 3)
                {
                    continue;
                }
                else
                {
                    Instantiate(boostObj, boostPosition, Quaternion.identity);
                    boostCount++;
                }
            }
        }
	}

	

}
