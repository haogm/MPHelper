微信公众平台助手
--------
模拟登录微信公众平台，以程序代替人工操作，适用于未认证订阅号。

#### 已实现功能
* 获取所有用户消息列表
* 根据关键字检索消息列表
* 获取星标消息列表
* 获取单个用户对话消息列表
* 获取用户信息
* 设置/取消星标消息
* 更改用户分组
* 单用户消息发送
* 群组消息推送
* 获取后台文件

#### 依赖
* [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json)

#### 使用
```csharp
//公众账号用户名
var account = "test@test.com";
//公众账号密码MD5值
var passwordMd5 = "498a5846ae15e26c96cffd8e21eb483b";

var manager = MpManager.GetInstance(account, passwordMd5);

var openId = "xxxxxxxx";
//获取用户信息
var contact = manager.GetContactInfo(openId);

if (contact != null)
    Console.WriteLine(contact.nick_name);
```
