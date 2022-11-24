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

    public float accelerationZ = 10;
    public float decelerationZ = 20;

    public float speedZMin = 50;
    public float speedZMax = 100;

    public float speedXMax = 50;
    public float speedYMax = 50;

    float targetAngleX;
    float targetAngleY;

    float angleX;
    float angleY;

    Rigidbody leftShotRigid;
    Rigidbody rightShotRigid;

    AudioSource leftShotSource;
    AudioSource rightShotSource;

    float speedZ;

    Starfield starfield;

    // Start is called before the first frame update
    void Start()
    {
        angleX = 0;
        angleY = 0;

        speedZ = 0;

        leftShotRigid = leftShot.GetComponent<Rigidbody>();
        rightShotRigid = rightShot.GetComponent<Rigidbody>();

        leftShotSource = leftShotSound.GetComponent<AudioSource>();
        rightShotSource = rightShotSound.GetComponent<AudioSource>();

        starfield = GameObject.FindWithTag("Starfield").GetComponent<Starfield>();
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

        if(Input.GetKey(KeyCode.Space))
        {
            speedZ += accelerationZ * Time.deltaTime;
            if(speedZ > speedZMax) { speedZ = speedZMax; }
        }
        else
        {
            speedZ -= decelerationZ * Time.deltaTime;
            if (speedZ < speedZMin) { speedZ = speedZMin; }
        }


        if (Input.GetMouseButtonDown(0))
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

        starfield.SetCameraSpeedZ(speedZ);

        Vector3 f = transform.forward;
        Vector3 pf = pivot.forward;
        Vector3 pfXZ = new Vector3(pf.x, 0, pf.z).normalized;
        Vector3 pfYZ = new Vector3(0, pf.y, pf.z).normalized;

        float horizontalFactor = 1 - Vector3.Dot(f, pfXZ);
        float verticalFactor = 1 - Vector3.Dot(f, pfYZ);

        starfield.SetCameraSpeedX(Mathf.Sign(pf.x) * speedXMax * horizontalFactor);
        starfield.SetCameraSpeedY(Mathf.Sign(pf.y) * speedYMax * verticalFactor);


    }


}
