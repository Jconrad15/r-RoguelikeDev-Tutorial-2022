using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpMovement : MonoBehaviour
{
    private readonly float defaultSpeed = 60f;
    private readonly float speedModifier = 1.2f;

    private float speed = 60f;
    private int coroutineCounter = 0;

    public void UpdatePosition(Entity entity)
    {
        StopAllCoroutines();
        UpdateSpeed();
        StartCoroutine(LerpPosition(entity));
    }

    private void UpdateSpeed()
    {
        if (coroutineCounter > 0)
        {
            speed *= speedModifier;
            coroutineCounter = 0;
        }
        else
        {
            speed = defaultSpeed;
        }
    }

    private IEnumerator LerpPosition(Entity entity)
    {
        coroutineCounter++;

        HexCoordinates hexCoords = entity.CurrentTile.Coordinates;
        Vector3 endPos = hexCoords.GetWorldPosition();
        Vector3 currentPos = gameObject.transform.position;

        float distance = Vector3.Distance(endPos, currentPos);
        while (distance > 0.1f)
        {
            currentPos = Vector3.MoveTowards(
                currentPos, endPos, speed * Time.deltaTime);

            gameObject.transform.position = currentPos;

            distance = Vector3.Distance(endPos, currentPos);
            yield return new WaitForEndOfFrame();
        }

        // Set end position
        gameObject.transform.position = endPos;
        coroutineCounter--;
    }


}
