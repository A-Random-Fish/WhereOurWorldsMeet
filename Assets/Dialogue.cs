using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string[] lines;
    public float textSpeed;
    private int index;

    void Start()
    {
        text.text = string.Empty;
        StartDialogue(new string[] {"Hola","Meep"});
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue(string[] textToSay)
    {
        lines = textToSay;
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
            if (text.text == lines[index])
            {
                yield return new WaitForSeconds(1f);
                NextLine();
            }
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
