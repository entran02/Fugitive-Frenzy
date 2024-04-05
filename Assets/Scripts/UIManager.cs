using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject warningPanel;
    public float flashDuration = 1f;

    private void Start()
    {
        
    }

    public void ShowWarning()
    {
        warningPanel.SetActive(true);
        Invoke("HideWarning", flashDuration);
    }

    private void HideWarning()
    {
        warningPanel.SetActive(false);
    }
}
