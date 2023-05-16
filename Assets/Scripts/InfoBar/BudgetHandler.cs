using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BudgetHandler : MonoBehaviour, IDataPersistence
{
    public TextMeshProUGUI budget;
    public string dollarSign;
    public int number;

    public void LoadData(GameData data)
    {
        this.number = data.budget;
    }

    public void SaveData(GameData data)
    {
        data.budget = this.number;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        budget.text = number.ToString() + dollarSign;
    }
    
}
