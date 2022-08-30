using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private GameObject producedUnit;
    [SerializeField] private GameObject spawnPos;
    [SerializeField] private GameObject destinationPos;
    [SerializeField] public string buildingName;
    [SerializeField] public int imageIndex;
    
    private float timeCounter;

    // Start is called before the first frame update
    void Start()
    {
        timeCounter = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (producedUnit != null)
        {
            timeCounter -= Time.deltaTime;
            if (timeCounter <= 0)
            {
                GameObject newUnit = Instantiate(producedUnit, spawnPos.transform.position, Quaternion.identity);
                UnitController u = newUnit.GetComponent<UnitController>();
                u.SetAgent();
                u.Move(destinationPos.transform.position);
                timeCounter = 2f;
            }
        }
    }

    public void SetDestinationPoint(Vector3 position)
    {
        destinationPos.transform.SetPositionAndRotation(position, Quaternion.identity);
    }
}
