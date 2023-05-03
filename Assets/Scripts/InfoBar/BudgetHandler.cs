using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BudgetHandler : MonoBehaviour
{
    public TextMeshProUGUI budget;
    public string dollarSign;
    public int number;
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
