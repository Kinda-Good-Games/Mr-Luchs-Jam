using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void OnColorChanged(string key, bool value);
    public OnColorChanged onColorChanged;

    public List<ColorData> datas;
    private InputMaster controls;
    private Crossfade crossfade;
    private void Start()
    {
        crossfade = FindObjectOfType<Crossfade>();
        controls = FindObjectOfType<Player>().controls;

        controls.General.Reload.performed += _ => Reload();
    }
    private void Reload()
    {
        crossfade.LoadWrapper(SceneManager.GetActiveScene().buildIndex);
    }

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
