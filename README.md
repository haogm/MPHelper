微信公众平台助手
------

模拟登录微信公众平台，以程序代替人工操作。<br />
初衷是为了弥补有些公众号没有高级接口权限的缺憾。

### 已实现功能

```csharp
/// <summary>
/// 获取所有用户消息列表
/// </summary>
/// <param name="count">消息条数</param>
/// <param name="day">0：今天；1：昨天；以此类推，后台最多保存5天数据，默认全部消息</param>
public static async Task<IList<MessageItem>> GetAllMessageListAsync(int count = 20, int day = 7);

/// <summary>
/// 获取星标消息列表
/// </summary>
/// <param name="count">消息条数</param>
public static async Task<IList<MessageItem>> GetStarMessageListAsync(int count = 20);

/// <summary>
/// 设置/取消星标消息
/// </summary>
/// <param name="messageId">消息ID</param>
/// <param name="isStar">是否为星标</param>
public static async Task<bool> SetStarMessageAsync(string messageId, bool isStar);

/// <summary>
/// 获取单个用户对话消息列表
/// </summary>
/// <param name="fakeId">用户FakeId</param>
public static async Task<IList<MessageItem>> GetSingleSendMessageListAsync(string fakeId);

/// <summary>
/// 更改用户分组（0：未分组； 1：黑名单； 2：星标组）
/// </summary>
/// <param name="fakeId">用户FakeId</param>
/// <param name="cateId">分组ID</param>
public static async Task<bool> ChangeCategoryAsync(string fakeId, string cateId);

/// <summary>
/// 单用户消息发送（目前只支持文字消息）
/// </summary>
/// <param name="fakeId">用户FakeId</param>
/// <param name="message">文字消息</param>
public static async Task<bool> SingleSendMessageAsync(string fakeId, string message);

/// <summary>
/// 获取用户信息
/// </summary>
/// <param name="fakeId">用户FakeId</param>
public static async Task<ContactInfo> GetContactInfoAsync(string fakeId);
```

### 依赖
1、.Net Frameword版本: 4.5.1<br />
2、[Json.Net](https://www.nuget.org/packages/Newtonsoft.Json) (from Nuget)<br />

### 配置
需在AppSetting中，配置公众账号用户名及密码MD5
```xml
<add key="MPAccount" value="公众号登录用户名" />
<add key="MPPasswordMD5" value="公众号登录密码MD5" />
```
