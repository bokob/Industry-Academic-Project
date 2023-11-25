using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncyclopediaControl : MonoBehaviour
{
    public GameObject prevBtn, nextBtn, backBtn,backToBtn;
    private Button pBtn, nBtn;
    public int currentPage = 0;
    public GameObject[] pages;
    void Start()
    {
        pBtn = prevBtn.GetComponent<Button>();
        nBtn = nextBtn.GetComponent<Button>();

        pBtn.onClick.AddListener(Prev);
        nBtn.onClick.AddListener(Next);
        
        // 초기 버튼 상태 설정
        prevBtn.SetActive(false);
    }

    void Prev()
    {
        SoundManager.Instance.PlaySFX("click");
        Debug.Log("Prev 버튼 클릭");

        if (currentPage > 0)
        {
            // 현재 페이지 비활성화
            pages[currentPage].SetActive(false);
            currentPage--;

            if (currentPage == 0)
                prevBtn.SetActive(false);

            nextBtn.SetActive(true); // 이전 버튼을 클릭할 때 다음 버튼을 활성화

            // 새로운 페이지 활성화
            ShowPage(currentPage);
        }
    }

    void Next()
    {
        SoundManager.Instance.PlaySFX("click");
        Debug.Log("Next 버튼 클릭");

        if (currentPage < pages.Length - 1)
        {
            // 현재 페이지 비활성화
            pages[currentPage].SetActive(false);
            currentPage++;

            if (currentPage == pages.Length - 1)
                nextBtn.SetActive(false);

            prevBtn.SetActive(true); // 다음 버튼을 클릭할 때 이전 버튼을 활성화

            // 새로운 페이지 활성화
            ShowPage(currentPage);
        }
    }

    void ShowPage(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < pages.Length)
        {
            pages[pageIndex].SetActive(true);
        }
    }

    public void ShowEncyclopediaAll(int pageIndex)
    {
        SoundManager.Instance.PlaySFX("click");

        pages[pageIndex].SetActive(true);

        if(pageIndex!=0)
            prevBtn.SetActive(true);
        if(pageIndex!=pages.Length-1)
            nextBtn.SetActive(true);
        backBtn.SetActive(true);
    }

    void HideEncyclopediaAll(int pageIndex)
    {
        SoundManager.Instance.PlaySFX("click");

        pages[pageIndex].SetActive(false);
        prevBtn.SetActive(false);
        nextBtn.SetActive(false);
        backBtn.SetActive(false);
    }

    public void ShowAnimalInfo(GameObject animalInfo)
    {
        SoundManager.Instance.PlaySFX("click");

        if(!animalInfo.activeSelf)
        {
            animalInfo.SetActive(true);
            backToBtn.SetActive(true);
            
            HideEncyclopediaAll(currentPage);
        }
    }
}