# 身體生理指標計算器工作報告

## 一、報告資訊

| 項目 | 內容 |
| --- | --- |
| 專案名稱 | InBodyCalculator |
| 執行日期 | 2026-07-22 |
| 報告時間 | 2026-07-22 16:05:25 |
| 專案狀態 | 開發完成，驗收通過 |
| 技術架構 | .NET 9 Minimal API、Vanilla JavaScript、Tailwind CSS 3、xUnit、Playwright |

## 二、工作目標

依據 `Requirement/InBodyUserStory.md` 與 `Requirement/InBodySDD.md`，從空白工作區建立可計算 BMI、BMR、TDEE 的同源 Web 應用程式，並完成欄位驗證、按鈕狀態連動、API 文件及自動化測試。

本次範圍不包含帳號、資料庫、歷史紀錄、國際化、容器化及雲端部署。

## 三、完成項目

### 3.1 規格校正

- 校正 175.5 cm、70 kg、25 歲男性的 BMR 範例為 1676.88 kcal／日。
- 校正 moderate 活動量的 TDEE 範例為 2599.16 kcal／日。
- 校正 BMI 範例為 22.73。
- 保留亞洲成人 BMI 分類門檻：`< 18.5`、`18.5..<24`、`24..<27`、`>=27`。
- 將 400 錯誤契約改為 RFC 相容的 `ValidationProblemDetails`，不依賴固定 `type` URI。

### 3.2 專案與後端

- 建立 `InBodyCalculator.sln`、.NET 9 Minimal API 及 xUnit 專案與參照。
- 實作 BMI、Mifflin-St Jeor BMR、五種活動量 TDEE 計算服務。
- 統一使用兩位小數及 `MidpointRounding.AwayFromZero`。
- 服務層保留正數、性別及活動量防衛檢查。
- 使用 DataAnnotations 與泛型 Endpoint Filter 產生欄位級驗證錯誤。
- 建立 `/api/calculate/bmi`、`/api/calculate/bmr`、`/api/calculate/tdee` 三個 POST 端點。
- 補上 endpoint 名稱、摘要、200 response 與 400 validation metadata。
- 啟用 Swagger、HTTPS redirect、靜態檔案及 SPA fallback。
- 前後端採同源託管，未加入寬鬆 CORS policy。

### 3.3 前端

- 建立 Vanilla JavaScript 單頁計算器。
- 提供身高、體重、整數年齡、性別與活動量欄位。
- 將欄位解析與 BMI／BMR／TDEE 合法性判斷集中於獨立模組。
- 依輸入完整度即時更新三個計算按鈕的 enabled／disabled 狀態。
- 各端點僅提交該計算所需的最小 payload。
- 支援 loading、成功結果、ProblemDetails 錯誤訊息與欄位錯誤狀態。
- 加入 label、`aria-describedby`、`aria-invalid`、`aria-live` 與穩定控制項尺寸。
- 完成 desktop 雙欄與 mobile 單欄響應式版面。
- 使用 Tailwind CSS 3 CLI 產生 minified production CSS，未使用 Tailwind Play CDN。

### 3.4 自動化測試

- 建立 CalculatorService xUnit 單元測試與 Minimal API integration tests。
- 建立 Playwright desktop／mobile E2E 測試。
- 覆蓋按鈕連動、三種成功計算、API 錯誤顯示、CSS 載入、固定按鈕高度與水平溢出檢查。
- 產生 desktop 與 mobile 測試截圖並完成人工檢視。

## 四、主要交付檔案

- [系統設計規格](../Requirement/InBodySDD.md)
- [Minimal API 入口](../src/InBodyCalculator.Api/Program.cs)
- [計算服務](../src/InBodyCalculator.Api/Services/CalculatorService.cs)
- [共用驗證 Filter](../src/InBodyCalculator.Api/Validation/ValidationFilter.cs)
- [前端頁面](../src/InBodyCalculator.Api/wwwroot/index.html)
- [前端互動程式](../src/InBodyCalculator.Api/wwwroot/js/app.js)
- [服務單元測試](../tests/InBodyCalculator.Tests/Services/CalculatorServiceTests.cs)
- [API 整合測試](../tests/InBodyCalculator.Tests/Endpoints/CalculationEndpointTests.cs)
- [Playwright 測試](../tests/InBodyCalculator.E2E/calculator.spec.js)

## 五、開發期間發現與處理

| 問題 | 處理結果 |
| --- | --- |
| SDD 的 BMR／TDEE 範例與公式不一致 | 依 Mifflin-St Jeor 公式修正文檔及測試期望值 |
| SDD 的 BMI 範例 22.72 未正確四捨五入 | 修正為 22.73 |
| 非整數年齡可能在 Endpoint Filter 前被 JSON binder 中止 | 以數值接收並透過自訂 DataAnnotation 驗證整數 |
| 前端建立三個結果模板時提早求值未使用欄位 | 將模板改為延遲求值，只產生目前選取的指標結果 |
| Playwright 對 visually-hidden radio 執行 `check()` 被 label 攔截 | 改以使用者實際可見的 segmented label 操作 |
| 舊 Playwright 與 PostCSS 版本有 npm audit 提示 | 升級至 Playwright 1.61.1 與 PostCSS 8.5.21，audit 歸零 |

## 六、驗收摘要

- Solution restore 與 build 成功，無編譯警告。
- xUnit 31 個測試全部通過。
- Playwright 8 個 desktop／mobile 測試全部通過。
- 前端與 E2E npm audit 均為 0 vulnerabilities。
- VS Code diagnostics 無錯誤。
- OpenAPI 三個端點、根頁及欄位級 400 已完成執行時 smoke check。

詳細測試紀錄請參閱 [測試報告](InBodyCalculator-TestReport-2026-07-22.md)。

## 七、執行資訊

- 應用程式：`https://localhost:7259`
- Swagger UI：`https://localhost:7259/swagger`
- 本機開發憑證若尚未受信任，瀏覽器可能顯示憑證提示。

## 八、結論

本次需求範圍已完成，主要計算、驗證契約、同源前端、響應式狀態與自動化測試均通過驗收，可進入需求確認或後續部署規劃階段。