using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SatisfactionHandler : MonoBehaviour
{
    public TextMeshProUGUI satisfaction;
    public string text;
    public int number;
    public string warningText;
    // Start is called before the first frame update
    void Start()
    {
        satisfaction = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (number < 0)
        {
            satisfaction.text = text + number.ToString() +"\n "+ warningText;
        }
        else
        {
            satisfaction.text = text + number.ToString();
        }
        // Debug.Log(satisfaction.text);
    }
}
