## 一起听
基于`.NET MAUI`开发的多平台、极简的在线音乐播放器。  

## 声明
该项目仅供学习使用。  
该项目牵扯多家平台的协议分析，所以 **禁止`Fork` 禁止`Fork` 禁止`Fork`**。  
另外还请您不要将这些协议参数用于各种暴力途径。 

## 说明
* 没有尊贵的 `APPLE` 调试环境，因此我只编译了`Windows`和`Android`。  
* 目前支持**网易**、**酷狗**、**酷我**、**咪咕**几个数据源。  
* 程序只是整合了不同平台的音乐链接，并不是破解会员啊什么的巴拉巴拉~~  
* 原来免费听不了的歌曲，现在依然听不了，唯一的好处就是如果有平台免费听不了，可以尝试搜下其他平台。  
* 程序不支持登录几大音乐网站。  

## 项目结构
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

## 使用
直接下载安装就行。  

尊贵的`Windows`安装包需要签名，所以打包的时候进行了自签名，首次安装时需要信任证书。
戳这里看教程👉👉👉[`微软官方教程`](https://docs.microsoft.com/zh-cn/dotnet/maui/windows/deployment/overview#installing-the-app)  

## 关于同步歌单
程序默认未配置服务器地址，因此为单机版本（无法同步歌单）。  
需要同步歌单时，可以自己发布下`ListenTogether.Api`项目（用`.NET 6`写的），然后在程序设置页面配置下服务器地址即可。  

## 来几张图片

### Windows

![win_playing.png](https://s2.loli.net/2022/08/12/tmIvjS81ukFG5hM.png)  

![win_search.png](https://s2.loli.net/2022/08/12/YTpZHV9xbol8XsD.png)  

### Android

![phone_playing.jpg](https://s2.loli.net/2022/08/12/rbF6qPntT97lG2I.jpg)  

![phone_playlist.jpg](https://s2.loli.net/2022/08/12/82bqSKjsfDCReBL.jpg)  