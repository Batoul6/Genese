using UnityEngine;

public class test : MonoBehaviour
{
    public ObjetPoisson objetPoisson;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float nage;
    void Start()
    {
        Debug.Log(objetPoisson.nomAffiche);
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.GetComponent<Transform>().position = vitesse;
    }
}
