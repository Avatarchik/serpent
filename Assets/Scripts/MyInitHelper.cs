using UnityEngine;
using System.Collections;

public interface IInitializable {
    void Init();
}

/**
 Allows to init objects in particular order.
 */
public class MyInitHelper : MonoBehaviour {

    public MonoBehaviour[] objects;

	void Start () {
        foreach (MonoBehaviour comp in objects) {
            var obj = comp as IInitializable;
            Debug.Assert(obj != null);
            if (comp.enabled)
                obj.Init();
        }
	}
}
