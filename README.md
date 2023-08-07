<div style="display: flex;justify-content: center;align-items: center;margin-bottom: 10px;">
<img src="https://github.com/JiuLing-zhang/ListenTogether/raw/main/docs/resources/images/logo.svg" width="50px">
<div style="margin-left: 10px;font-size: 25px;color: #C98FFF;">一起听</div>
</div>
<div style="display: flex;justify-content: center;margin-bottom: 20px;">

![](https://img.shields.io/github/license/JiuLing-zhang/ListenTogether)
[![](https://img.shields.io/github/v/release/JiuLing-zhang/ListenTogether)](https://github.com/JiuLing-zhang/ListenTogether)   

</div>


基于`.NET MAUI` / `.NET MAUI Blazor`开发的多平台、极简的在线音乐播放器。  

<div align="center">
<img src="https://github.com/JiuLing-zhang/ListenTogether/raw/main/docs/resources/images/android.png" width="60%">

<img src="https://github.com/JiuLing-zhang/ListenTogether/raw/main/docs/resources/images/windows_discover.png" width="45%">

<img src="https://github.com/JiuLing-zhang/ListenTogether/raw/main/docs/resources/images/windows_playing.png" width="45%">
</div>

支持 `Windows`、`Android`、~~`IOS`~~、~~`MacCatalyst`~~ 。（没有尊贵的 `APPLE` 调试环境，所以不确定程序是否能够正常运行:full_moon_with_face::full_moon_with_face:）  

## 1、声明  
该项目仅学习使用，所以仓库不会打包和分发安装包:warning::warning::warning:  

## 2、项目结构
### 2.1 `MAUI Blazor` 版的程序代码  
```txt
  ├─ListenTogether.Model              通用模型
  ├─ListenTogether.Pages              所有功能页面
  ├─ListenTogether.Service.Common     通用的服务实现
  ├─ListenTogether.Service.Interface  通用的接口定义
  ├─ListenTogether.Service.Maui       平台相关的服务实现
  ├─ListenTogetherMauiBlazor          主程序
  ├─NativeMediaMauiLib                本地播放模块
  └─NetMusicLib                       歌曲模块
```
* 页面基于 [`MudBlazor`](https://github.com/MudBlazor/MudBlazor/) 框架开发。  
* 部分图标使用 [`Font Awesome`](https://fontawesome.com/)  
* 目前是把所有页面单独集成到一个项目中，因为以后打算开发 `Blazor` 版本。  

### 2.2 `MAUI` 版的程序代码 
```txt
ListenTogetherMaui.sln
  ├─ListenTogether                    主程序
  ├─ListenTogether.Model              通用模型
  ├─ListenTogether.Service.Common     通用的服务实现
  ├─ListenTogether.Service.Interface  通用的接口定义
  ├─ListenTogether.Service.Maui       平台相关的服务实现
  ├─NativeMediaMauiLib                本地播放模块
  └─NetMusicLib                       歌曲模块
```
* **该项目后续应该基本不会维护了，因为框架本身的 `bug` 真心多**

### 2.3 `API` 项目的程序代码  
```txt
ListenTogetherApi.sln
  ├─ListenTogether.Api    网络服务接口（用来同步歌单）
  └─ListenTogether.Model  通用模型
```
* 该项目使用 `PostgreSQL` 数据库，这是[表结构脚本](https://github.com/JiuLing-zhang/ListenTogether/blob/main/docs/design/api_database.md)  

### 2.4 说明  

点击查看 [`NativeMediaMauiLib`](https://github.com/JiuLing-zhang/NativeMediaMauiLib) 和 [`NetMusicLib`](https://github.com/JiuLing-zhang/NetMusicLib) 的项目代码。  

对于 `MAUI` 和 `MAUI Blazor` 项目，下载后，手动添加资源文件 `主程序\Resources\Raw\NetConfig.json`（缺少文件时会编译不通过）。  
```json
{
  "UpdateDomain": "自动更新地址",
  "ApiDomain": "歌单服务地址",
}
``` 

## 3、开源协议
本项目基于`GPL-3.0 license`协议。  
