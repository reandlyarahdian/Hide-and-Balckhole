using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Changing : MonoBehaviour
{
    public Material GlassEye, Boots;
    public Image Glass, Boot;
    public Color glassColor, bootsColor;
    public GameObject glassObj, bootObj;
    public List<Color> colors = new List<Color>();
    int i, j;

    private void Start()
    {
        Glass.color = GlassEye.color;
        Boot.color = Boots.color;
    }

    public void PressedGlass()
    {
        glassColor = colors[i];
        ChangeColor(Glass, GlassEye, glassColor);
        i++;
        i = i > colors.Count -1 ? 0 : i;
    }

    public void PressedBoot()
    {
        bootsColor = colors[i];
        ChangeColor(Boot, Boots, bootsColor);
        j++;
        j = j > colors.Count -1 ? 0 : j;
    }

    public void ChangeColor(Image image, Material material, Color color)
    {
        image.color = color;
        material.color = color;
    }

    public void ToogleGlass(bool test)
    {
        glassObj.SetActive(test);
    }

    public void ToogleBoot(bool test)
    {
        bootObj.SetActive(test);
    }

    public void BlackholeScene()
    {
        SceneManager.LoadScene("BlackHole");
    }
    
    public void HideNSeekScene()
    {
        SceneManager.LoadScene("HideNSeek");
    }
}
