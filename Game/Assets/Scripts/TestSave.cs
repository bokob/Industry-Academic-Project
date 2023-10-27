using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSave : MonoBehaviour
{
    public void Save()
    {
        DataManager.Instance.Save();
    }
}
