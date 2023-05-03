using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopulationHandler : MonoBehaviour
{
    public TextMeshProUGUI population;
    public string sprite;
    public int number;

    public PopulationHandler(string sprite, int number) {
        this.number = number;
        this.sprite = sprite;
    }

    public PopulationHandler(){}

    // Start is called before the first frame update
    void Start()
    {
        population = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // number++;
        population.text = sprite + number.ToString();
        // Debug.Log(population.text);
    }

    public void setPopulation(int x)
    {
        this.number = x;
    }
}
