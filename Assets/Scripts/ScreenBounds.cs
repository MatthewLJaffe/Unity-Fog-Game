using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    public static readonly float MAX_X = 15.5f;
    public static readonly float MIN_X = -15.5f;
    public static readonly float MAX_Y = 25.5f;
    public static readonly float MIN_Y = -25.5f;

    public static bool inScreenBounds(Vector3 position) {
        return position.x > MIN_X && position.x < MAX_X && position.y > MIN_Y && position.y < MAX_Y;
    }
}