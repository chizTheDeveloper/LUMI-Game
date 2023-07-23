

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ResourceGenerator : MonoBehaviour
{
    [Header("Spawn Setup")]
    [SerializeField] private FlockUnit flockUnitPrefab;
    [SerializeField] private int flockSize;

    [Header("Speed Setup")]
    [Range(0, 10)]
    [SerializeField] private float _minSpeed;
    public float minSpeed { get { return _minSpeed; } }
    [Range(0, 10)]
    [SerializeField] private float _maxSpeed;
    public float maxSpeed { get { return _maxSpeed; } }

    [Header("Detection Distances")]

    [Range(0, 10)]
    [SerializeField] private float _cohesionDistance;
    public float cohesionDistance { get { return _cohesionDistance; } }

    [Range(0, 10)]
    [SerializeField] private float _avoidanceDistance;
    public float avoidanceDistance { get { return _avoidanceDistance; } }

    [Range(0, 10)]
    [SerializeField] private float _aligementDistance;
    public float aligementDistance { get { return _aligementDistance; } }

    [Range(0, 10)]
    [SerializeField] private float _obstacleDistance;
    public float obstacleDistance { get { return _obstacleDistance; } }

    [Range(0, 100)]
    [SerializeField] private float _boundsDistance;
    public float boundsDistance { get { return _boundsDistance; } }


    [Header("Behaviour Weights")]

    [Range(0, 10)]
    [SerializeField] private float _cohesionWeight;
    public float cohesionWeight { get { return _cohesionWeight; } }

    [Range(0, 10)]
    [SerializeField] private float _avoidanceWeight;
    public float avoidanceWeight { get { return _avoidanceWeight; } }

    [Range(0, 10)]
    [SerializeField] private float _aligementWeight;
    public float aligementWeight { get { return _aligementWeight; } }

    [Range(0, 10)]
    [SerializeField] private float _boundsWeight;
    public float boundsWeight { get { return _boundsWeight; } }

    [Range(0, 100)]
    [SerializeField] private float _obstacleWeight;
    public float obstacleWeight { get { return _obstacleWeight; } }

    public FlockUnit[] allUnits { get; set; }
    BoxCollider spawnBound;

    private void Start()
    {
        spawnBound = GetComponent<BoxCollider>();
        spawnBound.isTrigger = true;
        GenerateUnits();

    }

    private void Update()
    {
        for (int i = 0; i < allUnits.Length; i++)
        {
            allUnits[i].MoveUnit();
        }
    }

    private void GenerateUnits()
    {
        allUnits = new FlockUnit[flockSize];
        for (int i = 0; i < flockSize; i++)
        {
            var randomVector = UnityEngine.Random.insideUnitSphere;
            randomVector = new Vector3(randomVector.x * (transform.localScale.x / 2), randomVector.y * (transform.localScale.y / 2), randomVector.z * (transform.localScale.z / 2));
            var spawnPosition = transform.position + randomVector;
            var rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);

            RaycastHit[] hit = Physics.RaycastAll(spawnPosition, new Vector3(0, -1000, 0));
            if (hit.Length != 0)
            {
                for (int j = 0; j < hit.Length; j++)
                {
                    if (hit[j].collider.gameObject.tag != "Spawner" && hit[j].collider.gameObject.tag != "Player")
                    {
                        spawnPosition = hit[j].point;
                        allUnits[i] = Instantiate(flockUnitPrefab, spawnPosition, rotation);
                        allUnits[i].transform.up = hit[j].normal;
                    }
                }

            }


            //allUnits[i].AssignFlock(this);
            // allUnits[i].InitializeSpeed(UnityEngine.Random.Range(minSpeed, maxSpeed));
        }
    }
}