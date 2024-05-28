using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DesktopLevel : MonoBehaviour
{
    public Vector2 screenSize;

    [SerializeField] private BoxCollider2D leftCollider;
    [SerializeField] private BoxCollider2D rightCollider;
    [SerializeField] private BoxCollider2D upperCollider;
    [SerializeField] private BoxCollider2D lowerCollider;
    [SerializeField] private BoxCollider2D taskBarCollider;
    [SerializeField] private Camera desktopCamera;

    [Serializable]
    public class Window
    {
        public string name;
        public bool isActive { get { return active; } set { active = value; windowObject.SetActive(value); } }
        public bool active = false;
        public GameObject windowObject;
        [HideInInspector] public BoxCollider2D collider;
    }
    public List<Window> windows;
    public List<Process> processes;

    public void Awake()
    {
        GetScreenSize();
        SetParamsOfScene(screenSize);
        StartCoroutine(GetProcesses());
    }

    public void GetScreenSize() => screenSize = new Vector2(Screen.width, Screen.height);
    public void SetParamsOfScene(Vector2 ScreenSize)
    {
        leftCollider.offset = new Vector2(-0.5f, screenSize.y / 2);
        leftCollider.size = new Vector2(1, screenSize.y + 2);

        rightCollider.offset = new Vector2(screenSize.x + 0.5f, screenSize.y / 2);
        rightCollider.size = new Vector2(1, screenSize.y + 2);

        upperCollider.offset = new Vector2(screenSize.x / 2, screenSize.y + 0.5f);
        upperCollider.size = new Vector2(screenSize.x + 2, 1);

        lowerCollider.offset = new Vector2(screenSize.x / 2, -0.5f);
        lowerCollider.size = new Vector2(screenSize.x + 2, 1);


        desktopCamera.transform.position = new Vector2(screenSize.x / 2, screenSize.y / 2);
        desktopCamera.orthographic = true;
        desktopCamera.orthographicSize = screenSize.y / 2;
    }

    public void SetWindowCollider(Window process, bool active, Vector2 size, Vector2 position,string name)
    {
        if (process.windowObject == null)
        {
            process.windowObject = Instantiate(new GameObject(name));
            process.collider = process.windowObject.AddComponent<BoxCollider2D>();
        }
        process.collider.offset = new Vector2(0, -size.y);
        process.isActive = active;
        process.collider.size = size;
        process.windowObject.transform.position = position;
    }

    IEnumerator GetProcesses()
    {
        processes = new List<Process>(Process.GetProcesses());
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i < processes.Count; i++)
        {
            //SetWindowCollider(windows,true,processes[i].)
        }
        StartCoroutine(GetProcesses());
    }
}
