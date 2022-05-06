using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomMovement : MonoBehaviour
{
    RectTransform canvas;
    RectTransform currentObj;
    public float speed;
    float horizontalSpeedMultiplier = 1f; // +1 moves right, -1 moves left
    float verticalSpeedMultiplier = -1f; // +1 moves upward, -1 moves downward

    void Start()
    {
        currentObj = gameObject.GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
    }

    void Update()
    {
        transform.Translate(horizontalSpeedMultiplier * speed, verticalSpeedMultiplier * speed, 0f);

        if (currentObj.position.y > canvas.rect.height)
        {
            verticalSpeedMultiplier = -1f; // now start moving downward
        }

        if (currentObj.position.y < 0f)
        {
            verticalSpeedMultiplier = 1f; // now start moving upward
        }

        if (currentObj.position.x < 0f)
        {
            horizontalSpeedMultiplier = 1f; // now start moving towards right
        }

        if (currentObj.position.x > canvas.rect.width)
        {
            horizontalSpeedMultiplier = -1f; // now start moving towards left
        }
    }
}
