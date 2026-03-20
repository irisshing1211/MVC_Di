# MVC_Di

使用 ASP.NET Core MVC + Dependency Injection + Entity Framework Core 實作的記帳示範專案。

## 功能

- Cookie Login / Logout
- 依登入使用者讀取個人記帳資料
- 新增記帳紀錄
- 以 DI 注入 `IAuthService`、`IRecordService`
- 自訂 `RequestTimingMiddleware`
- 啟動時自動建立資料庫並寫入 demo 資料

## 專案結構

- `MVC_Di.Web/Controllers`: MVC Controller
- `MVC_Di.Web/Services`: 業務邏輯與 DI 服務
- `MVC_Di.Web/Data`: `DbContext` 與 seed 資料
- `MVC_Di.Web/Middleware`: 自訂 middleware
- `MVC_Di.Web/Views`: Razor Views
- `MVC_Di.Web/wwwroot`: 靜態資源

## 開發環境

- .NET 8 SDK
- LibMan
  可用 Visual Studio / Rider 內建功能還原，或安裝 LibMan CLI 後執行命令

## 快速開始

1. 還原 NuGet 套件：

```bash
dotnet restore
```

2. 調整資料庫連線字串：

位置：`MVC_Di.Web/appsettings.json` 或 `MVC_Di.Web/appsettings.Development.json`

這個專案在 `Program.cs` 使用 `UseSqlServer(...)`，因此連線字串應改成 SQL Server 格式，例如：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=localhost;Initial Catalog=Ledger;Persist Security Info=True;User ID=sa;Password=YourStrong!Passw0rd;Trust Server Certificate=True;"
  }
}
```

3. 還原前端靜態資源：

```bash
libman restore MVC_Di.Web/libman.json
```

4. 啟動專案：

```bash
dotnet run --project MVC_Di.Web
```

## 預設帳號

第一次啟動後，系統會自動建立 demo 使用者：

- 帳號：`demo`
- 密碼：`demo123`

並新增兩筆範例記帳資料。

## 前端資源

`MVC_Di.Web/wwwroot/lib/` 已加入 `.gitignore`，改由 `MVC_Di.Web/libman.json` 管理。

如果你是新 clone 下來的專案，啟動前請先執行一次 `libman restore`。

## 說明

- 這個專案目前將帳號密碼以明文方式存放，適合教學與練習，不適合直接用於正式環境。
- 若要正式使用，至少應補上密碼雜湊、遷移管理、環境化設定與更完整的授權控管。
