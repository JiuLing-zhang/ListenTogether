﻿using ListenTogether.HandCursorControls;

namespace ListenTogether.Pages;

public partial class SettingsPage : ContentPage
{
    SettingPageViewModel vm => BindingContext as SettingPageViewModel;
    public SettingsPage(SettingPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        HandCursor.Binding();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        player.OnAppearing();
        await vm.InitializeAsync();
    }

    protected override void OnDisappearing()
    {
        player.OnDisappearing();
        base.OnDisappearing();
    }
}