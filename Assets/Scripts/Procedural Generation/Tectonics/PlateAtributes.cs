using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlateAtributes
{
    
    
    public void SetPlateMovement(ref List<Plate> plates)
    {
        for (int i = 0; i < plates.Count; i++)
        {
            plates[i].direction = RollDirection();
            plates[i].magnitude = RollRandom();
            plates[i].elevation = RollRandom();

        }
        

    }

    private Vector2 RollDirection()
    {
        Vector2 dir = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
        return dir.normalized;


    }
    private float RollRandom()
    {
        float mag = Random.Range(-1, 1);
        return mag;
    }
    
    private void PlateDynamics()
    {

    }
}
