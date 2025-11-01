using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialBox : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    
    public GameObject bottomHighlight;
    public GameObject questHighlight;
    public GameObject messageHighlight;

    private int index;

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialog();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }

        if (textComponent.text == lines[1])
        {
            bottomHighlight.SetActive(true);
        }
        else
        {
            bottomHighlight.SetActive(false);
        }

        if (textComponent.text == lines[2])
        {
            questHighlight.SetActive(true);
        }
        else
        {
            questHighlight.SetActive(false);
        }

        if (textComponent.text == lines[3])
        {
            messageHighlight.SetActive(true);
        }
        else
        {
            messageHighlight.SetActive(false);
        }
    }

    void StartDialog()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c  in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
