using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void OnColorChanged(string key, bool value);
    public OnColorChanged onColorChanged;

    public List<ColorData> datas;

    public void SetData(string key, bool value)
    {
        var element = datas.Find(x => x.color == key);
        element.value = value;

        onColorChanged?.Invoke(key, value);
    }
}
[Serializable]
public class ColorData
{
    [field:SerializeField]
    public string color { private set; get; }
    public bool value = true;
}
