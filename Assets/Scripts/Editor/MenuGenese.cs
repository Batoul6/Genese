using UnityEditor;
using UnityEngine;

public static class MenuGenese
{
    const string dossierPoissons = "Assets/ScriptableObjects/Poissons";

    [MenuItem("Genese/Créer un type de poisson", false, 0)]
    static void CreerTypePoisson()
    {
        if (!AssetDatabase.IsValidFolder(dossierPoissons))
            AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Poissons");

        string chemin = AssetDatabase.GenerateUniqueAssetPath(dossierPoissons + "/Esp_Nouveau.asset");

        ObjetPoisson poisson = ScriptableObject.CreateInstance<ObjetPoisson>();
        AssetDatabase.CreateAsset(poisson, chemin);
        AssetDatabase.SaveAssets();

        Selection.activeObject = poisson;
        EditorGUIUtility.PingObject(poisson);
    }
}