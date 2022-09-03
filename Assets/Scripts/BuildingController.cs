using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private GameObject producedUnit;
    [SerializeField] public GameObject spawnPos;
    [SerializeField] public GameObject destinationPos;
    [SerializeField] public string buildingName;
    [SerializeField] public int imageIndex;

    public void SetDestinationPoint(Vector2 position)
    {
        destinationPos.transform.SetPositionAndRotation(position, Quaternion.identity);
    }

    public void ProduceUnit()
    {
        GameObject newUnit = Instantiate(producedUnit, spawnPos.transform.position, Quaternion.identity);
        UnitController u = newUnit.GetComponent<UnitController>();
        u.SetAgent();
        Vector2 newDestinationPos = GameManager.Instance.SetDestinationPos(destinationPos.transform.position);
        u.Move(newDestinationPos);
    }
    
}
