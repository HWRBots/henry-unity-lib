using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoDrive : MonoBehaviour
{
    public float Speed = 0.5f;
    private Sensor sensor;

    private List<int> stopDistances;
    private List<int> turnDistances;
    private List<int> freeDistancesLeft;
    private List<int> freeDistancesRight;

    private int problemSide = 0;

    /// <summary>
    /// robot can move forwards with three different velocities, stop and move backwards
    /// </summary>
    public enum WalkMode
    {
        BACKWARDS = -1,
        STOP = 0,
        FORWARDS_SLOW = 1,
        FORWARDS_MEDIUM = 2,
        FORWARDS_FAST = 3
    }

    /// <summary>
    /// robot can turn left and right in two intensities or move straight
    /// </summary>
    public enum TurnMode
    {
        LEFT_HARD = -2,
        LEFT_SMOOTH = -1,
        STRAIGHT = 0,
        RIGHT_SMOOTH = 1,
        RIGHT_HARD = 2
    }

    // Use this for initialization
    void Start()
    {
        sensor = transform.Find("Sensor").gameObject.GetComponent<Sensor>();
        //distances that cause an immediate stop
        stopDistances = new List<int>();
        for (var i = 0; i < 39; i++)
            stopDistances.Add(0);
        for (var i = 0; i < 193; i++)
            stopDistances.Add(300);
        for (var i = 0; i < 39; i++)
            stopDistances.Add(0);
        //distances that cause a turn
        turnDistances = new List<int>();
        for (var i = 0; i < 39; i++)
            turnDistances.Add(0);
        for (var i = 0; i < 193; i++)
            turnDistances.Add(500);
        for (var i = 0; i < 39; i++)
            turnDistances.Add(0);
        //distances free for random left
        freeDistancesLeft = new List<int>();
        for (var i = 0; i < 39; i++)
            freeDistancesLeft.Add(0);
        for (var i = 0; i < 97; i++)
            freeDistancesLeft.Add(550);
        for (var i = 0; i < 96; i++)
            freeDistancesLeft.Add(500);
        for (var i = 0; i < 39; i++)
            freeDistancesLeft.Add(0);
        //distances free for random right
        freeDistancesRight = new List<int>();
        for (var i = 0; i < 39; i++)
            freeDistancesRight.Add(0);
        for (var i = 0; i < 96; i++)
            freeDistancesRight.Add(500);
        for (var i = 0; i < 97; i++)
            freeDistancesRight.Add(550);
        for (var i = 0; i < 39; i++)
            freeDistancesRight.Add(0);
    }

    // Update is called once per frame
    void Update()
    {
        var currentWalkMode = WalkMode.FORWARDS_SLOW;
        var currentTurnMode = TurnMode.STRAIGHT;

        var sensorData = sensor.list;

        Debug.Log(problemSide);

        //if stop distances reached drive backwards
        if (CompareList(sensorData, stopDistances))
        {
            currentWalkMode = WalkMode.BACKWARDS;
        }
        else
        {
            //if turn distances not reached drive forwards
            if (!CompareList(sensorData, turnDistances))
            {
                if (!CompareList(sensorData, freeDistancesLeft) || !CompareList(sensorData, freeDistancesRight))
                {
                    currentTurnMode = sensorData.GetRange(45, 90).Average() > sensorData.GetRange(136, 90).Average() ? TurnMode.LEFT_SMOOTH : TurnMode.RIGHT_SMOOTH;
                }
            }
            else
            {
                currentWalkMode = WalkMode.STOP;
                //if there are more problems on the one side turn to the other side
                currentTurnMode = problemSide < 0 ? TurnMode.RIGHT_SMOOTH : TurnMode.LEFT_SMOOTH;
            }
        }
        transform.Translate((int)currentWalkMode * Vector3.forward * Speed * Time.deltaTime);
        transform.Rotate(Vector3.up, (int)currentTurnMode * Time.deltaTime * 25);
    }

    private bool CompareList(IList<int> data, IList<int> limits)
    {
        var max = data.Count > limits.Count ? limits.Count : data.Count;
        //how many problems are detected in comparison
        var count = 0;
        //loop through lists and compare if there are problems
        for (var i = 0; i < max; i++)
        {
            if (data[i] >= limits[i])
                continue;
            count++;
            if (i < 135)
            {
                if (problemSide > -15000)
                {
                    problemSide--;
                }
            }
            else
            {
                if (problemSide < 15000)
                {
                    problemSide++;
                }
            }
        }
        //if there are more problems than expected from measure failures than true (there are problems)
        return count >= 10;
    }
}


