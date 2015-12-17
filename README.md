微信公众平台助手
------
模拟登录微信公众平台，以程序代替人工操作

### 已实现功能
1、获取所有用户消息列表
2、根据关键字检索消息列表
3、获取星标消息列表
4、获取单个用户对话消息列表
5、获取用户信息
6、设置/取消星标消息
7、更改用户分组
8、单用户消息发送
9、群组消息推送
10、获取后台文件

### 依赖
1、[Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json)

### 使用
```csharp
//公众账号用户名
var _account = "test@test.com";
//公众账号密码MD5值
var _passwordMd5 = "498a5846ae15e26c96cffd8e21eb483b";

var manager = new MPManager(_account, _passwordMd5);

//获取用户信息
var contact = await manager.GetContactInfoAsync(FAKE_ID);
```
