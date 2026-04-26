using UnityEngine;

public class EnemySenses : MonoBehaviour
{
    public Transform Target { get; private set; }
    public bool HasLOS { get; private set; }
    public float LastSeenTime { get; private set; }

    [SerializeField] LayerMask losMask = ~0; // set to what blocks vision

    private float raycastDelayMax = .5f; 
    private float raycastDelay; 

    public void OnUpdate(Transform player, float sightRange)
    {
        Target = player;
        if (player == null) { Debug.Log("player null"); HasLOS = false; return; }

        Vector3 to = player.position - transform.position;
        if (to.sqrMagnitude > sightRange * sightRange)
        {
            // Debug.Log("Player out of LOS Range");
            HasLOS = false;
            return;
        }

        if (Time.time >= raycastDelay)
        {
            raycastDelay = Time.time + raycastDelayMax;
            // LOS ray
            Vector3 origin = transform.position;
            Vector3 dest   = player.position;
            Debug.DrawRay(origin, (dest - origin).normalized * sightRange, Color.green, 1.6f); 
            if (Physics.Raycast(origin, (dest - origin).normalized, out var hit)) //, losMask))
            {
                //Debug.Log("Player in LOS");
                HasLOS = hit.transform == player;
                if (HasLOS) LastSeenTime = Time.time;
            }
            else
            {
                // Debug.Log("Player not in LOS");
                HasLOS = false;
            }
        }
    }

    public bool RecentlySawTarget(float loseSightAfterSeconds)
        => Time.time - LastSeenTime <= loseSightAfterSeconds;
}
