using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class ConstructionController : MonoBehaviour
{
    public BuildingData attributes;
    private ProgressBar progressBar;
    private GameObject progressBarObject;
    private GameObject visual;
    private BuildManager builder;

    private float buildTime
    {
        get
        {
            return attributes.health / 10;
        }
    }

    private float buildProgress = 0;

    void Awake()
    {
        BuildingShared shared = gameObject.GetComponent<BuildingShared>();
        this.attributes = shared.attributes;
        this.builder = shared.builder;

        transform.position = shared.buildPosition;

        StartConstruction();
    }

    private void Update()
    {
        UpdateBuildProgress();
    }

    private void StartConstruction()
    {
        visual = Instantiate(attributes.visual, transform, false);

        progressBarObject = Instantiate(Resources.Load<GameObject>("ProgressBar"), transform, false);
        progressBarObject.transform.localPosition = new Vector3(0, 4f, 0);
        progressBar = progressBarObject.GetComponent<ProgressBar>();
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
        Destroy(progressBarObject);
        Destroy(visual);
        builder.CreateBuilding(attributes, transform.localPosition);
        Destroy(this);
    }
}
