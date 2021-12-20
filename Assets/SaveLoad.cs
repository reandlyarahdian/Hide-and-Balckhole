using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoad : MonoBehaviour
{
    public Player player;
    public Changing changing;
    [SerializeField]
    Color boots, glassEye;
    string[] separator = new string[] { "RGBA(", ", ", ")" };

    private void Start()
    {
        player = FindObjectOfType<Player>();
        changing = FindObjectOfType<Changing>();
        Debug.Log(boots.ToString());
    }

    public void SaveFile()
    {
        boots = changing.bootsColor;
        glassEye = changing.glassColor;
        Data data = new Data
        {
            colorBoots = boots.ToString(),
            colorGlass = glassEye.ToString(),
            position = player.transform.position
            
        };

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.dataPath + "/save.txt", json);
    }

    public void LoadFile()
    {
        if(File.Exists(Application.dataPath + "/save.txt"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
            Data data = JsonUtility.FromJson<Data>(saveString);

            boots = ParseColor(data.colorBoots);
            glassEye = ParseColor(data.colorGlass);

            Debug.Log(boots);

            changing.ChangeColor(changing.Boot, changing.Boots, boots);
            changing.ChangeColor(changing.Glass, changing.GlassEye, glassEye);

            player.transform.position = data.position;
        }
        else
        {
            return;
        }
    }

    Color ParseColor(string text)
    {
        string[] test = text.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
        float r = float.Parse(test[0]);
        float g = float.Parse(test[1]);
        float b = float.Parse(test[2]);
        float a = float.Parse(test[3]);
        Debug.Log(test[0]);
        return new Color(r, g, b, a);
    }
}

public class Data
{
    public string colorBoots;
    public string colorGlass;
    public Vector3 position;
}
