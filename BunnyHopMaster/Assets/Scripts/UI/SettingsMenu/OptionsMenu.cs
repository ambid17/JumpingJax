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

    public HotKeyOptions controlsPanel;
    public VideoOptions videoPanel;
    public AudioOptions audioPanel;
    public MiscOptions miscPanel;

    public Button controlsTabButton;
    public Button videoTabButton;
    public Button audioTabButton;
    public Button miscTabButton;

    public SettingsTabs currentTab;

    public Button backButton;
    public Button defaultButton;

    public PauseMenu pauseMenu; // This is the parent menu, needed to go back

    private void Start() {
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
        backButton.onClick.AddListener(() => SetCurrentTab(SettingsTabs.controls));
    }

    public void InitializeDefaultButton()
    {
        defaultButton.onClick.RemoveAllListeners();
        defaultButton.onClick.AddListener(() => SetCurrentTab(SettingsTabs.controls));
    }

    public void SetCurrentTab(SettingsTabs tabToSet)
    {
        ClearTabs();
        switch (tabToSet)
        {
            case SettingsTabs.controls:
                controlsPanel.gameObject.SetActive(true);
                break;
            case SettingsTabs.video:
                videoPanel.gameObject.SetActive(true);
                break;
            case SettingsTabs.audio:
                audioPanel.gameObject.SetActive(true);
                break;
            case SettingsTabs.misc:
                miscPanel.gameObject.SetActive(true);
                break;
        }
    }

    public void ClearTabs()
    {
        controlsPanel.gameObject.SetActive(false);
        videoPanel.gameObject.SetActive(false);
        audioPanel.gameObject.SetActive(false);
        miscPanel.gameObject.SetActive(false);
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
