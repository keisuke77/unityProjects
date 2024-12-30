using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BookCloseShader
{
    public class CloseTestButton : MonoBehaviour
    {
        [SerializeField] Button closeButton;
        [SerializeField] Button openButton;
        [SerializeField] BookCloseEffect effecter;

        [SerializeField] Camera mainCamera;

        // Start is called before the first frame update
        void Start()
        {
            closeButton.onClick.AddListener(Close);
            openButton.onClick.AddListener(Open);
        }

        void Close()
        {
            effecter.CloseBook(mainCamera, 2.0f);
        }

        void Open()
        {
            effecter.OpenBook(mainCamera, 2.0f);
        }
    }
}