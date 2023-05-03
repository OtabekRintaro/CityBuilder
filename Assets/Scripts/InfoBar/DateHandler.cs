using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DateHandler : MonoBehaviour, IDataPersistence
{
    public TextMeshProUGUI dateText;
    public Button pauseButton;
    public Button resumeButton;
    public Button increaseSpeedButton;
    public Button decreaseSpeedButton;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI pausedText;
    public bool isPaused = false;
    public DateTime currentDate;
    //public static DateHandler inst;
    //private float lastUpdateTime;
    //private float updateRate= 2f;

    private float timeScale = 1f;

    IEnumerator IncreaseDate()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f / timeScale);
            if (!isPaused)
            {
                currentDate = currentDate.AddDays(1);
                dateText.text = currentDate.ToString("yyyy/MM/dd");
            }
        }
    }
    // Start is called before the first frame update

    void Start()
    {
        currentDate = new DateTime(1900, 1, 1);
        dateText.text = currentDate.ToString("yyyy/MM/dd");
        StartCoroutine(IncreaseDate());

        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
        increaseSpeedButton.onClick.AddListener(IncreaseSpeed);
        decreaseSpeedButton.onClick.AddListener(DecreaseSpeed);

        resumeButton.gameObject.SetActive(false);
        pausedText.gameObject.SetActive(false);
        UpdateSpeedText();
    }
    void Pause()
    {
        isPaused = true;
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(true);
        increaseSpeedButton.gameObject.SetActive(false);
        decreaseSpeedButton.gameObject.SetActive(false);
        pausedText.gameObject.SetActive(true);
    }

    void Resume()
    {
        isPaused = false;
        pauseButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);
        pausedText.gameObject.SetActive(false);
        if (timeScale == 2f)
        {
            decreaseSpeedButton.gameObject.SetActive(true);
        }
        else if (timeScale == 0.5f)
        {
            increaseSpeedButton.gameObject.SetActive(true);
        }
        else
        {
            decreaseSpeedButton.gameObject.SetActive(true);
            increaseSpeedButton.gameObject.SetActive(true);
        }
        
    }

    void IncreaseSpeed()
    {
        timeScale *= 2f;
        UpdateSpeedText();
        if (timeScale >= 2f)
            increaseSpeedButton.gameObject.SetActive(false);
        if (!decreaseSpeedButton.IsActive())
            decreaseSpeedButton.gameObject.SetActive(true);
    }

    void DecreaseSpeed()
    {
        timeScale /= 2f;
        UpdateSpeedText();
        if (timeScale <= 0.5f)
            decreaseSpeedButton.gameObject.SetActive(false);
        if (!increaseSpeedButton.IsActive())
            increaseSpeedButton.gameObject.SetActive(true);
    }
    void UpdateSpeedText()
    {
        speedText.text = $"Speed: {timeScale}x";
    }

    public void LoadData(GameData data)
    {
        this.currentDate = data.currDate;
    }

    public void SaveData(GameData data)
    {
        data.currDate = this.currentDate;
    }
    //public DateTime GetCurrentTime()
    //{
    //    return currentDate;
    //}

    //public void SetCurrentTime(DateTime time)
    //{
    //    currentDate = time;
    //}
    // Update is called once per frame
    //void Update()
    //{
    //    if(Time.time - lastUpdateTime> updateRate) {
    //        lastUpdateTime = Time.time;
    //    }
    //}

    public bool hasPassed5Seconds(DateTime date)
    {
        int day1 = currentDate.Day; int day2 = date.Day;
        return day1 - day2 >= 5;
    }
}
