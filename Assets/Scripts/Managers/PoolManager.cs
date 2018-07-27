using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

    public void CreatePool(GameObject prefab, int poolSize)
    {
        int poolKey = prefab.GetInstanceID();

        GameObject poolHolder = new GameObject(prefab.name + " Pool");
        poolHolder.tag = "Pool Holder";
        poolHolder.transform.parent = transform;

        if(!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

            for(int i = 0; i < poolSize; i++)
            {
                ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject);
                poolDictionary[poolKey].Enqueue(newObject);
                newObject.SetParent(poolHolder.transform);
            }
        }
    }

    public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();

        if(poolDictionary.ContainsKey(poolKey))
        {
            ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue(objectToReuse);

            objectToReuse.Reuse(position, rotation);
        }
    }

    public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation, FlightPattern flightPattern, int index)
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey))
        {
            ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue(objectToReuse);

            objectToReuse.Reuse(position, rotation, flightPattern, index);
        }
    }

    public void DisableAllPoolObjects()
    {
        Debug.Log("DISABLE ALL POOLJECTS");
        GameObject[] p = GameObject.FindGameObjectsWithTag("Pool Holder");

        foreach (GameObject o in p)
        {
            foreach (Transform child in o.transform)
            {
                if (child.GetComponent<PoolObject>() != null)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

}

public class ObjectInstance
{
    GameObject gameObject;
    Transform transform;

    bool hasPoolObjectComponent = false;
    PoolObject poolObject;

    public ObjectInstance(GameObject objectInstance)
    {
        gameObject = objectInstance;
        transform = gameObject.transform;
        gameObject.SetActive(false);

        poolObject = gameObject.GetComponent<PoolObject>();
        if (poolObject != null)
        {
            hasPoolObjectComponent = true;
        }
    }

    public void Reuse(Vector3 position, Quaternion rotation)
    {
        gameObject.SetActive(true);

        if (hasPoolObjectComponent) poolObject.DisableComponents();

        transform.position = position;
        transform.rotation = rotation;

        if (hasPoolObjectComponent) poolObject.OnObjectReuse();
    }

    public void Reuse(Vector3 position, Quaternion rotation, FlightPattern flightPattern, int index)
    {
        gameObject.SetActive(true);

        if (hasPoolObjectComponent) poolObject.DisableComponents();

        transform.position = position;
        transform.rotation = rotation;

        if (hasPoolObjectComponent) poolObject.OnObjectReuse(flightPattern, index);
    }

    public void SetParent(Transform parent)
    {
        transform.parent = parent;
    }
    
}
