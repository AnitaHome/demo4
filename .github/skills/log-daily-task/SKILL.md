---
name: log-daily-task
description: 'Use when saving the user prompt and the model response (chat history logs) into a daily log file located at .\logs\[yyyy-MM-dd].md. Facilitates recording prompt-response turns in a standardized format.'
argument-hint: '請提供要紀錄的 Prompt 內容與 Model 回覆內容'
user-invocable: true
---
# Daily Task Logging Skill

本 Skill 專門用於將當次對話的 Prompt 與 Model 的回覆記錄至每日任務日誌中，日誌檔案將統一存放在本工作區 root 目錄下的 `./logs/[yyyy-MM-dd].md`。

## Triggers
- 用戶要求「記錄今日任務」、「儲存對話紀錄」、「寫入任務 log」。
- 完成特定開發任務後，需要將 Prompt 和 Model 的解決方案記錄成檔。

## Procedure
1. **確認今日日期與目標檔案路徑**：
   - 取得當前系統日期，格式為 `yyyy-MM-dd`（例如 `2026-07-22`）。
   - 目標檔案路徑應為：`./logs/yyyy-MM-dd.md`（相對於工作區根目錄）。

2. **確認 `./logs` 資料夾與目標檔案是否存在**：
   - 如果 `./logs/` 資料夾不存在，應先予以建立。
   - 檢查該 `yyyy-MM-dd.md` 檔案是否存在。

3. **格式化紀錄內容**：
   - 讀取本次要記錄的 Prompt (用戶輸入) 與 Model (助理回答)。
   - **計數(n)規定**：
     - 若目標檔案**不存在**：則本次為 `Prompt[1]` 與 `Model`。
     - 若目標檔案**已存在**：先讀取（或分析）檔案內容中已有的 `### Prompt[n]:` 數量，將當前記錄編號設為 `n+1`。
   - **格式範本**：
     ```markdown
     ### Prompt[n]: [在此放入用戶提示詞/問題]
      (Model): [在此放入助理回覆的主要程式碼或回答摘要]
     ```

4. **寫入檔案**：
   - 若檔案不存在：以新檔形式寫入。
   - 若檔案已存在：在原檔案的**最後面**追加（Append）新內容，與前段內容保留一個空行。

5. **確認與反饋**：
   - 確認檔案已正確寫入或更新。
   - 回報用戶已成功記錄至哪個檔案及 Prompt 編號。
