using UnityEngine;
using UnityEngine.Events;

// Suivi dynamique de la progression du joueur
public class ProgressionJoueur : MonoBehaviour
{
    [SerializeField] ReglagesProgression reglages;

    [Header("État actuel")]
    [SerializeField, Range(1, 5)] int stade = 1;
    [SerializeField] int xpTotale;
    [SerializeField] int xpForce;
    [SerializeField] int xpVitalite;
    [SerializeField] int xpVitesse;

    [Header("Événements")]
    public UnityEvent<int> quandXPChange;
    public UnityEvent<int> quandEvolution;

    [Header("Test")]
    [SerializeField] ObjetPoisson poissonTest;

    public int Stade => stade;
    public int XPTotale => xpTotale;


    // Manger = mettre a jour l'XP
    public void MangerPoisson(ObjetPoisson fiche)
    {
        if (fiche == null || reglages == null) return;

        float multiplicateur = reglages.Multiplicateur(fiche.stade, stade);
        int gain = Mathf.Max(1, Mathf.RoundToInt(fiche.xpDonnee * multiplicateur));

        xpTotale += gain;
        AjouterXPBranche(reglages.BrancheDe(fiche.regime), gain);
        quandXPChange.Invoke(xpTotale);

        VerifierEvolution();
    }
    // Vréfier si on a assez d'XP pour évoluer
    void VerifierEvolution()
    {
        int seuil = reglages.SeuilPourStadeSuivant(stade);
        while (seuil >= 0 && xpTotale >= seuil)
        {
            stade++;
            quandEvolution.Invoke(stade);
            seuil = reglages.SeuilPourStadeSuivant(stade);
        }
    }

    // Ajouter l'XP selon la branche du poisson qui donne l'XP
    void AjouterXPBranche(Branche branche, int quantite)
    {
        switch (branche)
        {
            case Branche.Force: xpForce += quantite; break;
            case Branche.Vitalite: xpVitalite += quantite; break;
            case Branche.Vitesse: xpVitesse += quantite; break;
        }
    }

    [ContextMenu("Test : manger le poisson test")]
    void TestManger()
    {
        MangerPoisson(poissonTest);
    }
}