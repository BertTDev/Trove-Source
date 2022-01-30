using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class optionsManager : MonoBehaviour
{
    public Color OnColor = new Vector4(0,140,0,255);
    public Color OffColor = new Vector4(211, 0, 0, 255);

    public Image musicButton;
    public Image soundButton;
    public Image sleepButton;
    public Image classicButton;
    public Image wallButton;

    public TextMeshProUGUI musicText;
    public TextMeshProUGUI soundText;
    public TextMeshProUGUI sleepText;
    public TextMeshProUGUI classicText;
    public TextMeshProUGUI wallText;

    public void setMusic()
    {
        PersistentManager._instance.musicOn = !PersistentManager._instance.musicOn;
        if (PersistentManager._instance.musicOn)
        {
            musicButton.color = OnColor;
            musicText.text = "MUSIC: ON";
            PersistentManager._instance.backAudioMenu.Play();
        }
        else
        {
            musicButton.color = OffColor;
            musicText.text = "MUSIC: OFF";
            PersistentManager._instance.backAudioMenu.Pause();
        }
    }
    public void setSound()
    {
        PersistentManager._instance.soundOn = !PersistentManager._instance.soundOn;
        if (PersistentManager._instance.soundOn)
        {
            soundButton.color = OnColor;
            soundText.text = "SOUND: ON";
        }
        else
        {
            soundButton.color = OffColor;
            soundText.text = "SOUND: OFF";
        }
    }
    public void setSleepingDragonMode()
    {
        PersistentManager._instance.sleepMode = !PersistentManager._instance.sleepMode;
        if (PersistentManager._instance.sleepMode)
        {
           sleepButton.color = OnColor;
           sleepText.text = "SLEEPING DRAGON MODE: ON";
        }
        else
        {
            sleepButton.color = OffColor;
            sleepText.text = "SLEEPING DRAGON MODE: OFF";
        }
    }

    public void setClassicMode()
    {
        PersistentManager._instance.isClassicMode = !PersistentManager._instance.isClassicMode;
        if (PersistentManager._instance.isClassicMode)
        {
            classicButton.color = OnColor;
            classicText.text = "CLASSIC MODE: ON";
        }
        else
        {
            classicButton.color = OffColor;
            classicText.text = "CLASSIC MODE: OFF";
        }
    }

    public void setWallJumping()
    {
        Debug.Log("Set Wall Jumping");
        PersistentManager._instance.wallJumping = !PersistentManager._instance.wallJumping;
        if (PersistentManager._instance.wallJumping)
        {
            wallButton.color = OnColor;
            wallText.text = "WALL JUMPING: ON";
        }
        else
        {
            wallButton.color = OffColor;
            wallText.text = "WALL JUMPING: OFF";
        }
    }

    private void Start()
    {
        if (PersistentManager._instance.musicOn)
        {
            musicButton.color = OnColor;
            musicText.text = "MUSIC: ON";
        }
        else
        {
            musicButton.color = OffColor;
            musicText.text = "MUSIC: OFF";
        }
        if (PersistentManager._instance.soundOn)
        {
            soundButton.color = OnColor;
            soundText.text = "SOUND: ON";
        }
        else
        {
            soundButton.color = OffColor;
            soundText.text = "SOUND: OFF";
        }
        if (PersistentManager._instance.sleepMode)
        {
            sleepButton.color = OnColor;
            sleepText.text = "SLEEPING DRAGON MODE: ON";
        }
        else
        {
            sleepButton.color = OffColor;
            sleepText.text = "SLEEPING DRAGON MODE: OFF";
        }
        if (PersistentManager._instance.isClassicMode)
        {
            classicButton.color = OnColor;
            classicText.text = "CLASSIC MODE: ON";
        }
        else
        {
            classicButton.color = OffColor;
            classicText.text = "CLASSIC MODE: OFF";
        }
        if (PersistentManager._instance.wallJumping)
        {
            wallButton.color = OnColor;
            wallText.text = "WALL JUMPING: ON";
        }
        else
        {
            wallButton.color = OffColor;
            wallText.text = "WALL JUMPING: OFF";
        }
    }
}
