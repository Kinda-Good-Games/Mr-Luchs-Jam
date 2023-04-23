using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField] private int timeBetweenChars;
    [SerializeField] private float timeBetweenText;
    [TextArea(3, 7)]
    [SerializeField] private List<string> text;
    [SerializeField] private TextMeshProUGUI textBox;
    private void Awake()
    {
        DoDisplay();
    }
    private async Task DoDisplay()
    {
        for (int i = 0; i < text.Count; i++)
        {
            textBox.text = "";
            Task task;
            task = DisplayText(text[i]);


            await Task.WhenAll(task);
        }

    }
    private async Task DisplayText(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            textBox.text += text[i];

            await Task.Delay(timeBetweenChars);
        }
    }
}
