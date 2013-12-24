微信公众平台助手
------
模拟登录微信公众平台，以程序代替人工操作

### 已实现功能
1、获取所有用户消息列表<br />
2、获取星标消息列表<br />
3、获取单个用户对话消息列表<br />
4、获取用户信息<br />
5、设置/取消星标消息<br />
6、更改用户分组<br />
7、单用户消息发送<br />
8、群组消息推送<br />

### 依赖
1、.Net Frameword版本: 4.5.1<br />
2、[Json.Net](https://www.nuget.org/packages/Newtonsoft.Json) (from Nuget)<br />

### 使用
在构造函数中传入公众账号用户名及密码MD5值
```csharp
var _account = "test@test.com";
var _passwordMd5 = "498a5846ae15e26c96cffd8e21eb483b";

var manager = new MPManager(_account, _passwordMd5);
var contactInfo = await manager.GetContactInfoAsync(FAKE_ID);
```
