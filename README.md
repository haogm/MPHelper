微信公众账号机器人工具
------

模拟登录微信公众账号后台，以程序代替人工操作。<br />
初衷是为了弥补有些公众号没有高级接口权限的缺憾。

### 已实现功能
1、获取所有用户消息列表<br />
2、获取单个用户对话消息列表<br />
3、获取用户信息<br />
4、发送信息（目前只支持文字消息）<br />
5、更改用户分组<br />

### 依赖
1、[Json.Net](https://www.nuget.org/packages/Newtonsoft.Json) (from Nuget)<br />
2、Hanger.dll (自己写的工具类库，可直接在Reference目录下引用dll)<br />

### 配置

需在AppSetting中，配置公众账号用户名及密码
```xml
<add key="MPAccount" value="foo" />
<add key="MPPassword" value="bar" />
```
