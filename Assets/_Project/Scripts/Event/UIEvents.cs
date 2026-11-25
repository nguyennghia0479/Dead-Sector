using System;
using UnityEngine;

public static class UIEvents
{
    public static event Action OnNewGameBtnClicked;
    public static event Action OnMainMenuBtnClicked;
    public static event Action OnGameInputsBtnClicked;
    public static event Action OnSettingsBtnClicked;
    public static event Action OnCreditsBtnClicked;
    public static event Action<float> OnSensitivitySliderChanged;
    public static event Action<float> OnButtonClicked;

    public static void RaiseNewGameButtonClicked()
    {
        OnNewGameBtnClicked?.Invoke();
        RaiseButtonClicked();
    }

    public static void RaiseMainMenuButtonClicked()
    {
        OnMainMenuBtnClicked?.Invoke();
        RaiseButtonClicked();
    }

    public static void RaiseGameInputsButtonClicked()
    {
        OnGameInputsBtnClicked?.Invoke();
        RaiseButtonClicked();
    }

    public static void RaiseSettingButtonClicked()
    {
        OnSettingsBtnClicked?.Invoke();
        RaiseButtonClicked();
    }

    public static void RaiseCreditsButtonClicked()
    {
        OnCreditsBtnClicked?.Invoke();
        RaiseButtonClicked();
    }

    public static void RaiseSensitivitySliderChanaged(float value)
    {
        OnSensitivitySliderChanged?.Invoke(value);
    }

    public static void RaiseButtonClicked(float volume = .5f)
    {
        OnButtonClicked?.Invoke(volume);
    }
}
