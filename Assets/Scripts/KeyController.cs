using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyController : MonoBehaviour
{
    public float Speed = 0.5f;
    private int count;
    public Text countText;
    public Text winText;

    /// <summary>
    /// Walkmode object is needed to set speed and direction of movement
    /// </summary>
    private WalkMode currentWalkMode;

    /// <summary>
    /// TurnMode object is needed to set intensity of lateral movement
    /// </summary>
    private TurnMode currentTurnMode;

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
        currentWalkMode = WalkMode.STOP;
        currentTurnMode = TurnMode.STRAIGHT;

        count = 0;
        SetCountText();
        winText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate((int)currentWalkMode * Vector3.forward * Speed * Time.deltaTime);
        transform.Rotate(Vector3.up, (int)currentTurnMode * Time.deltaTime * 25);


        //pressing up key switches from BACKWARDS to STOP to FORWARDS_SLOW to FORWARDS_MEDIUM to FORWARDS_FAST
        //once reached FORWARDS_FAST, pressing up key again will not have any effect
        if (Input.GetKeyDown(KeyCode.W))
        {
            switch (currentWalkMode)
            {
                case WalkMode.BACKWARDS:
                    currentWalkMode = WalkMode.STOP;
                    break;
                case WalkMode.STOP:
                    currentWalkMode = WalkMode.FORWARDS_SLOW;
                    break;
                case WalkMode.FORWARDS_SLOW:
                    currentWalkMode = WalkMode.FORWARDS_MEDIUM;
                    break;
                case WalkMode.FORWARDS_MEDIUM:
                    currentWalkMode = WalkMode.FORWARDS_FAST;
                    break;
            }
        }


        //pressing down key switches from FORWARDS_FAST to FORWARDS_MEDIUM to FORWARDS_SLOW to STOP to BACKWARDS
        //once reached BACKWARDS, pressing down key again will not have any effect
        if (Input.GetKeyDown(KeyCode.S))
        {
            switch (currentWalkMode)
            {
                case WalkMode.FORWARDS_FAST:
                    currentWalkMode = WalkMode.FORWARDS_MEDIUM;
                    break;
                case WalkMode.FORWARDS_MEDIUM:
                    currentWalkMode = WalkMode.FORWARDS_SLOW;
                    break;
                case WalkMode.FORWARDS_SLOW:
                    currentWalkMode = WalkMode.STOP;
                    break;
                case WalkMode.STOP:
                    currentWalkMode = WalkMode.BACKWARDS;
                    break;
            }
        }
        //pressing left key switches from RIGHT_HARD to RIGHT_SMOOTH to STRAIGHT to LEFT_SMOOTH to LEFT_HARD
        //once reached LEFT_HARD, pressing left key again will not have any effect
        if (Input.GetKeyDown(KeyCode.A))
        {
            switch (currentTurnMode)
            {
                case TurnMode.RIGHT_HARD:
                    currentTurnMode = TurnMode.RIGHT_SMOOTH;
                    break;
                case TurnMode.RIGHT_SMOOTH:
                    currentTurnMode = TurnMode.STRAIGHT;
                    break;
                case TurnMode.STRAIGHT:
                    currentTurnMode = TurnMode.LEFT_SMOOTH;
                    break;
                case TurnMode.LEFT_SMOOTH:
                    currentTurnMode = TurnMode.LEFT_HARD;
                    break;
            }
        }

        //pressing right key switches from LEFT_HARD to LEFT_SMOOTH to STRAIGHT to RIGHT_SMOOTH to RIGHT_HARD
        //once reached RIGHT_HARD, pressing right key again will not have any effect
        if (Input.GetKeyDown(KeyCode.D))
        {
            switch (currentTurnMode)
            {
                case TurnMode.LEFT_HARD:
                    currentTurnMode = TurnMode.LEFT_SMOOTH;
                    break;
                case TurnMode.LEFT_SMOOTH:
                    currentTurnMode = TurnMode.STRAIGHT;
                    break;
                case TurnMode.STRAIGHT:
                    currentTurnMode = TurnMode.RIGHT_SMOOTH;
                    break;
                case TurnMode.RIGHT_SMOOTH:
                    currentTurnMode = TurnMode.RIGHT_HARD;
                    break;
            }
        }
    }
    //called when henry touches a trigger collider
    //collider other is the object henry has touched
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();  
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count;

        if(count >= 14)
        {
            winText.text = "Collected all";
        }
    }

}
