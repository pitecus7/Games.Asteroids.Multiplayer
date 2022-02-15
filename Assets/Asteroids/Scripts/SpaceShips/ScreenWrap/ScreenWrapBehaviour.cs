using UnityEngine;
using Mirror;

public class ScreenWrapBehaviour : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private SpaceshipActor spaceshipActor;

    bool isWrappingX = false;
    bool isWrappingY = false;

    void Awake()
    {
        if (spaceshipActor == null)
            spaceshipActor = GetComponentInParent<SpaceshipActor>();
    }

    void Update()
    {
        ScreenWrap();
    }

    void ScreenWrap()
    {
        if (spriteRenderer.isVisible)
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }
        else
        {
            HideSpaceship();
        }


        // If we're already wrapping on both axes there is nothing to do
        if (isWrappingX && isWrappingY)
        {
            return;
        }

        var cam = Camera.main;
        var newPosition = spaceshipActor.transform.position;

        var viewportPosition = cam.WorldToViewportPoint(spaceshipActor.transform.position);


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
        Teleport(newPosition);
    }

    private void HideSpaceship()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
    }

    private void Teleport(Vector3 newPosition)
    {
        spaceshipActor.transform.position = newPosition;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}