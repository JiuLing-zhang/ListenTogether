![](https://img.shields.io/github/license/JiuLing-zhang/ListenTogether)
[![](https://img.shields.io/github/v/release/JiuLing-zhang/ListenTogether)](https://github.com/JiuLing-zhang/ListenTogether)   

![logo](https://github.com/JiuLing-zhang/ListenTogether/raw/main/docs/resources/images/logo.svg)  
## 一起听
基于`.NET MAUI`开发的多平台、极简的在线音乐播放器。  

* 支持`Windows`、`Android`两个平台。（没有尊贵的 `APPLE` 调试环境:full_moon_with_face::full_moon_with_face:）  
* 支持**网易**、**酷狗**、**酷我**、**咪咕**数据源。  
* 不支持登录对应平台账号。  
* 程序只是整合了不同平台的音乐链接，当一个平台听不了时你可以方便的去另一个平台再碰碰运气:dog::dog:  

## 1、声明
该项目仅学习使用，仓库也不会打包和分发安装包:warning::warning::warning:  
该项目牵扯多家平台的协议分析，所以 **禁止`Fork` 禁止`Fork` 禁止`Fork`**。  
另外还请您不要将这些协议参数用于各种暴力途径。 

## 2、项目结构
```txt
src
  ├─ListenTogether              主程序
  ├─ListenTogether.Api          网络服务接口（同步歌单等）
  ├─ListenTogether.Business     业务逻辑
  ├─ListenTogether.Data         数据获取模块（本地数据或Api数据）
  ├─ListenTogether.EasyLog      简单的日志记录模块
  ├─ListenTogether.Model        通用模型
  ├─ListenTogether.Network      音乐源网站的数据提供服务
  └─NativeMediaMauiLib          本地播放模块（魔改自.NET博客项目）
```

`ListenTogether.Api`项目的[数据库脚本](https://github.com/JiuLing-zhang/ListenTogether/blob/main/docs/design/api_database.md)  

## 3、使用
直接下载安装就行。  

尊贵的`Windows`安装包需要签名，所以打包的时候进行了自签名，首次安装时需要信任证书。
戳这里看教程👉👉👉[`微软官方教程`](https://docs.microsoft.com/zh-cn/dotnet/maui/windows/deployment/overview#installing-the-app)  

## 4、服务配置

通过资源文件 `\ListenTogether\Resources\Raw\NetConfig.json` 设置**同步歌单**、**自动更新**服务。  
> 同步歌单时，发布`ListenTogether.Api`项目即可。  
```json
{
  "UpdateDomain": "自动更新地址",
  "ApiDomain": "歌单服务地址",
}
```

## 5、来几张图片

### Windows

![win_playing.png](https://github.com/JiuLing-zhang/ListenTogether/raw/main/docs/resources/images/windows_playing.png)  

![windows_playlist.png](https://github.com/JiuLing-zhang/ListenTogether/raw/main/docs/resources/images/windows_playlist.png)  

### Android

![phone_playing.jpg](https://github.com/JiuLing-zhang/ListenTogether/raw/main/docs/resources/images/android.png)  

## 6、已知问题
[点击查看Issues](https://github.com/JiuLing-zhang/ListenTogether/issues):bug:

## 7、开源协议
本项目基于`GPL-3.0 license`协议。  
