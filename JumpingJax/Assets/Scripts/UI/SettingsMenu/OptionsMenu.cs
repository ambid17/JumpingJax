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

    public PauseMenu pauseMenu; // This is the parent menu, needed to go back


    [Header("Set in editor")]
    public TabButton controlsTabButton;
    public TabButton videoTabButton;
    public TabButton audioTabButton;
    public TabButton miscTabButton;

    public Button backButton;
    public Button defaultButton;



    private SettingsTabs currentTab;


    private void OnEnable() {
        GetSubcomponents();
        SetCurrentTab(SettingsTabs.controls);
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
        videoPanel = GetComponentInChildren<VideoOptions>(true);
        audioPanel = GetComponentInChildren<AudioOptions>(true);
        miscPanel = GetComponentInChildren<MiscOptions>(true);

        if (controlsPanel == null || videoPanel == null || audioPanel == null || miscPanel == null)
        {
            Debug.LogError("An options panel was not found");
        }
    }

    // Remove all listeners and add them in case this script is ran twice
    public void InitializeTabButtons()
    {
        controlsTabButton.Init("CONTROLS", () => SetCurrentTab(SettingsTabs.controls));
        videoTabButton.Init("VIDEO", () => SetCurrentTab(SettingsTabs.video));
        audioTabButton.Init("AUDIO", () => SetCurrentTab(SettingsTabs.audio));
        miscTabButton.Init("MISC", () => SetCurrentTab(SettingsTabs.misc));
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

        currentTab = tabToSet;

        switch (tabToSet)
        {
            case SettingsTabs.controls:
                controlsPanel.gameObject.SetActive(true);
                controlsTabButton.SelectTab();
                break;
            case SettingsTabs.video:
                videoPanel.gameObject.SetActive(true);
                videoTabButton.SelectTab();
                break;
            case SettingsTabs.audio:
                audioPanel.gameObject.SetActive(true);
                audioTabButton.SelectTab();
                break;
            case SettingsTabs.misc:
                miscPanel.gameObject.SetActive(true);
                miscTabButton.SelectTab();
                break;
        }
    }

    public void ClearTabs()
    {
        controlsPanel.gameObject.SetActive(false);
        videoPanel.gameObject.SetActive(false);
        audioPanel.gameObject.SetActive(false);
        miscPanel.gameObject.SetActive(false);

        controlsTabButton.UnselectTab();
        videoTabButton.UnselectTab();
        audioTabButton.UnselectTab();
        miscTabButton.UnselectTab();
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
