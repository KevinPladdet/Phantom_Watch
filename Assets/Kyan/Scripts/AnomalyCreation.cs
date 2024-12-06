using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalyCreation : MonoBehaviour
{
    public static AnomalyCreation instance;

    private void Awake()
    {
        if(instance == null) { instance = this; } else { Destroy(this); }
    }

    bool alive = true;
    [SerializeField] float _debugSpeed = 5;
    [SerializeField] bool _debugSpeedOn = false;
    
    [SerializeField] float startDelay = 30;
    [SerializeField] float timeBetweenSpawn = 10;
    [Range(0,100)]
    [SerializeField] float anomalyCreationPercentage = 20;
    [SerializeField] float anomalyCreationBuildup = 3;
    [SerializeField] float maxAnomlyCreationPercentage = 65;

    private void Start()
    {
        AnomalyController.instance.playerDead.AddListener(Die);
    }

    public void StartScript()
    {
        StartCoroutine(SpawnClock());
    }

    private void Update()
    {
        if (_debugSpeedOn) { Time.timeScale = _debugSpeed; }
    }

    void Die()
    {
        alive = false;
    }
    IEnumerator SpawnClock()
    {
        yield return new WaitForSeconds(startDelay);

        while (alive)
        {
            yield return new WaitForSeconds(timeBetweenSpawn * Random.Range(0.9f,1.1f));
            
            //Spawn depending on the ghostSpawnPercentage
            int randomNum = Random.Range(0, 100);
            if (randomNum < anomalyCreationPercentage)
            {
                AnomalyController.instance.CreateAnomaly();
            }
            else
            {
                if(anomalyCreationPercentage < maxAnomlyCreationPercentage)
                { 
                    anomalyCreationPercentage += anomalyCreationPercentage * anomalyCreationBuildup / 100;
                    if(anomalyCreationPercentage > maxAnomlyCreationPercentage)
                    {
                        anomalyCreationPercentage = maxAnomlyCreationPercentage;
                    }
                }
            }
        }
    }
}
