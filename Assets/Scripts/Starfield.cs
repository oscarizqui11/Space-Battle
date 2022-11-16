using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starfield : MonoBehaviour
{
    [Header("Models")]

    public Mesh starship;
    public Mesh planet;
    public Mesh ring;
    public Mesh asteroid;
    public Mesh explosion;
    public Mesh shot;

    [Header("Materials")]

    public Material[] starshipMaterials;
    public Material[] planetMaterials;
    public Material[] ringMaterials;
    public Material[] asteroidMaterials;
    public Material[] shotMaterials;
    public Material[] explosionMaterials;



    // Start is called before the first frame update
    void Start()
    {
    }

    void OnDrawGizmos()
    {


    }

    // Update is called once per frame
    void Update()
    {
    }

    // Funciones auxiliares para simplificar
    // las llamadas al generador de números aleatorios

    int Random(int a, int b)
    {
        return UnityEngine.Random.Range(a, b + 1);
    }

    float Random(float a, float b)
    {
        return UnityEngine.Random.Range(a, b);
    }


}
