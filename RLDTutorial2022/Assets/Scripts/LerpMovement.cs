using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpMovement : MonoBehaviour
{
    private float speed = 100f;
    
    public IEnumerator UpdatePosition(Entity entity)
    {
        StopAllCoroutines();

        HexCoordinates hexCoords = entity.CurrentTile.Coordinates;
        Vector3 endPos = hexCoords.GetWorldPosition();
        Vector3 currentPos = gameObject.transform.position;

        float distance = Vector3.Distance(endPos, currentPos);
        while(distance > 0.1f)
        {
            currentPos = Vector3.MoveTowards(
                currentPos, endPos, speed * Time.deltaTime);

            gameObject.transform.position = currentPos;
            yield return new WaitForEndOfFrame();
        }

        // Set end position
        gameObject.transform.position = endPos;
    }


}
