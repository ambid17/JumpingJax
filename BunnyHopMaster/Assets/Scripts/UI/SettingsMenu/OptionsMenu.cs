using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {
    public enum SettingsTabs
    {
        controls, video, audio, misc
    }

    [Header("Set at runtime")]
    public HotKeyOptions controlsPanel;
    public VideoOptions videoPanel;
    public AudioOptions audioPanel;
    public MiscOptions miscPanel;

    [Header("Set in editor")]
    public Button controlsTabButton;
    public Button videoTabButton;
    public Button audioTabButton;
    public Button miscTabButton;

    public Button backButton;
    public Button defaultButton;

    public PauseMenu pauseMenu; // This is the parent menu, needed to go back


    private SettingsTabs currentTab;

    private void Start() {
        GetSubcomponents();
        SetCurrentTab(SettingsTabs.video);
        InitializeTabButtons();
        InitializeBackButton();
        InitializeDefaultButton();

        pauseMenu = GetComponentInParent<PauseMenu>();
        if(pauseMenu == null)
        {
            Debug.LogError("Options menu does not have a parented Pause Menu");
        }
    }

    // Get subcomponents to prevent messing up in the editor
    private void GetSubcomponents()
    {
        controlsPanel = GetComponentInChildren<HotKeyOptions>(true);

        if(controlsPanel == null)
        {
            Debug.LogError("panel not found");
        }
        videoPanel = GetComponentInChildren<VideoOptions>(true);
        audioPanel = GetComponentInChildren<AudioOptions>(true);
        miscPanel = GetComponentInChildren<MiscOptions>(true);
    }

    // Remove all listeners and add them in case this script is ran twice
    public void InitializeTabButtons()
    {
        controlsTabButton.onClick.RemoveAllListeners();
        controlsTabButton.onClick.AddListener(() => SetCurrentTab(SettingsTabs.controls));

        videoTabButton.onClick.RemoveAllListeners();
        videoTabButton.onClick.AddListener(() => SetCurrentTab(SettingsTabs.video));

        audioTabButton.onClick.RemoveAllListeners();
        audioTabButton.onClick.AddListener(() => SetCurrentTab(SettingsTabs.audio));

        miscTabButton.onClick.RemoveAllListeners();
        miscTabButton.onClick.AddListener(() => SetCurrentTab(SettingsTabs.misc));
    }

    public void InitializeBackButton()
    {
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() => Back());
    }

    public void InitializeDefaultButton()
    {
        defaultButton.onClick.RemoveAllListeners();
        defaultButton.onClick.AddListener(() => DefaultCurrentOptions());
    }

    public void SetCurrentTab(SettingsTabs tabToSet)
    {
        ClearTabs();

        ColorBlock selectedColors = new ColorBlock();
        selectedColors.normalColor = new Color(1, 0, 0);
        selectedColors.highlightedColor = new Color(0.9f, 0, 0);
        selectedColors.selectedColor = new Color(0.9f, 0, 0);
        selectedColors.colorMultiplier = 2;

        switch (tabToSet)
        {
            case SettingsTabs.controls:
                controlsPanel.gameObject.SetActive(true);
                controlsTabButton.colors = selectedColors;
                break;
            case SettingsTabs.video:
                videoPanel.gameObject.SetActive(true);
                videoTabButton.colors = selectedColors;
                break;
            case SettingsTabs.audio:
                audioPanel.gameObject.SetActive(true);
                audioTabButton.colors = selectedColors;
                break;
            case SettingsTabs.misc:
                miscPanel.gameObject.SetActive(true);
                miscTabButton.colors = selectedColors;
                break;
        }
    }

    public void ClearTabs()
    {
        controlsPanel.gameObject.SetActive(false);
        videoPanel.gameObject.SetActive(false);
        audioPanel.gameObject.SetActive(false);
        miscPanel.gameObject.SetActive(false);

        ColorBlock normalColors = new ColorBlock();
        normalColors.normalColor = new Color(1, 1, 1);
        normalColors.highlightedColor = new Color(0.9f, 0, 0);
        normalColors.selectedColor = new Color(0.9f, 0, 0);
        normalColors.colorMultiplier = 2;

        controlsTabButton.colors = normalColors;
        videoTabButton.colors = normalColors;
        audioTabButton.colors = normalColors;
        miscTabButton.colors = normalColors;
    }

    // Tell our parent to toggle back to the main pause menu panel
    public void Back()
    {
        pauseMenu.ToggleOptionsMenu();
    }

    public void DefaultCurrentOptions()
    {
        switch (currentTab)
        {
            case SettingsTabs.controls:
                controlsPanel.SetDefaults();
                break;
            case SettingsTabs.video:
                videoPanel.SetDefaults();
                break;
            case SettingsTabs.audio:
                audioPanel.SetDefaults();
                break;
            case SettingsTabs.misc:
                miscPanel.SetDefaults();
                break;
        }
    }
}
