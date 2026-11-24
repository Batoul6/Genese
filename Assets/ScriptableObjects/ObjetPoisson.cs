using UnityEngine;

// Définition des enum
public enum Regime { Carnivore, Herbivore, Omnivore, Special }
public enum Temperament { Peureux, Neutre, Agressif }

[CreateAssetMenu(fileName = "ObjetPoisson", menuName = "Genese/Poissons/Objet Poisson")]
public class ObjetPoisson : ScriptableObject
{
    [Header("Identité")]
    [Tooltip("Nom affiché dans le UI")]
    public string nomAffiche;
    [TextArea(2, 4)] public string description;
    [Range(1, 5)] public int stade = 1;
    public Regime regime;

    [Space(12)]
    [Header("Combat")]
    [Min(1f)] public float pvMax = 10f;
    [Min(0f)] public float degats = 1f;
    [Min(0)] public int xpDonnee = 1;
    [Tooltip("Temps en secondes entre deux attaques, tiré au hasard dans l'intervalle")]
    [SliderMinMax(0.5f, 5f, "#FF8A8A")] public Vector2 delaiAttaque = new Vector2(1f, 2f);

    [Space(12)]
    [Header("Comportement")]
    [Tooltip("Réaction quand le joueur est proche")]
    public Temperament temperament = Temperament.Neutre;
    [Range(1f, 30f)] public float distanceDetection = 6f;

    [Space(12)]
    [Header("Nage")]
    [SliderMinMax(0f, 10f, "#8FD3FE")] public Vector2 vitesse = new Vector2(1f, 3f);
    [Range(0.5f, 10f)] public float vitesseRotation = 4f;
    [Tooltip("Distance entre les poissons dans le flocking")]
    [Range(0.5f, 10f)] public float distanceVoisinage = 2f;

    [Space(12)]
    [Header("Apparition")]
    [Tooltip("Variation de grosseur")]
    [SliderMinMax(0.5f, 2f, "#C9A7FF")] public Vector2 echelle = new Vector2(0.9f, 1.1f);

    [Space(12)]
    [Header("Visuel")]
    public GameObject prefabModele;
    public Color couleurPlaceholder = Color.white;
}