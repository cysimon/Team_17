using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Store : MonoBehaviour
{
    public int storeLevel = 1;

    public int materialPoint = 100;

    public Store instance;

    public Texture2DArray storeBackground;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addPoints(int Point)
    {
        materialPoint += Point;
    }

    public bool minusPoints(int Point)
    {
        int diff = materialPoint - Point;
        if (diff < 0)
        {
            // Msg: Not Enough
            
            return false;
        }
        else
        {
            materialPoint -= Point;
            return true;
        }
    }

    public void storeLevelUp()
    {
        storeLevel+=1;
    }
}
