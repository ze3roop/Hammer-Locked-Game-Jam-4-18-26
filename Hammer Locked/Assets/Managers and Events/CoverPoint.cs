using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public struct CoverPointData
{
    public Transform transform;
    public string ID; 
}

public class CoverPoint : MonoBehaviour
{
    public CoverPointData coverPointData = new(); 
    public void Awake()
    {
        coverPointData.transform = transform; 
        coverPointData.ID = this.name; 
    }
    public bool IsViable()
    {
        // if (!PlayerIsInLOS() && DistanceFromPlayer(enemyPosition) > 1f)
        // {
            
        // }
        // return false;
        Vector3 origin = transform.position;
        Vector3 dest   = PlayerStateMachine.Instance.transform.position;
        Debug.DrawRay(origin, (dest - origin).normalized * 20f, Color.green, 3f); 
        if (Physics.Raycast(origin, (dest - origin).normalized, out var hit)) //, losMask))
        {
            //Check if Player was in LOS. If is in LOS, then this cover isn't viable
            // Debug.Log(hit.transform.tag);
            if(hit.transform.tag == "Player")
                return false;

            //Check if we hit Cover, if we didn't then this Cover point isn't viable 
            if(hit.transform.tag != "Cover")
            {
                return false;
            }
            else
            {
                //Check that the cover point is close enough to an obstacle that is in the
                //same direction to the player

                //Or, in other words, check that there is close cover relative to the player
                if(Vector3.Distance(hit.transform.position, transform.position) < 5f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool PlayerIsInLOS()
    {
        return false;
    }

    private float DistanceFromPlayer(Vector3 enemyPosition)
    {
        return Vector3.Distance(enemyPosition, PlayerStateMachine.Instance.transform.position); 
    }

    public Vector3 Position()
    {
        return(transform.position); 
    }
}
