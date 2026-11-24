using UnityEngine;

public class SliderMinMaxAttribute : PropertyAttribute
{
    public float min;
    public float max;
    public Color couleur;

    public SliderMinMaxAttribute(float min, float max, string couleurHex = "#FF9ECF")
    {
        this.min = min;
        this.max = max;
        if (!ColorUtility.TryParseHtmlString(couleurHex, out couleur))
            couleur = new Color(1f, 0.62f, 0.81f);
    }
}