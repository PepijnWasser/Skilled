using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NonsensePprinter : MonoBehaviour
{
    public Image textPrefab;
    public Image content;

    public float speed = 0.05f;

    public List<string> nonsenseList;

    float secondCOunter = 0;

    List<Text> textList = new List<Text>();

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
        secondCOunter += Time.deltaTime;
        if(secondCOunter > 0.05)
        {
            secondCOunter = 0;
            for(int i = 0; i < 16; i++)
            {
                if(i == 15)
                {
                    textList[i].text = nonsenseList[0];
                    nonsenseList.RemoveAt(0);
                }
                else if(i == 0)
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
    }
}
