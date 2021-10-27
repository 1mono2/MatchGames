using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseMatchBallController : MonoBehaviour
{
    Sensor sensorN;
    Sensor sensorS;
    Sensor sensorE;
    Sensor sensorW;

    internal enum Direction
    {
        N,
        S,
        E,
        W
    }
    int moveUnit = 2;

    // Start is called before the first frame update
    void Start()
    {
        sensorN = transform.Find("SensorN").GetComponent<Sensor>();
        sensorS = transform.Find("SensorS").GetComponent<Sensor>();
        sensorE = transform.Find("SensorE").GetComponent<Sensor>();
        sensorW = transform.Find("SensorW").GetComponent<Sensor>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void Move(Direction direction)
    {
        switch (direction)
        {
            case Direction.N:
                if (sensorN.isEnaleToGo)
                {
                    transform.position += new Vector3(0, 0, moveUnit);
                }
                break;
            case Direction.S:
                if (sensorS.isEnaleToGo)
                {
                    transform.position += new Vector3(0, 0, -moveUnit);
                }
                break;
            case Direction.E:
                if (sensorE.isEnaleToGo)
                {
                    transform.position += new Vector3(moveUnit, 0, 0);
                }
                break;
            case Direction.W:
                if (sensorW.isEnaleToGo)
                {
                    transform.position += new Vector3(-moveUnit, 0, 0);
                }
                break;

        }

    }

}
