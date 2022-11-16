using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform pivot;

    public Transform leftShotStartPoint;
    public Transform rightShotStartPoint;

    public Transform leftShotSound;
    public Transform rightShotSound;

    public Transform leftShot;
    public Transform rightShot;

    public float shotSpeed = 50;

    public float shotRange = 50;

    public float angleXRange = 45;
    public float angleYRange = 45;

    public float angleSpeedBase = 50;
    public float angleSpeedMultiplierDistance = 30;

    float targetAngleX;
    float targetAngleY;

    float angleX;
    float angleY;

    Rigidbody leftShotRigid;
    Rigidbody rightShotRigid;

    AudioSource leftShotSource;
    AudioSource rightShotSource;

    // Start is called before the first frame update
    void Start()
    {
        angleX = 0;
        angleY = 0;

        leftShotRigid = leftShot.GetComponent<Rigidbody>();
        rightShotRigid = rightShot.GetComponent<Rigidbody>();

        leftShotSource = leftShotSound.GetComponent<AudioSource>();
        rightShotSource = rightShotSound.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float aspect = Screen.width / (float) Screen.height;

        targetAngleX = (Input.mousePosition.y / Screen.height - 0.5f) * 2 * angleXRange;
        targetAngleY = (Input.mousePosition.x / Screen.width - 0.5f) * 2 * angleYRange * aspect;

        if (targetAngleX > angleXRange) { targetAngleX = angleXRange; }
        else if (targetAngleX < -angleXRange) { targetAngleX = -angleXRange; }

        if (targetAngleY > angleYRange) { targetAngleY = angleYRange; }
        else if (targetAngleY < -angleYRange) { targetAngleY = -angleYRange; }

        angleX += (targetAngleX - angleX) / angleSpeedMultiplierDistance * angleSpeedBase * Time.deltaTime;
        angleY += (targetAngleY - angleY) / angleSpeedMultiplierDistance * angleSpeedBase * Time.deltaTime;


        if(Input.GetMouseButtonDown(0))
        {
            leftShot.gameObject.SetActive(true);
            leftShotRigid.position = leftShotStartPoint.position;
            leftShotRigid.rotation = leftShotStartPoint.rotation;
            leftShotRigid.velocity = leftShotStartPoint.forward * shotSpeed;
            leftShotSource.PlayOneShot(leftShotSource.clip, 1.0f);
        }
        else
        {
            if ((leftShotRigid.position - transform.position).magnitude > shotRange) { leftShot.gameObject.SetActive(false); }
        }

        if (Input.GetMouseButtonDown(1))
        {
            rightShot.gameObject.SetActive(true);
            rightShotRigid.position = rightShotStartPoint.position;
            rightShotRigid.rotation = rightShotStartPoint.rotation;
            rightShotRigid.velocity = rightShotStartPoint.forward * shotSpeed;
            rightShotSource.PlayOneShot(rightShotSource.clip, 1.0f);

        }
        else
        {
            if ((rightShotRigid.position - transform.position).magnitude > shotRange) { rightShot.gameObject.SetActive(false); }
        }

        pivot.localRotation = Quaternion.Euler(0, angleY, 0) * Quaternion.Euler(angleX, 0, 0);


    }


}
