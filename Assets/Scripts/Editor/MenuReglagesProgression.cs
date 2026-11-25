using UnityEditor;
using UnityEngine;

// Script pour controler le nb de scriptableObjects ReglagesProgression pcq on en veut juste 1 a la fois
public static class MenuReglagesProgression
{
    const string dossier = "Assets/ScriptableObjects/Reglages";
    const string chemin = dossier + "/ReglagesProgression.asset";

    [MenuItem("Assets/Create/Genese/Progression/Reglages Progression", false, 0)]
    [MenuItem("Genese/Reglages Progression")]
    static void OuvrirOuCreer()
    {
        string[] trouves = AssetDatabase.FindAssets("t:ReglagesProgression");
        ReglagesProgression reglages;

        if (trouves.Length > 0)
        {
            string cheminTrouve = AssetDatabase.GUIDToAssetPath(trouves[0]);
            reglages = AssetDatabase.LoadAssetAtPath<ReglagesProgression>(cheminTrouve);

            if (trouves.Length > 1)
                Debug.LogWarning($"Il y a {trouves.Length} ReglagesProgression dans le projet, il devrait y en avoir juste un.");
        }
        else
        {
            if (!AssetDatabase.IsValidFolder(dossier))
                AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Reglages");

            reglages = ScriptableObject.CreateInstance<ReglagesProgression>();
            AssetDatabase.CreateAsset(reglages, chemin);
            AssetDatabase.SaveAssets();
        }

        Selection.activeObject = reglages;
        EditorGUIUtility.PingObject(reglages);
    }
}