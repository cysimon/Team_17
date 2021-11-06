using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global
{
    public static short characterIDCounter = 0;

    public static short equipmentIDCounter = 0;
}

public class Game : MonoBehaviour
{
    public static Game instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
