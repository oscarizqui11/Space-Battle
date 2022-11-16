using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform exitPoint;
    public Transform targetPoint;
    public Transform enterPoint;

    public Transform[] bodyParts;
    public Transform explosion;
    public Transform explosionSound;

    public float limboTimeMax = 20;
    public float limboTimeMin = 5;

    public float holdTimeMax = 6;
    public float holdTimeMin = 3;


    public float speedBase = 50;
    public float speedMultiplierDistance = 10;

    public float stopRange = 0.5f;

    public float angleXRange = 45;
    public float angleYRange = 45;

    public float angleSpeedBase = 50;
    public float angleSpeedMultiplierDistance = 30;

    public float explosionStartScale = 1.0f;
    public float explosionEndScale = 5.0f;
    public float explosionTime = 1.0f;

    float targetAngleX;
    float targetAngleY;

    float angleX;
    float angleY;

    enum State
    {
        limbo,
        entering,
        hold,
        exiting,
        exploding
    }

    State state;
    State nextState;

    public bool hit;

    float limboTimer;
    float holdTimer;
    float explosionTimer;

    AudioSource explosionSource;

    // Start is called before the first frame update
    void Start()
    {
        explosionSource = explosionSound.GetComponent<AudioSource>();

        for(int i = 0; i < bodyParts.Length; i++) { bodyParts[i].gameObject.SetActive(false); }
        explosion.gameObject.SetActive(false);

        state = State.limbo;
        nextState = State.limbo;
        limboTimer = UnityEngine.Random.Range(limboTimeMin, limboTimeMax);
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.limbo)
        {
            limboTimer -= Time.deltaTime;

            if(limboTimer <= 0) { nextState = State.entering; }
        }
        else if(state == State.entering)
        {
            Vector3 difference = (targetPoint.position - transform.position);

            if (hit) { nextState = State.exploding; }
            else if (difference.magnitude < stopRange) { nextState = State.hold; }
            else
            {
                transform.position += difference.normalized * speedBase * difference.magnitude / speedMultiplierDistance * Time.deltaTime;
            }
        }
        else if (state == State.hold)
        {
            holdTimer -= Time.deltaTime;

            if(hit) { nextState = State.exploding; }
            else if (holdTimer <= 0) { nextState = State.exiting; }
        }
        else if (state == State.exiting)
        {
            Vector3 difference = (exitPoint.position - transform.position);

            if (hit) { nextState = State.exploding; }
            else if (difference.magnitude < stopRange) { nextState = State.limbo; }
            else
            {
                transform.position += difference.normalized * speedBase * difference.magnitude / speedMultiplierDistance * Time.deltaTime;
            }
        }
        else if(state == State.exploding)
        {
            explosion.transform.localScale = Vector3.one * (explosionStartScale + (explosionEndScale - explosionStartScale) * (explosionTime - explosionTimer) / explosionTime);

            explosionTimer -= Time.deltaTime;

            if(explosionTimer < 0) { nextState = State.limbo; }
        }

        if (state != nextState)
        {
            if (nextState == State.limbo)
            {
                for (int i = 0; i < bodyParts.Length; i++) { bodyParts[i].gameObject.SetActive(false); }
                explosion.gameObject.SetActive(false);

                limboTimer = UnityEngine.Random.Range(limboTimeMin, limboTimeMax);
            }
            else if (nextState == State.entering)
            {
                for(int i = 0; i < bodyParts.Length; i ++) { bodyParts[i].gameObject.SetActive(true); }
                transform.position = enterPoint.position;
                transform.rotation = enterPoint.rotation;
            }
            else if (nextState == State.hold)
            {
                holdTimer = UnityEngine.Random.Range(holdTimeMin, holdTimeMax);
            }
            else if (nextState == State.exploding)
            {
                explosionTimer = explosionTime;
                for (int i = 0; i < bodyParts.Length; i++) { bodyParts[i].gameObject.SetActive(false); }
                explosion.gameObject.SetActive(true);
                explosion.localScale = Vector3.one * explosionStartScale;
                explosionSource.PlayOneShot(explosionSource.clip, 1.0f);
            }

            state = nextState;
        }
        
        hit = false;
    }

    void OnCollisionEnter(Collision other)
    {
        hit = true;
    }
}
