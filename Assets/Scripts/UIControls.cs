using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIControls : MonoBehaviour
{
    public static UIControls S;

    public GameObject successText;
    public Slider successBar;
    public GameObject SetupPanel;

    // Start is called before the first frame update
    void Start()
    {
        if (S != null)
        {
            Destroy(this);
        }
        else
        {
            S = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowSetupPanel()
    {
        SetupPanel.SetActive(true);
    }

    public void HideSetupPanel()
    {
        SetupPanel.SetActive(false);
    }

    public void ActivateSuccessText()
    {
        successText.SetActive(true);
    }
    public void HideSuccessText()
    {
        successText.SetActive(false);
    }

    public void ActivateSuccessBar()
    {
        successBar.gameObject.SetActive(true);
    }

    public void ConfigureSuccessBar(float successTime)
    {
        successBar.maxValue = successTime;
    }

    public void SetSuccessBar(float timeValue)
    {
        successBar.value = timeValue;
    }
}
