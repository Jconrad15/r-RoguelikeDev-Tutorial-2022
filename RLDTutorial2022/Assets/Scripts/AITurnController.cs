using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnController : MonoBehaviour
{
    private EntityManager entityManager;

    void Start()
    {
        TurnController.Instance.RegisterOnStartAITurn(OnStartTurn);
        entityManager = FindObjectOfType<EntityManager>();
    }

    private void OnStartTurn()
    {
        StartCoroutine(AIProcessing());
    }

    private IEnumerator AIProcessing()
    {
        List<Entity> entities = entityManager.GetNonPlayerEntities();
        Debug.Log("Start AI processing for " +
            entities.Count + " entities");
        // Loop through each entities turn
        foreach (Entity entity in entities)
        {
            // Leave loop when entity acts
            bool entityActed = false;
            int counter = 0;
            while (entityActed == false)
            {
                counter++;

                entityActed = CheckEntityAction(entity);

                // Abort if tried action more than 2 times
                if (counter > 2 && entityActed == false)
                {
                    Debug.LogWarning(
                        "Exited entity try action loop.");
                    break;
                }
            }
        }

        // Turn is done
        TurnController.Instance.NextTurn();
        yield return null;
    }

    private bool CheckEntityAction(Entity entity)
    {
        if (entity == null) { return false; }
        if (entity.Components == null) { return false; }

        // Search components list for AI component
        // if (entity.Components.OfType<AI>().Any()) { }
        foreach (BaseComponent c in entity.Components)
        {
            if (c is AI)
            {
                AI ai = c as AI;
                return ai.TryAction(entity);
            }
        }

        return false; 
    }

}
