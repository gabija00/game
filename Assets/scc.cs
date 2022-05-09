using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class scc : MonoBehaviour
{private float _initialYAngle = 0f;
    private float _appliedGyroYAngle = 0f;
    private float _calibrationYAngle = 0f;
    private Transform _rawGyroRotation;
    private float _tempSmoothing;
    private Vector3 pos;
    Vector3 forceVec;
    Rigidbody rb;
    GameObject obj = null;
    int num =0;
    private float movementSpeed = 5f;
    string[] objNames = {"Kryziuotis_baltas1", "Kryziuotis_baltas2", "Kryziuotis_baltas3","Kryziuotis_baltas4","Kryziuotis_baltas5", "Kryziuotis_juodas1","Kryziuotis_juodas2","Kryziuotis_juodas3","Kryziuotis_juodas4","Kryziuotis_juodas5"};
    

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
        Vector3 pos = transform.position;
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
        
       
        if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            
             Vector3 acceleration = Vector3.zero;
            foreach (AccelerationEvent accEvent in Input.accelerationEvents)
            {
               
               acceleration += accEvent.acceleration * accEvent.deltaTime;
               //Debug.Log(acceleration.z);
               if (acceleration.z > 0.001)
               {
                   transform.position += transform.forward * Time.deltaTime * movementSpeed;
               }
               Debug.Log(acceleration.y);
               if (acceleration.y > 0.000)
               {
                   Debug.Log("*");
                   transform.position += new Vector3(0, 0.5f * Time.deltaTime * movementSpeed, 0);
                   for (int i = 0; i < objNames.Length; i++)
                   {
                       obj = GameObject.Find(objNames[i]);
                       //Destroy(obj);
                       Vector3 objPos = obj.transform.position;
                       if (Math.Abs(pos.x-objPos.x) < 3.5 && Math.Abs(pos.z-objPos.z) < 3.5)
                       {
                           Debug.Log("****");
                           Destroy(obj);
                           objNames = objNames.Where((source, index) =>index != i).ToArray();
                       }
                   }
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
