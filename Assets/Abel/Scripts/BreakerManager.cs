using System.Collections;
using UnityEngine;

public class BreakerManager : MonoBehaviour
{
    public static BreakerManager Instance;

    [SerializeField] private float _initialTripFrequency = 10f; // Initial time between fuse breaks
    [SerializeField] private float _minTripFrequency = 4f; // Minimum time between fuse breaks
    [SerializeField] private int _tripChance = 3;

    [Space(10)]
    [SerializeField] private float _startDelay = 90f; // Delay before fuses start breaking
    public bool debug_BreakFuse;

    [Space(10)]
    [SerializeField] private GameObject _fusePrefab;
    [SerializeField] private GameObject[] _fuseObjects;

    private FuseSlot[] _fuseSlots;

    private float _elapsedTime = 0f; // Tracks the elapsed time in seconds
    private float _totalGameDuration = 720f; // Total game time in seconds (12 minutes)

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        foreach (GameObject obj in _fuseObjects)
        {
            obj.SetActive(false);
        }

        _fuseSlots = GetComponentsInChildren<FuseSlot>();
    }

    public void StartScript()
    {
        StartCoroutine(FuseTripperCoroutine());
    }

    private void Update()
    {
        if (debug_BreakFuse)
        {
            BreakRandomFuse();

            debug_BreakFuse = false;
        }

        // Track elapsed time
        _elapsedTime += Time.deltaTime;
    }

    private IEnumerator FuseTripperCoroutine()
    {
        // Wait for the start delay
        yield return new WaitForSeconds(_startDelay);

        while (_elapsedTime < _totalGameDuration)
        {
            // Calculate the current trip frequency based on elapsed time
            float currentTripFrequency = Mathf.Lerp(
                _initialTripFrequency,
                _minTripFrequency,
                (_elapsedTime - _startDelay) / (_totalGameDuration - _startDelay)
            );

            // Random chance to break a fuse 
            int rnd = Random.Range(0, _tripChance);
            if (rnd == 0)
            {
                BreakRandomFuse();
            }

            // Wait for the current trip frequency
            yield return new WaitForSeconds(currentTripFrequency);
        }

        Debug.Log("No more fuses will break, game time is over.");
    }

    private void BreakRandomFuse()
    {
        // Skip if all fuses are already broken
        int brokenCount = 0;
        foreach (FuseSlot fuseSlot in _fuseSlots)
        {
            if (fuseSlot.IsBroken) brokenCount++;
        }
        if (brokenCount >= _fuseSlots.Length)
        {
            return;
        }

        bool passed = false;
        int brokenFuseIndex = 0;

        while (!passed)
        {
            int rnd = Random.Range(0, _fuseSlots.Length);
            if (!_fuseSlots[rnd].IsBroken)
            {
                _fuseSlots[rnd].Break();
                brokenFuseIndex = rnd;
                passed = true;
            }
        }

        SpawnFuse();
    }

    private void SpawnFuse()
    {
        // Skip if all fuses are already active
        int activeFuseCount = 0;
        foreach (GameObject obj in _fuseObjects)
        {
            if (obj.activeInHierarchy)
                activeFuseCount++;
        }
        if (activeFuseCount >= _fuseSlots.Length)
            return;

        int rnd = 0;
        bool success = false;
        while (!success)
        {
            rnd = Random.Range(0, _fuseObjects.Length);

            if (!_fuseObjects[rnd].activeInHierarchy)
                success = true;
        }

        _fuseObjects[rnd].SetActive(true);
    }
}
