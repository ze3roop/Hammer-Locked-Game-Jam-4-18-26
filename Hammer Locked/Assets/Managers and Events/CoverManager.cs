using System.Collections.Generic;
using UnityEngine;

public class CoverManager : MonoBehaviour
{
    public static CoverManager Instance {get; private set;}

    public List<CoverPoint> CoverPoints; 

    void Awake()
    {
        //Singleton
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this; 
    }

    public CoverPointData FindCover()
    {
        List<CoverPoint> ViableCoverPoints = new();
        for(int i = 0; i < CoverPoints.Count; i++)
        {
            if (CoverPoints[i].IsViable())
            {
                ViableCoverPoints.Add(CoverPoints[i]);
            }
        }

        int randomPoint = Random.Range(0, ViableCoverPoints.Count); 

        return ViableCoverPoints[randomPoint].coverPointData; 
    }

    public bool IsCoverPointViable(CoverPointData coverPointData)
    {
        CoverPoint point = CoverPoints.Find(e => e.coverPointData.ID == coverPointData.ID);
        return point.IsViable(); 
    }
}
