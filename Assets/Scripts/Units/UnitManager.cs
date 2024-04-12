using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    private GameObject dataManager;
    private DataManager trainer
    {
        get
        {
            return dataManager.GetComponent<DataManager>();
        }
    }

    public void Create(UnitData unit, Vector3 position)
    {
        GameObject unitObject = Instantiate(unit.visual, position, Quaternion.identity);
        unitObject.AddComponent<UnitController>();
        trainer.AddUnit(unitObject);
    }
}