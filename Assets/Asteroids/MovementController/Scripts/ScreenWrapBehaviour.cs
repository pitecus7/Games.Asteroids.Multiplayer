using UnityEngine;
using System.Collections;
using Mirror;

public class ScreenWrapBehaviour : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    Renderer[] renderers;

    bool isWrappingX = false;
    bool isWrappingY = false;

    float screenWidth;
    float screenHeight;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        var cam = Camera.main;

        var screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        var screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        // The width is then equal to difference between the rightmost and leftmost x-coordinates
        screenWidth = screenTopRight.x - screenBottomLeft.x;
        // The height, similar to above is the difference between the topmost and the bottom yycoordinates
        screenHeight = screenTopRight.y - screenBottomLeft.y;

        if (isLocalPlayer)
        {
            spriteRenderer.color = Color.cyan;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ScreenWrap();
    }

    void ScreenWrap()
    {
        // If all parts of the object are invisible we wrap it around
        foreach (var renderer in renderers)
        {
            if (renderer.isVisible)
            {
                isWrappingX = false;
                isWrappingY = false;
                return;
            }
        }

        // If we're already wrapping on both axes there is nothing to do
        if (isWrappingX && isWrappingY)
        {
            return;
        }

        var cam = Camera.main;
        var newPosition = transform.position;

        var viewportPosition = cam.WorldToViewportPoint(transform.position);


        // Wrap it is off screen along the x-axis and is not being wrapped already
        if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;

            isWrappingX = true;
        }

        // Wrap it is off screen along the y-axis and is not being wrapped already
        if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;

            isWrappingY = true;
        }

        //Apply new position
        transform.position = newPosition;
    }
}