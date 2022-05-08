using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class scc : MonoBehaviour
{private float _initialYAngle = 0f;
    private float _appliedGyroYAngle = 0f;
    private float _calibrationYAngle = 0f;
    private Transform _rawGyroRotation;
    private float _tempSmoothing;
    private Vector3 pos;
    Vector3 forceVec;
    Rigidbody rb;
    float speed = 10.0f;
    int num =0;
    private float movementSpeed = 5f;


    // SETTINGS
    [SerializeField] private float _smoothing = 0.1f;

    private IEnumerator Start()
    {
        Input.gyro.enabled = true;
        Application.targetFrameRate = 60;
        _initialYAngle = transform.eulerAngles.y;

        _rawGyroRotation = new GameObject("GyroRaw").transform;
        _rawGyroRotation.position = transform.position;
        _rawGyroRotation.rotation = transform.rotation;

        // Wait until gyro is active, then calibrate to reset starting rotation.
        yield return new WaitForSeconds(1);

        StartCoroutine(CalibrateYAngle());
        rb = GetComponent<Rigidbody>();
    }
    private void MoveOnZ(float amount)
     {
         transform.position += transform.forward * amount;
     }

    private void Update()
    {
        pos = transform.position;
        if (num == 15)
        {
            if (pos.x < 0)
        {
            transform.position += new Vector3(UnityEngine.Random.Range(0.0f,1.0f), 0, 0);
        }
        if (pos.x > 200)
        {
            transform.position += new Vector3(UnityEngine.Random.Range(-1.0f,0.0f), 0, 0);
        }

        if (pos.z < 0)
        {
            transform.position += new Vector3(0, 0, UnityEngine.Random.Range(0.0f,0.2f));
        }
        if (pos.z > 200)
        {
            transform.position += new Vector3(0, 0, UnityEngine.Random.Range(-1.0f,0.0f));
        }
        num = 0;
        }
        else
        {
            num++;
        }
        ApplyGyroRotation();
        ApplyCalibration();

        transform.rotation = Quaternion.Slerp(transform.rotation, _rawGyroRotation.rotation, _smoothing);
        
       //transform.position = transform.position + new Vector3(0, 0, -Input.acceleration.z);
        //transform.Translate(new Vector3((lastLatitude-currentLatitude)*30 , 0, (lastLongitude-currentLongitude)*30));
        //float temp = Input.acceleration.z;
        //transform.Translate(new Vector3(0,0,-temp) * 0.8f );
        //transform.position = new Vector3((lastLatitude-currentLatitude)*30 , 0, (lastLongitude-currentLongitude)*30) * speed * Time.deltaTime;
        /*transform.position = pos + new Vector3((lastLatitude-currentLatitude)*30 , 0, (lastLongitude-currentLongitude)*30);
        pos = pos + new Vector3((lastLatitude-currentLatitude)*30 , 0, (lastLongitude-currentLongitude)*30);*/

        /*lastLatitude = currentLatitude;
        lastLongitude = currentLongitude;*/

         /*Vector3 dir = Vector3.zero;

        // we assume that device is held parallel to the ground
        // and Home button is in the right hand

        // remap device acceleration axis to game coordinates:
        //  1) XY plane of the device is mapped onto XZ plane
        //  2) rotated 90 degrees around Y axis
        dir.x = -Input.acceleration.y;
        dir.z = Input.acceleration.x;
        dir.y = Input.acceleration.z;

        // clamp acceleration vector to unit sphere
        if (dir.sqrMagnitude > 1)
            dir.Normalize();

        // Make it move 10 meters per second instead of 10 meters per frame...
        dir *= Time.deltaTime;

        // Move object
        transform.Translate(dir * speed);*/
       /* float temp = Input.acceleration.z;
       transform.Translate(0, 0, -Input.acceleration.z);*/
       //MoveOnZ(1f);
         float horizontalInput = Input.acceleration.x;
        //get the Input from Vertical axis
        float verticalInput = Input.acceleration.y;
        float zInput = Input.acceleration.z;
       
        if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            
             Vector3 acceleration = Vector3.zero;
            foreach (AccelerationEvent accEvent in Input.accelerationEvents)
            {
               
               acceleration += accEvent.acceleration * accEvent.deltaTime;
               Debug.Log(acceleration.z);
               if (acceleration.z > 0.000)
               {
                   transform.position += transform.forward * Time.deltaTime * movementSpeed;
               }
               /* (acceleration.z < -0.004)
               {
                   transform.position -= transform.forward * Time.deltaTime * movementSpeed;
               }*/
               //transform.position += new Vector3(0,0,acceleration.z * movementSpeed * Time.deltaTime);
               //transform.Translate(0, 0, -acceleration.z* Time.deltaTime);
               // transform.position += transform.forward * Time.deltaTime * movementSpeed;
                //transform.position += transform.forward * Time.deltaTime * movementSpeed * Input.acceleration.z;
            }

        }
       if (verticalInput < -1f && verticalInput > -0.9f)
       {
            transform.position += transform.forward * Time.deltaTime;
           //Vector3 acceleration = Vector3.zero;
           // acceleration += Input.acceleration * Time.deltaTime;
             //transform.position = transform.position + new Vector3(/*horizontalInput * movementSpeed * Time.deltaTime*/0, 0, -acceleration.z + movementSpeed * Time.deltaTime);
             //transform.Translate(0, 0, -acceleration.z + movementSpeed*Time.deltaTime);
            /*foreach (AccelerationEvent accEvent in Input.accelerationEvents)
           {
             acceleration += accEvent.acceleration * accEvent.deltaTime;
             transform.position = transform.position + new Vector3(0, 0, -acceleration.z + movementSpeed * Time.deltaTime);
             transform.Translate(0, 0, -acceleration.z + movementSpeed*Time.deltaTime);
           }*/

       //get the Input from Horizontal axis
      

        //update the position
        

        //output to log the position change
        //Debug.Log(transform.position);
        Debug.Log("*");
       }
        


       
       
    }

    private IEnumerator CalibrateYAngle()
    {
        _tempSmoothing = _smoothing;
        _smoothing = 1;
        _calibrationYAngle = _appliedGyroYAngle - _initialYAngle; // Offsets the y angle in case it wasn't 0 at edit time.
        yield return null;
        _smoothing = _tempSmoothing;
    }

    private void ApplyGyroRotation()
    {
        _rawGyroRotation.rotation = Input.gyro.attitude;
        _rawGyroRotation.Rotate(0f, 0f, 180f, Space.Self); // Swap "handedness" of quaternion from gyro.
        _rawGyroRotation.Rotate(90f, 180f, 0f, Space.World); // Rotate to make sense as a camera pointing out the back of your device.
        _appliedGyroYAngle = _rawGyroRotation.eulerAngles.y; // Save the angle around y axis for use in calibration.
    }

    private void ApplyCalibration()
    {
        _rawGyroRotation.Rotate(0f, -_calibrationYAngle, 0f, Space.World); // Rotates y angle back however much it deviated when calibrationYAngle was saved.
    }

    public void SetEnabled(bool value)
    {
        enabled = true;
        StartCoroutine(CalibrateYAngle());
    }
    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
        transform.position = pos;
    }


  
}
