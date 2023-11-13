using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToEncyclopedia : MonoBehaviour
{
    public Transform parentObject;
    public string searchString="Info";
    public EncyclopediaControl encyclopediaControl;
    void Start()
    {
        
    }

    public void backTo()
    {
        if (parentObject != null)
        {
            int childCount = parentObject.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Transform child = parentObject.GetChild(i);
                
                // 자식 오브젝트의 이름이 특정 문자열을 포함하면 비활성화합니다.
                if (child.name.Contains(searchString))
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("부모 오브젝트가 할당되지 않았습니다.");
        }
        encyclopediaControl.ShowEncyclopediaAll(encyclopediaControl.currentPage);
        gameObject.SetActive(false);
    }
}