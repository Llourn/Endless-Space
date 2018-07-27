using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour {

    public virtual void DisableComponents()
    {

    }

    public virtual void OnObjectReuse()
    {

    }

    public virtual void OnObjectReuse(FlightPattern flightPattern, int index)
    {

    }

    public virtual void OnDestroy()
    {
        gameObject.SetActive(false);
    }
}
