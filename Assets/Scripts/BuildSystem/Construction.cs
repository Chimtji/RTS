using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class Construction : MonoBehaviour
{
    public TBuilding attributes;
    private GameObject building;
    private Vector3 position;

    private ProgressBar progressBar;

    private float buildTime
    {
        get
        {
            return attributes.health / 10;
        }
    }

    private float buildProgress = 0;

    private void Update()
    {
        UpdateBuildProgress();
    }

    public void Setup(TBuilding attributes, Vector3 position)
    {
        this.attributes = attributes;
        this.position = position;
        transform.position = position;

        StartConstruction();
    }

    private void StartConstruction()
    {
        building = Instantiate(attributes.visualConstruction, position, Quaternion.identity, transform);
        GameObject progressBarObj = Instantiate(Resources.Load<GameObject>("ProgressBar"), new Vector3(position.x, 4f, position.z), Quaternion.identity, transform);
        progressBar = progressBarObj.GetComponent<ProgressBar>();
        progressBar.SetMaxValue(buildTime);
        progressBar.SetValue(0);
    }

    private void UpdateBuildProgress()
    {
        buildProgress += Time.deltaTime;

        progressBar.SetValue(buildProgress);


        if (buildProgress >= buildTime)
        {
            FinishBuilding();
        }
    }

    private void FinishBuilding()
    {
        GameObject finishedBuilding = Instantiate(attributes.visual, position, Quaternion.identity);
        finishedBuilding.AddComponent<Building>().Setup(attributes);
        Destroy(gameObject);
    }
}
