using UnityEngine;

public enum Branche { Aucune, Force, Vitalite, Vitesse }

[System.Serializable]
public struct LienRegimeBranche
{
    public Regime regime;
    public Branche branche;
}

public class ReglagesProgression : ScriptableObject
{
    [Header("Évolution")]
    [Tooltip("XP total pour atteindre les stades 2, 3, 4 et 5")]
    public int[] seuilsEvolution = { 20, 60, 140, 300 };

    [Space(12)]
    [Header("XP selon la taille")]
    [Tooltip("Multiplicateur du XP en fonction de la grosseur relative du poésson")]
    public AnimationCurve multiplicateurEcart = new AnimationCurve(
        new Keyframe(-4f, 0.1f),
        new Keyframe(0f, 1f),
        new Keyframe(4f, 3f));

    [Space(12)]
    [Header("Arbre de compétences")]
    public LienRegimeBranche[] liensRegimeBranche =
    {
        new LienRegimeBranche { regime = Regime.Carnivore, branche = Branche.Force },
        new LienRegimeBranche { regime = Regime.Herbivore, branche = Branche.Vitalite },
        new LienRegimeBranche { regime = Regime.Omnivore, branche = Branche.Vitesse },
    };

    [Space(12)]
    [Header("Mort")]
    public bool perdreXPDuStadeAuDeces = false;

    public int SeuilPourStadeSuivant(int stadeActuel)
    {
        int index = stadeActuel - 1;
        if (index < 0 || index >= seuilsEvolution.Length) return -1;
        return seuilsEvolution[index];
    }

    public float Multiplicateur(int stadePoisson, int stadeJoueur)
    {
        return Mathf.Max(0f, multiplicateurEcart.Evaluate(stadePoisson - stadeJoueur));
    }

    public Branche BrancheDe(Regime regime)
    {
        foreach (LienRegimeBranche lien in liensRegimeBranche)
        {
            if (lien.regime == regime) return lien.branche;
        }
        return Branche.Aucune;
    }
}