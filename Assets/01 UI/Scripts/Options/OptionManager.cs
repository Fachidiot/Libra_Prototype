using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Michsky.MUIP;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.InputSystem.Controls;
using VInspector.Libs;
using System.Collections.ObjectModel;

public class OptionManager : MonoBehaviour
{
    [Header("Screen Setting")]
    [SerializeField] private Toggle m_VSync;
    [SerializeField] private HorizontalSelector m_Fullscreen;
    [SerializeField] private Slider m_Brightness;
    [SerializeField] private CustomDropdown m_Resolution;

    [Header("Graphic Setting")]
    [SerializeField] private HorizontalSelector m_QualityDropdown;
    [SerializeField] private HorizontalSelector m_ShadowDropdown;
    [SerializeField] private HorizontalSelector m_AODropdown;
    [SerializeField] private HorizontalSelector m_ReflectionDropdown;

    [Header("Sound Setting")]
    [SerializeField] private Slider m_MasterSoundSlider;
    [SerializeField] private Slider m_BGMSoundSlider;
    [SerializeField] private Slider m_EffectSoundSlider;

    [Header("Gameplay Setting")]
    [SerializeField] private HorizontalSelector m_LanguageSelector;
    [SerializeField] private Toggle m_ScreenVibration;

    [Header("Shortcut Setting")]
    [SerializeField] private Button m_KeyMoveLeft;
    [SerializeField] private Button m_KeyMoveRight;
    [SerializeField] private Button m_KeyMoveBack;
    [SerializeField] private Button m_KeyMoveForward;
    [SerializeField] private Button m_KeyJump;
    [SerializeField] private Button m_KeyInteract;
    [SerializeField] private Button m_KeyInventory;
    [SerializeField] private Button m_KeyEscape;

    [SerializeField] private ModalWindowManager m_ShortcutModal;

    private KeyInput inputKey = KeyInput.NONE;

    public void InitMenuLayouts()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        // V-Sync
        m_VSync.isOn = OptionDataManager.Instance.OptionData.m_VSync;
        // FullScreenMode 설정
        m_Fullscreen.defaultIndex = (int)OptionDataManager.Instance.OptionData.m_FullScreenMode - 1;
        // Brightness
        m_Brightness.value = OptionDataManager.Instance.OptionData.m_ScreenBrightness;
        // Resolution
        HashSet<CustomDropdown.Item> resolutionOptions = new HashSet<CustomDropdown.Item>();
        for (int i = 0; i < OptionDataManager.Instance.Resolutions.Count; ++i)
        {   // DropDown 설정
            CustomDropdown.Item item = new CustomDropdown.Item();
            item.itemName = OptionDataManager.Instance.Resolutions[i].width + "x" + OptionDataManager.Instance.Resolutions[i].height;
            item.itemIndex = i;
            resolutionOptions.Add(item);
        }
        m_Resolution.items = new List<CustomDropdown.Item>(resolutionOptions);
        m_Resolution.selectedItemIndex = OptionDataManager.Instance.OptionData.m_ScreenResolution;
        m_Resolution.SetupDropdown();

        // Quality
        m_QualityDropdown.defaultIndex = OptionDataManager.Instance.OptionData.m_GraphicQuality;
        // Shadow
        m_ShadowDropdown.defaultIndex = OptionDataManager.Instance.OptionData.m_ShadowQuality;
        // AO
        m_AODropdown.defaultIndex = OptionDataManager.Instance.OptionData.m_AmbientOcclusion;
        // Reflection
        m_ReflectionDropdown.defaultIndex = OptionDataManager.Instance.OptionData.m_ReflectionQuality;

        // Sounds
        m_MasterSoundSlider.SetValueWithoutNotify(OptionDataManager.Instance.OptionData.m_MasterVolume);
        AudioManager.Instance.MasterVolume = OptionDataManager.Instance.OptionData.m_MasterVolume;
        m_BGMSoundSlider.SetValueWithoutNotify(OptionDataManager.Instance.OptionData.m_BGMVolume);
        AudioManager.Instance.BGMVolume = OptionDataManager.Instance.OptionData.m_BGMVolume;
        m_EffectSoundSlider.SetValueWithoutNotify(OptionDataManager.Instance.OptionData.m_EffectVolume);
        AudioManager.Instance.EffectVolume = OptionDataManager.Instance.OptionData.m_EffectVolume;

        // Language
        switch (OptionDataManager.Instance.OptionData.m_Language)
        {
            case SystemLanguage.Korean:
                m_LanguageSelector.defaultIndex = 0;
                break;
            case SystemLanguage.English:
                m_LanguageSelector.defaultIndex = 1;
                break;
            case SystemLanguage.Japanese:
                m_LanguageSelector.defaultIndex = 2;
                break;
        }
        // ScreenVibration
        m_ScreenVibration.isOn = OptionDataManager.Instance.OptionData.m_ScreenVibration;
        
        m_KeyMoveLeft.GetComponentInChildren<TMP_Text>().text = OptionDataManager.Instance.OptionData.m_keyData.m_KeyMoveLeft.ToString();
        m_KeyMoveRight.GetComponentInChildren<TMP_Text>().text = OptionDataManager.Instance.OptionData.m_keyData.m_KeyMoveRight.ToString();
        m_KeyMoveBack.GetComponentInChildren<TMP_Text>().text = OptionDataManager.Instance.OptionData.m_keyData.m_KeyMoveUp.ToString();
        m_KeyMoveForward.GetComponentInChildren<TMP_Text>().text = OptionDataManager.Instance.OptionData.m_keyData.m_KeyMoveDown.ToString();
        m_KeyJump.GetComponentInChildren<TMP_Text>().text = OptionDataManager.Instance.OptionData.m_keyData.m_KeyJump.ToString();
        // m_KeyMoveLeft.GetComponentInChildren<TMP_Text>().text = OptionDataManager.Instance.OptionData.m_keyData.m_KeySprint.ToString();
        // m_KeyMoveLeft.GetComponentInChildren<TMP_Text>().text = OptionDataManager.Instance.OptionData.m_keyData.m_KeyCrouch.ToString();
        m_KeyInteract.GetComponentInChildren<TMP_Text>().text = OptionDataManager.Instance.OptionData.m_keyData.m_KeyInteract.ToString();
        m_KeyInventory.GetComponentInChildren<TMP_Text>().text = OptionDataManager.Instance.OptionData.m_keyData.m_KeyInventory.ToString();
    }

    public void SetVSync(bool vsync)
    {
        QualitySettings.vSyncCount = vsync ? 1 : 0;
        OptionDataManager.Instance.OptionData.m_VSync = vsync;
        OptionDataManager.Instance.SaveOptionData();
    }

    public void SetFullScreenMode(int fullScreenMode)
    {
        OptionDataManager.Instance.OptionData.m_FullScreenMode = (FullScreenMode)(fullScreenMode + 1);
        OptionDataManager.Instance.SaveOptionData();
        SetResolution(OptionDataManager.Instance.OptionData.m_ScreenResolution);
    }

    public void SetBrightness(float brightness)
    {
        // TODO : 밝기 조절 스크립트 필요
        OptionDataManager.Instance.OptionData.m_ScreenBrightness = brightness;
        OptionDataManager.Instance.SaveOptionData();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = OptionDataManager.Instance.Resolutions[resolutionIndex];

        Screen.SetResolution(resolution.width, resolution.height, OptionDataManager.Instance.OptionData.m_FullScreenMode, Screen.currentResolution.refreshRateRatio);
        OptionDataManager.Instance.OptionData.m_ScreenResolution = resolutionIndex;
        OptionDataManager.Instance.SaveOptionData();
    }

    public void QualitySelector(int quality)
    {
        QualitySettings.SetQualityLevel(quality, true);
        OptionDataManager.Instance.OptionData.m_GraphicQuality = quality;
        OptionDataManager.Instance.SaveOptionData();
    }

    public void ShadowQualitySelector(int quality)
    {
        QualitySettings.SetQualityLevel(quality, true);
        OptionDataManager.Instance.OptionData.m_ShadowQuality = quality;
        OptionDataManager.Instance.SaveOptionData();
    }

    public void AOSelector(int quality)
    {
        QualitySettings.SetQualityLevel(quality, true);
        OptionDataManager.Instance.OptionData.m_AmbientOcclusion = quality;
        OptionDataManager.Instance.SaveOptionData();
    }

    public void ReflectionQualitySelector(int quality)
    {
        QualitySettings.SetQualityLevel(quality, true);
        OptionDataManager.Instance.OptionData.m_ReflectionQuality = quality;
        OptionDataManager.Instance.SaveOptionData();
    }

    public void MasterValueChanged()
    {
        AudioManager.Instance.MasterVolume = m_MasterSoundSlider.value;
        OptionDataManager.Instance.OptionData.m_MasterVolume = m_MasterSoundSlider.value;
        OptionDataManager.Instance.SaveOptionData();
    }

    public void BGMValueChanged()
    {
        AudioManager.Instance.BGMVolume = m_BGMSoundSlider.value;
        OptionDataManager.Instance.OptionData.m_BGMVolume = m_BGMSoundSlider.value;
        OptionDataManager.Instance.SaveOptionData();
    }

    public void EffectValueChanged()
    {
        AudioManager.Instance.EffectVolume = m_EffectSoundSlider.value;
        OptionDataManager.Instance.OptionData.m_EffectVolume = m_EffectSoundSlider.value;
        OptionDataManager.Instance.SaveOptionData();
    }

    public void SelectLangDropdown(int language)
    {
        switch (language)
        {
            //한국어
            case 0:
                OptionDataManager.Instance.OptionData.m_Language = SystemLanguage.Korean;
                break;
            case 1:
                OptionDataManager.Instance.OptionData.m_Language = SystemLanguage.English;
                break;
            case 2:
                OptionDataManager.Instance.OptionData.m_Language = SystemLanguage.Japanese;
                break;
        }

        OptionDataManager.Instance.SaveOptionData();
    }

    public void SetScreenVibration(bool vibration)
    {
        // TODO : 화면 흔들림 설정 스크립트 필요
        OptionDataManager.Instance.OptionData.m_ScreenVibration = vibration;
        OptionDataManager.Instance.SaveOptionData();
    }

    public void ShortCutInput(int keyCode)
    {
        inputKey = (KeyInput)keyCode;
    }

    void OnGUI()
    {
        if (KeyInput.NONE != inputKey)
        {
            Event keyEvent = Event.current;
            if (keyEvent.isKey)
            {
                if (KeyCode.Escape == keyEvent.keyCode)
                {
                    switch (inputKey)
                    {
                        case KeyInput.LEFT:
                            m_KeyMoveLeft.GetComponentInChildren<TMP_Text>().text = OptionDataManager.Instance.OptionData.m_keyData.m_KeyMoveLeft.ToString();
                            break;
                        case KeyInput.RIGHT:
                            m_KeyMoveRight.GetComponentInChildren<TMP_Text>().text = OptionDataManager.Instance.OptionData.m_keyData.m_KeyMoveRight.ToString();
                            break;
                        case KeyInput.UP:
                            m_KeyMoveBack.GetComponentInChildren<TMP_Text>().text = OptionDataManager.Instance.OptionData.m_keyData.m_KeyMoveUp.ToString();
                            break;
                        case KeyInput.DOWN:
                            m_KeyMoveForward.GetComponentInChildren<TMP_Text>().text = OptionDataManager.Instance.OptionData.m_keyData.m_KeyMoveDown.ToString();
                            break;
                        case KeyInput.JUMP:
                            m_KeyJump.GetComponentInChildren<TMP_Text>().text = OptionDataManager.Instance.OptionData.m_keyData.m_KeyJump.ToString();
                            break;
                        case KeyInput.SPRINT:
                            m_KeyMoveLeft.GetComponentInChildren<TMP_Text>().text = OptionDataManager.Instance.OptionData.m_keyData.m_KeySprint.ToString();
                            break;
                        case KeyInput.CROUCH:
                            m_KeyMoveLeft.GetComponentInChildren<TMP_Text>().text = OptionDataManager.Instance.OptionData.m_keyData.m_KeyCrouch.ToString();
                            break;
                        case KeyInput.INTERACT:
                            m_KeyInteract.GetComponentInChildren<TMP_Text>().text = OptionDataManager.Instance.OptionData.m_keyData.m_KeyInteract.ToString();
                            break;
                        case KeyInput.INVENTORY:
                            m_KeyInventory.GetComponentInChildren<TMP_Text>().text = OptionDataManager.Instance.OptionData.m_keyData.m_KeyInventory.ToString();
                            break;
                    }
                    return;
                }
                switch (inputKey)
                {
                    case KeyInput.LEFT:
                        OptionDataManager.Instance.OptionData.m_keyData.m_KeyMoveLeft = keyEvent.keyCode;
                        m_KeyMoveLeft.GetComponentInChildren<TMP_Text>().text = keyEvent.keyCode.ToString();
                        break;
                    case KeyInput.RIGHT:
                        OptionDataManager.Instance.OptionData.m_keyData.m_KeyMoveRight = keyEvent.keyCode;
                        m_KeyMoveRight.GetComponentInChildren<TMP_Text>().text = keyEvent.keyCode.ToString();
                        break;
                    case KeyInput.UP:
                        OptionDataManager.Instance.OptionData.m_keyData.m_KeyMoveUp = keyEvent.keyCode;
                        m_KeyMoveBack.GetComponentInChildren<TMP_Text>().text = keyEvent.keyCode.ToString();
                        break;
                    case KeyInput.DOWN:
                        OptionDataManager.Instance.OptionData.m_keyData.m_KeyMoveDown = keyEvent.keyCode;
                        m_KeyMoveForward.GetComponentInChildren<TMP_Text>().text = keyEvent.keyCode.ToString();
                        break;
                    case KeyInput.JUMP:
                        OptionDataManager.Instance.OptionData.m_keyData.m_KeyJump = keyEvent.keyCode;
                        m_KeyJump.GetComponentInChildren<TMP_Text>().text = keyEvent.keyCode.ToString();
                        break;
                    case KeyInput.SPRINT:
                        OptionDataManager.Instance.OptionData.m_keyData.m_KeySprint = keyEvent.keyCode;
                        m_KeyMoveLeft.GetComponentInChildren<TMP_Text>().text = keyEvent.keyCode.ToString();
                        break;
                    case KeyInput.CROUCH:
                        OptionDataManager.Instance.OptionData.m_keyData.m_KeyCrouch = keyEvent.keyCode;
                        m_KeyMoveLeft.GetComponentInChildren<TMP_Text>().text = keyEvent.keyCode.ToString();
                        break;
                    case KeyInput.INTERACT:
                        OptionDataManager.Instance.OptionData.m_keyData.m_KeyInteract = keyEvent.keyCode;
                        m_KeyInteract.GetComponentInChildren<TMP_Text>().text = keyEvent.keyCode.ToString();
                        break;
                    case KeyInput.INVENTORY:
                        OptionDataManager.Instance.OptionData.m_keyData.m_KeyInventory = keyEvent.keyCode;
                        m_KeyInventory.GetComponentInChildren<TMP_Text>().text = keyEvent.keyCode.ToString();
                        break;
                }
                inputKey = KeyInput.NONE;
                m_ShortcutModal.Close();
            }
        }
    }
}

[Serializable]
public enum KeyInput
{
    NONE,
    LEFT,
    RIGHT,
    UP,
    DOWN,
    JUMP,
    SPRINT,
    CROUCH,
    INTERACT,
    INVENTORY
}