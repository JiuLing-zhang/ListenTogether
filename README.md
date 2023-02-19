<div style="display: flex;justify-content: center;align-items: center;margin-bottom: 10px;">
<img src="https://github.com/JiuLing-zhang/ListenTogether/raw/main/docs/resources/images/logo.svg" width="50px">
<div style="margin-left: 10px;font-size: 25px;color: #C98FFF;">一起听</div>
</div>
<div style="display: flex;justify-content: center;margin-bottom: 20px;">

![](https://img.shields.io/github/license/JiuLing-zhang/ListenTogether)
[![](https://img.shields.io/github/v/release/JiuLing-zhang/ListenTogether)](https://github.com/JiuLing-zhang/ListenTogether)   

</div>


基于`.NET MAUI`开发的多平台、极简的在线音乐播放器。  

* 支持 `Windows`、`Android`、~~`IOS`~~、~~`MacCatalyst`~~ 。（没有尊贵的 `APPLE` 调试环境，所以不确定程序是否能够正常运行:full_moon_with_face::full_moon_with_face:）  
* 支持**网易**、**酷狗**、**酷我**、**咪咕**数据源。  
* 不支持登录对应平台账号。  
* 程序只是整合了不同平台的音乐链接，当一个平台听不了时你可以方便的去另一个平台再碰碰运气:dog::dog:  

<div align="center">
<img src="https://github.com/JiuLing-zhang/ListenTogether/raw/main/docs/resources/images/android.png" width="60%">

<img src="https://github.com/JiuLing-zhang/ListenTogether/raw/main/docs/resources/images/windows_discover.png" width="45%">

<img src="https://github.com/JiuLing-zhang/ListenTogether/raw/main/docs/resources/images/windows_playing.png" width="45%">
</div>

## 1、声明  
该项目仅学习使用，本人也不会打包和分发安装包:warning::warning::warning:  
该项目牵扯多家平台的协议分析，所以 **禁止`Fork` 禁止`Fork` 禁止`Fork`**。  
另外还请您不要将这些协议参数用于非法途径。 

## 2、项目结构
```txt
src
  ├─ListenTogether              主程序
  ├─ListenTogether.Api          网络服务接口（用来同步歌单）
  ├─ListenTogether.Business     业务逻辑
  ├─ListenTogether.Data         数据获取模块（本地数据或Api数据）
  ├─ListenTogether.EasyLog      简单的日志记录模块
  ├─ListenTogether.Model        通用模型
  ├─ListenTogether.Network      音乐源网站的数据提供服务
  ├─ListenTogether.Storage      本地存储模块
  └─NativeMediaMauiLib          本地播放模块（魔改自.NET博客项目）
```

`ListenTogether.Api` 项目使用 `PostgreSQL` 数据库，这是[表结构脚本](https://github.com/JiuLing-zhang/ListenTogether/blob/main/docs/design/api_database.md)  

## 3、配置文件

项目下载后，手动添加资源文件 `\ListenTogether\Resources\Raw\NetConfig.json`（缺少文件时会编译不通过）。  
该文件用于配置**同步歌单**、**自动更新**服务。  
```json
{
  "UpdateDomain": "自动更新地址",
  "ApiDomain": "歌单服务地址",
}
```
> 同步歌单时，发布`ListenTogether.Api`项目即可。  

## 4、已知问题
[点击查看Issues](https://github.com/JiuLing-zhang/ListenTogether/issues):bug:

## 5、开源协议
本项目基于`GPL-3.0 license`协议。  
