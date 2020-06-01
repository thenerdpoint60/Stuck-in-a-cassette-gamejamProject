using System;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public event EventHandler<btnPressedEventArgs> btnPressed;
    public class btnPressedEventArgs : EventArgs
    {
        public buttonType color;
    }
    public enum buttonType { Blue, Orange};
    public bool isPressed;
    public float releaseAfter=0f;
    public buttonType btnType;
    private Animator anim;
    private float waiting = 0;


    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (isPressed&&releaseAfter!=0)
        {
            waiting += Time.deltaTime;
            if (waiting >= releaseAfter)
            {
                isPressed = false;
                waiting = 0;
            }
        }

        anim.SetBool("isPressed", isPressed);
    }

    public void pressTheButton()
    {
        isPressed = true;

        btnPressed?.Invoke(this, new btnPressedEventArgs { color = btnType });
    }
}
