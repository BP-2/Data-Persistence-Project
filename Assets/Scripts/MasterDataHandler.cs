using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDataHandler : MonoBehaviour
{
    public static MasterDataHandler Instance;

    public int bestScore = 0;

    public string playerName;

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy (gameObject);
        }
        Instance = this;
        DontDestroyOnLoad (gameObject);
    }
}
