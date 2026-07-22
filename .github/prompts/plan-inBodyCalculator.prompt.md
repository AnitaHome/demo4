## Plan: 建置身體生理指標計算器

從空白工作區建立 .NET 9 Minimal API 與同站託管的 Vanilla JavaScript/Tailwind 前端；核心計算集中於可測試服務，輸入由 DataAnnotations 與泛型 Endpoint Filter 統一轉成欄位級 ValidationProblemDetails，並以 xUnit API/服務測試和 Playwright E2E 覆蓋需求中的計算與按鈕狀態。

**Steps**

### 第一階段：規格校正與專案骨架
1. 校正 `c:\Shares\mydemo\demo4\Requirement\InBodySDD.md` 中與 Mifflin-St Jeor 公式矛盾的範例：175.5 cm、70 kg、25 歲男性 BMR 應為 1676.88，moderate TDEE 應為 2599.16；保留 BMI 分類門檻 `<18.5`、`18.5..<24`、`24..<27`、`>=27`，並將 400 契約統一描述為 RFC 相容的 `ValidationProblemDetails`，不綁定已過時的固定 `type` URI。
2. 在工作區根目錄建立 `InBodyCalculator.sln`、`src/InBodyCalculator.Api`（`net9.0` Minimal API）及 `tests/InBodyCalculator.Tests`（`net9.0` xUnit），加入專案參照與 solution；API 啟用 nullable、OpenAPI/Swagger 所需套件及靜態檔案託管。此步驟阻擋後續所有程式實作。
3. 在 API 專案建立 `wwwroot` 與前端 npm 工具鏈，固定 Tailwind 3.x 以符合 SDD 的 CLI/config 流程；設定 `tailwind.config.js` 掃描 HTML/JS、`src/input.css`、輸出 `wwwroot/css/output.css`，並提供 `dev`、`build:css`、`test:e2e` scripts。正式頁面只引用編譯 CSS，不依賴 Play CDN。

### 第二階段：後端核心與 API
4. 建立 request/response records：`BmiRequest`、`BmrRequest`、`TdeeRequest` 以 DataAnnotations 表達必填、正數、整數與允許值；`BmiResponse`、`BmrResponse`、`TdeeResponse` 維持 SDD 的 camelCase JSON 契約。性別僅接受 `male`/`female`，活動量僅接受五個列舉字串，採不分大小寫處理但 OpenAPI 範例使用小寫。
5. 實作 `ICalculatorService` 與無狀態 `CalculatorService`：BMI、Mifflin-St Jeor BMR、TDEE 活動乘數與兩位小數四捨五入集中在服務；服務本身保留參數防衛，避免繞過 HTTP 驗證時產生無效結果。BMI 類別沿用 SDD 的亞洲門檻，而非改採 WHO 25/30 門檻。
6. 實作泛型 `ValidationFilter<TRequest>`，使用 DataAnnotations 收集欄位錯誤並回傳 `Results.ValidationProblem`；在 `Program.cs` 建立 `/api/calculate` route group，映射三個 POST handler、註冊 scoped/singleton 無狀態服務、OpenAPI/Swagger、HTTPS/靜態檔案與 SPA fallback。因前端同源託管，不啟用 `AllowAnyOrigin` CORS。
7. 為三個端點補上名稱、摘要、`Produces`/`ProducesValidationProblem` metadata，使 Swagger 明確呈現 200 response 與 400 欄位錯誤契約；開發環境提供 Swagger UI，根路徑呈現實際計算器。

### 第三階段：前端體驗
8. 建立 `wwwroot/index.html` 的單頁計算器：身高、體重、整數年齡、性別 segmented radio、活動量 select、三個具穩定尺寸的計算按鈕，以及具 `aria-live` 的結果與錯誤區；採清楚、克制的健康工具視覺，不建立行銷 landing page。
9. 在 `wwwroot/js/validation.js` 實作可獨立測試的欄位解析與 `isBmiValid`/`isBmrValid`/`isTdeeValid` 判斷；在 `api-client.js` 封裝同源 POST 與 ProblemDetails 解析；在 `app.js` 綁定 `input`/`change`、初始化 disabled 狀態、提交對應最小 payload、呈現 loading/result/error，並確保輸入變更立即重算三個按鈕狀態。
10. 完成響應式與可及性狀態：disabled/loading/focus/error 樣式不造成版面位移；mobile/desktop 均無文字溢出或控制項重疊；數值欄位具有適當 `min`、`step`、label 與錯誤關聯，但最終合法性仍由 JavaScript 與 API 雙層判定。

### 第四階段：自動化驗證
11. 在 `CalculatorServiceTests` 依 AAA 與 `Method_Scenario_Expected` 命名覆蓋：BMI 各分類邊界與非法尺寸、男女 BMR 正確值與非法性別、五種 TDEE 乘數與非法活動量、兩位小數結果。
12. 使用 `Microsoft.AspNetCore.Mvc.Testing` 建立 API integration tests，覆蓋三個成功 response 的狀態碼/JSON，以及零值、負值、缺欄位、非整數年齡、非法性別與非法活動量的 400 `application/problem+json` 和欄位 errors。此步驟可與步驟 13 平行。
13. 在 `tests/InBodyCalculator.E2E` 設定 Playwright，透過 `webServer` 啟動 API；測試初始全 disabled、逐欄輸入後 BMI/BMR/TDEE 依序啟用、欄位清空後重新停用、三種計算成功呈現，以及 API 錯誤可讀呈現。另以 desktop/mobile viewport 截圖檢查版面與實際 CSS 載入。
14. 執行完整建置與驗收，修正僅限需求範圍內的失敗；確認 production CSS 已 minify、API/OpenAPI 可啟動、前端由同一 origin 載入且三條工作流完成。

**Relevant files**
- `c:\Shares\mydemo\demo4\Requirement\InBodySDD.md` — 校正公式範例與 400 回應契約。
- `c:\Shares\mydemo\demo4\InBodyCalculator.sln` — API 與 xUnit 測試專案入口。
- `c:\Shares\mydemo\demo4\src\InBodyCalculator.Api\Program.cs` — DI、route group、filter、OpenAPI、Swagger 與靜態託管組態。
- `c:\Shares\mydemo\demo4\src\InBodyCalculator.Api\Models\Requests\*.cs` — DataAnnotations request contracts。
- `c:\Shares\mydemo\demo4\src\InBodyCalculator.Api\Models\Responses\*.cs` — API response records。
- `c:\Shares\mydemo\demo4\src\InBodyCalculator.Api\Validation\ValidationFilter.cs` — 共用欄位級 ProblemDetails 驗證。
- `c:\Shares\mydemo\demo4\src\InBodyCalculator.Api\Services\ICalculatorService.cs` — 計算服務契約。
- `c:\Shares\mydemo\demo4\src\InBodyCalculator.Api\Services\CalculatorService.cs` — BMI/BMR/TDEE 公式與分類核心。
- `c:\Shares\mydemo\demo4\src\InBodyCalculator.Api\wwwroot\index.html` — 同站單頁 UI 與可及性結構。
- `c:\Shares\mydemo\demo4\src\InBodyCalculator.Api\wwwroot\js\validation.js` — 前端欄位與按鈕有效性。
- `c:\Shares\mydemo\demo4\src\InBodyCalculator.Api\wwwroot\js\api-client.js` — API 與 ProblemDetails 處理。
- `c:\Shares\mydemo\demo4\src\InBodyCalculator.Api\wwwroot\js\app.js` — DOM 事件、提交與結果狀態。
- `c:\Shares\mydemo\demo4\src\InBodyCalculator.Api\package.json` — Tailwind build 與 E2E scripts。
- `c:\Shares\mydemo\demo4\tests\InBodyCalculator.Tests\Services\CalculatorServiceTests.cs` — AAA 核心單元測試。
- `c:\Shares\mydemo\demo4\tests\InBodyCalculator.Tests\Endpoints\CalculationEndpointTests.cs` — Minimal API 整合與 ProblemDetails 測試。
- `c:\Shares\mydemo\demo4\tests\InBodyCalculator.E2E\playwright.config.js` — 同站 API 啟動與 viewport 設定。
- `c:\Shares\mydemo\demo4\tests\InBodyCalculator.E2E\calculator.spec.js` — UI 狀態與完整工作流。

**Verification**
1. `dotnet restore InBodyCalculator.sln` 與 `dotnet build InBodyCalculator.sln --no-restore` 必須成功且無編譯警告。
2. `dotnet test InBodyCalculator.sln --no-build` 必須通過所有服務及 API integration tests。
3. 在 `src/InBodyCalculator.Api` 執行 `npm ci`、`npm run build:css`，確認 minified CSS 產生且頁面不引用 Tailwind CDN。
4. 執行 `npm run test:e2e`，Playwright 必須通過 desktop/mobile 的按鈕連動、成功計算、錯誤處理與截圖檢查。
5. `dotnet run --project src/InBodyCalculator.Api` 後人工檢查 `/swagger` 三個 POST 契約、根頁 UI、BMI/BMR/TDEE 範例值與非法 payload 的欄位級 400。

**Decisions**
- 包含完整前端、後端、服務單元測試、API 整合測試與 Playwright E2E；不包含帳號、資料庫、歷史紀錄、國際化、容器化或雲端部署。
- 前端由 API 的 `wwwroot` 同站託管；不採獨立 frontend deployment，也不加入寬鬆 CORS。
- 驗證使用平台內建 DataAnnotations、Endpoint Filter 與 `ValidationProblemDetails`；不加入 FluentValidation 或自製 exception middleware。
- 公式是規格真實來源；錯誤的示例數字會修正文檔，不會讓實作迎合錯誤示例。
- 使用 Tailwind 3.x 以維持 SDD 指定的 `tailwind.config.js` 與 CLI purge/minify 流程；`output.css` 為可重建產物。
- 數值只套用需求明示的 `> 0` 約束，不自行增加身高、體重或年齡上限。
