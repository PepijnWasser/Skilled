using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class NonsensePrinter : MonoBehaviour
{
    public Image textPrefab;
    public Image content;

    public Vector2 speed;
    public Vector2 speedDuration;

    public List<string> nonsenseList;

    float secondCounter = 0;
    float secondCounterSpeed = 0;

    List<Text> textList = new List<Text>();

    float currentSpeedDuration;
    float currentSpeed;

    bool needToPrint = false;

    private void Awake()
    {
        MapGenerator.OnCompletion += EnablePrinting;
    }

    private void OnDestroy()
    {
        MapGenerator.OnCompletion -= EnablePrinting;
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 16; i++)
        {
            Image image = Instantiate(textPrefab, content.transform);
            textList.Add(image.GetComponent<NonsensePrefabManager>().text);
        }

        for(int i = 0; i < textList.Count; i++)
        {
            textList[i].text = nonsenseList[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        secondCounter += Time.deltaTime;
        secondCounterSpeed += Time.deltaTime;
        if (secondCounter > currentSpeed)
        {
            secondCounter = 0;
            for (int i = 0; i < 16; i++)
            {
                if (i == 15)
                {
                    textList[i].text = nonsenseList[0];
                    nonsenseList.RemoveAt(0);
                }
                else if (i == 0)
                {
                    nonsenseList.Add(textList[i].text);
                    textList[i].text = textList[i + 1].text;
                }
                else
                {
                    textList[i].text = textList[i + 1].text;
                }
            }
        }

        if (secondCounterSpeed > currentSpeedDuration)
        {
          //  int i = Random.Range(0, 2);
            currentSpeedDuration = 2;
            currentSpeedDuration = 2; // Random.Range(speedDuration.x, speedDuration.y);
            currentSpeed = 2;// Random.Range(speed.x, speed.y);
            secondCounterSpeed = 0;
        }

    }

    void EnablePrinting()
    {
        needToPrint = true;
    }
}
