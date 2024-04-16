using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateStats : MonoBehaviour
{
    public TextMeshProUGUI timePlayed;
    public TextMeshProUGUI totalDistance;
    public TextMeshProUGUI totalJumps;
    public TextMeshProUGUI totalNitro;
    public TextMeshProUGUI totalOil;
    public TextMeshProUGUI totalDeaths;

    // Start is called before the first frame update
    void Start()
    {
        timePlayed.text = "Minutes Played:     " + PlayerPrefs.GetFloat("TotalMinutes").ToString("F2");
        totalDistance.text = "Total Distance:     " + PlayerPrefs.GetFloat("TotalDistance").ToString("F2");
        totalJumps.text = "Total Jumps:     " + PlayerPrefs.GetInt("TotalJumps");
        totalNitro.text = "Nitro Collected:     " + PlayerPrefs.GetInt("TotalNitro");
        totalOil.text = "Oil Slick Collected:     " + PlayerPrefs.GetInt("TotalOil");
        totalDeaths.text = "Deaths:     " + PlayerPrefs.GetInt("TotalDeaths");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
