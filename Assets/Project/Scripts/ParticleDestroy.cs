using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    private ParticleSystem _ps;
    // Start is called before the first frame update
    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_ps != null)
        {
            if (!_ps.IsAlive()) Destroy(gameObject);
        }
    }
}
