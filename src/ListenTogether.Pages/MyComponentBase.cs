using JiuLing.CommonLibs.ExtensionMethods;
using ListenTogether.Data;
using Microsoft.AspNetCore.Components;

namespace ListenTogether.Pages;
public class MyComponentBase : ComponentBase
{
    [Inject]
    public ILoginDataStorage _loginDataStorage { get; set; } = null!;

    public bool IsLogin => _loginDataStorage.GetUsername().IsNotEmpty();
    public bool IsNotLogin => !IsLogin;
}