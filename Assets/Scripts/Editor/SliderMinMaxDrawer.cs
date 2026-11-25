using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SliderMinMaxAttribute))]
public class SliderMinMaxDrawer : PropertyDrawer
{
    const float largeurChamp = 42f;
    const float hauteurPiste = 6f;
    const float taillePoignee = 14f;

    int poigneeActive;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.Vector2)
        {
            EditorGUI.LabelField(position, label.text, "[MinMax] marche seulement sur un Vector2");
            return;
        }

        SliderMinMaxAttribute attr = (SliderMinMaxAttribute)attribute;

        EditorGUI.BeginProperty(position, label, property);
        Rect zone = EditorGUI.PrefixLabel(position, label);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        float h = EditorGUIUtility.singleLineHeight;
        Rect champMin = new Rect(zone.x, zone.y, largeurChamp, h);
        Rect champMax = new Rect(zone.xMax - largeurChamp, zone.y, largeurChamp, h);
        Rect zoneSlider = new Rect(champMin.xMax + 10f, zone.y, champMax.x - champMin.xMax - 20f, h);

        Vector2 valeur = property.vector2Value;

        EditorGUI.BeginChangeCheck();
        valeur.x = EditorGUI.FloatField(champMin, valeur.x);
        valeur.y = EditorGUI.FloatField(champMax, valeur.y);
        valeur = Slider(zoneSlider, valeur, attr);
        if (EditorGUI.EndChangeCheck())
        {
            valeur.x = Mathf.Clamp(valeur.x, attr.min, attr.max);
            valeur.y = Mathf.Clamp(valeur.y, valeur.x, attr.max);
            property.vector2Value = valeur;
        }

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    Vector2 Slider(Rect zone, Vector2 valeur, SliderMinMaxAttribute attr)
    {
        int id = GUIUtility.GetControlID(FocusType.Passive, zone);
        Event e = Event.current;
        Rect piste = new Rect(zone.x, zone.center.y - hauteurPiste / 2f, zone.width, hauteurPiste);

        EditorGUIUtility.AddCursorRect(zone, MouseCursor.SlideArrow);

        switch (e.GetTypeForControl(id))
        {
            case EventType.MouseDown:
                if (e.button == 0 && zone.Contains(e.mousePosition))
                {
                    float dMin = Mathf.Abs(e.mousePosition.x - VersX(valeur.x, piste, attr));
                    float dMax = Mathf.Abs(e.mousePosition.x - VersX(valeur.y, piste, attr));
                    if (Mathf.Approximately(dMin, dMax))
                        poigneeActive = e.mousePosition.x > VersX(valeur.y, piste, attr) ? 1 : 0;
                    else
                        poigneeActive = dMin < dMax ? 0 : 1;

                    GUIUtility.hotControl = id;
                    valeur = Deplacer(valeur, e.mousePosition.x, piste, attr);
                    GUI.changed = true;
                    e.Use();
                }
                break;

            case EventType.MouseDrag:
                if (GUIUtility.hotControl == id)
                {
                    valeur = Deplacer(valeur, e.mousePosition.x, piste, attr);
                    GUI.changed = true;
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                if (GUIUtility.hotControl == id)
                {
                    GUIUtility.hotControl = 0;
                    e.Use();
                }
                break;

            case EventType.Repaint:
                Dessiner(piste, valeur, attr, GUIUtility.hotControl == id);
                break;
        }

        return valeur;
    }

    Vector2 Deplacer(Vector2 valeur, float x, Rect piste, SliderMinMaxAttribute attr)
    {
        float v = Mathf.Lerp(attr.min, attr.max, Mathf.InverseLerp(piste.x, piste.xMax, x));
        v = Mathf.Round(v * 100f) / 100f;

        if (poigneeActive == 0) valeur.x = Mathf.Min(v, valeur.y);
        else valeur.y = Mathf.Max(v, valeur.x);

        return valeur;
    }

    float VersX(float v, Rect piste, SliderMinMaxAttribute attr)
    {
        return Mathf.Lerp(piste.x, piste.xMax, Mathf.InverseLerp(attr.min, attr.max, v));
    }

    void Dessiner(Rect piste, Vector2 valeur, SliderMinMaxAttribute attr, bool actif)
    {
        Texture blanc = EditorGUIUtility.whiteTexture;
        Color fond = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.12f) : new Color(0f, 0f, 0f, 0.12f);
        float xMin = VersX(valeur.x, piste, attr);
        float xMax = VersX(valeur.y, piste, attr);

        GUI.DrawTexture(piste, blanc, ScaleMode.StretchToFill, true, 0f, fond, 0f, hauteurPiste / 2f);

        Rect plein = Rect.MinMaxRect(xMin, piste.y, xMax, piste.yMax);
        GUI.DrawTexture(plein, blanc, ScaleMode.StretchToFill, true, 0f, attr.couleur, 0f, hauteurPiste / 2f);

        Poignee(xMin, piste.center.y, attr.couleur, actif && poigneeActive == 0);
        Poignee(xMax, piste.center.y, attr.couleur, actif && poigneeActive == 1);
    }

    void Poignee(float x, float y, Color couleur, bool active)
    {
        float taille = active ? taillePoignee * 1.25f : taillePoignee;
        Rect r = new Rect(x - taille / 2f, y - taille / 2f, taille, taille);
        Texture blanc = EditorGUIUtility.whiteTexture;

        GUI.DrawTexture(r, blanc, ScaleMode.StretchToFill, true, 0f, Color.white, 0f, taille / 2f);
        GUI.DrawTexture(r, blanc, ScaleMode.StretchToFill, true, 0f, couleur, 3f, taille / 2f);
    }
}