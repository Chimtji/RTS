using UnityEngine;

public class Unit : MonoBehaviour, ISelectable
{
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void OnDeselect()
    {

    }

    public void OnSelect()
    {

    }
}