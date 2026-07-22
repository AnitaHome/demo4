# 身體生理指標計算器測試報告

## 一、報告資訊

| 項目 | 內容 |
| --- | --- |
| 專案名稱 | InBodyCalculator |
| 測試日期 | 2026-07-22 |
| 報告時間 | 2026-07-22 16:05:25 |
| 測試結論 | 通過 |

## 二、測試環境

| 元件 | 版本／環境 |
| --- | --- |
| 作業系統 | Windows |
| .NET SDK | 10.0.300，目標框架 net9.0 |
| Node.js | 24.18.0 |
| npm | 11.16.0 |
| xUnit | 2.9.2 |
| Microsoft.AspNetCore.Mvc.Testing | 9.0.6 |
| Playwright | 1.61.1 |
| 瀏覽器引擎 | Chromium／Chrome for Testing |
| Tailwind CSS | 3.4.17 |

## 三、測試範圍

1. CalculatorService 計算公式、分類邊界、兩位小數與非法參數防衛。
2. 三個 Minimal API 端點的成功 response、JSON 契約與欄位級 400 response。
3. 前端必要欄位與三個按鈕的動態 enabled／disabled 狀態。
4. BMI、BMR、TDEE 三條瀏覽器端完整計算流程。
5. API ProblemDetails 錯誤的前端可讀呈現。
6. Desktop／mobile 響應式版面、CSS 載入、按鈕高度及水平溢出。
7. OpenAPI 路徑、根頁與實際 HTTPS host smoke check。

## 四、測試結果總覽

| 測試項目 | 結果 | 詳細結果 |
| --- | --- | --- |
| `dotnet restore InBodyCalculator.sln` | 通過 | 還原成功 |
| `dotnet build InBodyCalculator.sln --no-restore` | 通過 | 0 errors，無編譯警告 |
| `dotnet test InBodyCalculator.sln --no-build` | 通過 | 31 passed，0 failed，0 skipped |
| Frontend `npm ci` | 通過 | 依 lockfile 安裝成功，0 vulnerabilities |
| `npm run build:css` | 通過 | minified CSS 產生成功 |
| E2E `npm ci` | 通過 | 依 lockfile 安裝成功，0 vulnerabilities |
| `npm run test:e2e` | 通過 | 8 passed，0 failed |
| VS Code diagnostics | 通過 | `src` 與 `tests` 無錯誤 |
| Runtime smoke check | 通過 | OpenAPI、根頁、400 ProblemDetails 均符合預期 |

## 五、xUnit 測試明細

### 5.1 CalculatorService 單元測試

| 測試類別 | 覆蓋內容 | 結果 |
| --- | --- | --- |
| BMI 分類邊界 | 18.49、18.5、24、27 | 通過 |
| BMI 非法尺寸 | 身高／體重為 0 或負數 | 通過 |
| BMR 男女性別 | male、FEMALE 大小寫處理及正確值 | 通過 |
| BMR 非法參數 | 身高、體重、年齡無效及非法性別 | 通過 |
| TDEE 活動乘數 | sedentary、light、moderate、heavy、athlete | 通過 |
| TDEE 非法活動量 | 不支援的活動量字串 | 通過 |
| 兩位小數 | BMI、BMR、TDEE 結果 | 通過 |

### 5.2 API Integration Tests

| 測試類別 | 覆蓋內容 | 結果 |
| --- | --- | --- |
| BMI 成功 response | HTTP 200、`bmi`、`category` | 通過 |
| BMR 成功 response | HTTP 200、`bmr` | 通過 |
| TDEE 成功 response | HTTP 200、`bmr`、`tdee` | 通過 |
| 零值／負值 | Height、Weight 欄位錯誤 | 通過 |
| 缺少欄位 | Height、Gender、ActivityLevel | 通過 |
| 非整數年齡 | Age 欄位錯誤 | 通過 |
| 非法允許值 | Gender、ActivityLevel | 通過 |
| 錯誤媒體類型 | `application/problem+json` | 通過 |

## 六、Playwright E2E 測試明細

Playwright 以 desktop 1440 × 900 與 Pixel 7 mobile viewport 執行下列 4 個案例，共 8 個測試。

| 案例 | Desktop | Mobile |
| --- | --- | --- |
| 初始停用、逐欄啟用及清空後重新停用 | 通過 | 通過 |
| BMI、BMR、TDEE 成功結果 | 通過 | 通過 |
| API validation error 可讀呈現 | 通過 | 通過 |
| Production CSS、48px 按鈕及無水平溢出 | 通過 | 通過 |

測試執行時產生 desktop 與 mobile full-page 截圖，人工檢視結果未發現控制項重疊、文字裁切或不合理水平捲動。

## 七、基準資料驗證

| 指標 | 輸入 | 預期結果 | 實際結果 |
| --- | --- | --- | --- |
| BMI | 175.5 cm、70 kg | 22.73、Normal | 通過 |
| BMR | 175.5 cm、70 kg、25 歲、male | 1676.88 kcal／日 | 通過 |
| TDEE | 上述資料、moderate | 2599.16 kcal／日 | 通過 |

## 八、Runtime Smoke Check

- `GET /` 回傳 HTTP 200 與 `text/html`。
- Swagger OpenAPI 文件包含 `/api/calculate/bmi`、`/api/calculate/bmr`、`/api/calculate/tdee`。
- BMR request 使用 `age: 25.5` 時回傳 HTTP 400、`application/problem+json`，且 `errors.Age` 包含 `Age must be an integer.`。

## 九、剩餘風險與未涵蓋項目

- E2E 目前僅使用 Chromium，未執行 Firefox 與 WebKit 相容性測試。
- 未執行負載、壓力、長時間穩定性及滲透測試。
- 未建立雲端部署、反向代理或正式 TLS 憑證環境測試。
- BMI、BMR、TDEE 屬公式估算值，不應作為醫療診斷依據。
- Browserslist 會提示 `caniuse-lite` 資料可更新，但不影響本次 CSS 建置與驗收結果。

## 十、測試結論

本次所有自動化測試、建置驗證、npm audit、視覺檢查及 runtime smoke check 均通過。系統符合目前 User Story 與 SDD 定義的計算、驗證、按鈕狀態及同源前端需求。