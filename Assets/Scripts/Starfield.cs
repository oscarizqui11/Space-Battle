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

    [Header("Seed")]

    public int randomSeed;

    [Header("Planetas")]

    public int planetsMin;
    public int planetsMax;
    public float planetPosXMin;
    public float planetPosYMin;
    public float planetPosZMin;
    public float planetPosXMax;
    public float planetPosYMax;
    public float planetPosZMax;
    public float planetSizeXMin;
    public float planetSizeYMin;
    public float planetSizeZMin;
    public float planetSizeXMax;
    public float planetSizeYMax;
    public float planetSizeZMax;

    private int numPlanets;
    private float[,] planets;



    // Start is called before the first frame update
    void Start()
    {
        RandomizePlanets();
        

    }

    void OnDrawGizmos()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if(UnityEngine.Random.seed != randomSeed)
        {
            RandomizePlanets();
        }

        for (int i = 0; i < numPlanets; i++)
        {
            Vector3 position = new Vector3(planets[i, 0], planets[i, 1], planets[i, 2]);

            DrawPlanet(Matrix4x4.Translate(position), i);
        }
    }

    private void DrawPlanet(Matrix4x4 system)
    {
        Vector3 scale = new Vector3(Random(planetSizeXMin, planetSizeXMax),
                                    Random(planetSizeYMin, planetSizeYMax),
                                    Random(planetSizeZMin, planetSizeZMax));

        Graphics.DrawMesh(planet, system * Matrix4x4.Scale(scale), planetMaterials[0], 0);
    }
    private void DrawPlanet(Matrix4x4 system, int _planet)
    {
        Vector3 scale = new Vector3(planets[_planet, 3], planets[_planet, 4], planets[_planet, 5]);

        Graphics.DrawMesh(planet, system * Matrix4x4.Scale(scale), planetMaterials[0], 0);
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

    private void RandomizePlanets()
    {
        UnityEngine.Random.seed = randomSeed;
        numPlanets = Random(planetsMin, planetsMax);

        planets = new float[numPlanets, 6];

        for (int i = 0; i < numPlanets; i++)
        {
            planets[i, 0] = Random(planetPosXMin, planetPosXMax);
            planets[i, 1] = Random(planetPosYMin, planetPosYMax);
            planets[i, 2] = Random(planetPosZMin, planetPosZMax);
            planets[i, 3] = Random(planetSizeXMin, planetSizeXMax);
            planets[i, 4] = Random(planetSizeYMin, planetSizeYMax);
            planets[i, 5] = Random(planetSizeZMin, planetSizeZMax);
        }

        Debug.Log("Numero de planetas: " + planets.Length / 6);

    }

}
