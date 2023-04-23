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

    public int prefabCountPossible;

    float secondCounter = 0;
    float secondCounterSpeed = 0;

    List<Text> textList = new List<Text>();

    float currentSpeedDuration;
    float currentSpeed;

    bool needToPrint = false;

    private void Awake()
    {
        MapGenerator.onCompletion += EnablePrinting;
    }

    private void OnDestroy()
    {
        MapGenerator.onCompletion -= EnablePrinting;
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < prefabCountPossible; i++)
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

        if (needToPrint)
        {
            secondCounter += Time.deltaTime;
            secondCounterSpeed += Time.deltaTime;

            
            //printing lines
            if (secondCounter > currentSpeed)
            {
                secondCounter = 0;
                for (int i = 0; i < prefabCountPossible; i++)
                {
                    if (i == prefabCountPossible - 1)
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

            

            //setting speed
            if (secondCounterSpeed > currentSpeedDuration)
            {
                currentSpeedDuration = Random.Range(speedDuration.x, speedDuration.y);
                currentSpeed = 2;
                secondCounterSpeed = 0;
            }

        }
    }

    void EnablePrinting()
    {
        needToPrint = true;
    }
}
