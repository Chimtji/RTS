using UnityEngine;

public abstract class Selectable : MonoBehaviour
{
    private bool selected = false;

    private void Update()
    {
        if (selected)
        {
            OnSelect();
        }

        OnUpdate();

    }

    protected virtual void OnUpdate() { }

    public void Select(bool value)
    {
        selected = value;
    }

    private void OnSelect()
    {
        if (selected)
        {
            return;
        }


    }

}