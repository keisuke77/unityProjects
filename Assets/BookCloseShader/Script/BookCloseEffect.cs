using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BookCloseShader
{
    public class BookCloseEffect : MonoBehaviour
    {
        [SerializeField, Header("RenderTextureのフォーマット")]
        RenderTextureFormat _format;

        [SerializeField] Camera _subCamera;
        [SerializeField] Animator _bookRoot;
        [SerializeField] Animator _book;
        [SerializeField] Material _material;


        private void Start()
        {
            SetAnimation(1);
        }

        public void SetCloseValue(float closed)
        {
            SetAnimation(closed);
        }

        void SetAnimation(float closed)
        {
            _book.SetFloat("MotionTime", 1 - closed);
            _bookRoot.SetFloat("MotionTime", 1 - closed);
        }

        public void CloseBook(Camera nowCamera, float time)
        {
            StartCoroutine(Close(nowCamera, time));
        }

        IEnumerator Close(Camera nowCamera, float time)
        {
            //レンダーテクスチャーを作成
            RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24, _format);

            _material.SetTexture("_MainTex", renderTexture);
            SetAnimation(0);

            _subCamera.enabled = true;
            Camera.SetupCurrent(_subCamera);

            float maxDepth = Mathf.Max(nowCamera.depth, _subCamera.depth);
            float minDepth = Mathf.Min(nowCamera.depth, _subCamera.depth);

            nowCamera.depth = minDepth;
            _subCamera.depth = maxDepth;

            //nowCameraの方にセット
            RenderTexture prev = nowCamera.targetTexture;
            nowCamera.targetTexture = renderTexture;

            //本閉じ
            float timer = 0;
            float diff = 0.01f / time;
            float closed = 0;

            while (timer < time)
            {
                closed += diff;
                SetAnimation(Mathf.Clamp(closed, 0, 1));
                timer += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }

            nowCamera.targetTexture = prev;
            //レンダーテクスチャを開放
            renderTexture.Release();
        }

        public void OpenBook(Camera nextCamera, float time)
        {
            StartCoroutine(Open(nextCamera, time));
        }

        IEnumerator Open(Camera nextCamera, float time)
        {
            //レンダーテクスチャーを作成
            RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24, _format);

            _material.SetTexture("_MainTex", renderTexture);
            SetAnimation(1);

            _subCamera.enabled = true;
            Camera.SetupCurrent(_subCamera);

            //nowCameraの方にセット
            nextCamera.targetTexture = renderTexture;

            //本開き
            float timer = 0;
            float diff = 0.01f / time;
            float closed = 1;

            while (timer < time)
            {
                closed -= diff;
                SetAnimation(Mathf.Clamp(closed, 0.01f, 1));
                timer += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }

            SetAnimation(0);
            nextCamera.targetTexture = null;
            Camera.SetupCurrent(nextCamera);
            float maxDepth = Mathf.Max(nextCamera.depth, _subCamera.depth);
            float minDepth = Mathf.Min(nextCamera.depth, _subCamera.depth);

            nextCamera.depth = maxDepth;
            _subCamera.depth = minDepth;

            _subCamera.enabled = false;

            //レンダーテクスチャを開放
            renderTexture.Release();
        }

    }
}