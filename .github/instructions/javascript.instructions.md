---
name: "JavaScript Coding Standards"
description: "Use when creating, editing, refactoring, or reviewing any JavaScript source code (.js) to ensure alignment with standard modern JavaScript (ES6+) coding conventions based on Airbnb JavaScript style guide."
applyTo: "**/*.js"
---
# JavaScript 程式碼規範

此規範定義了 JavaScript 的程式碼風格與撰寫慣例（基於 **Airbnb JavaScript Style Guide**），以保持程式碼的一致性與高品質。

## 1. 命名規範 (Naming Conventions)

- **camelCase (小駝峰命名法)**:
  - 適用於：變數 (Variables)、物件屬性 (Object properties)、函數 (Functions)、方法 (Methods)。
  - *範例*: `const userProfile = {};`, `function calculateTotalAmount() {}`
- **PascalCase (大駝峰命名法)**:
  - 適用於：類別 (Classes)、建構函數 (Constructor functions)。
  - *範例*: `class UserSession {}`
- **UPPERCASE_WITH_UNDERSCORES (蛇形大寫)**:
  - 適用於：全域或模組級別的常數 (Constants)。
  - *範例*: `const MAX_RETRY_COUNT = 3;`
- **底線開頭 `_camelCase`**:
  - 適用於：類別的私有屬性或方法（若無使用 `#private` 語法時）。
  - *範例*: `this._internalId = id;`

## 2. 變數與宣告 (Variables & Scope)

- **避免使用 `var`**:
  - 一律使用 `const` 宣告，只有在確定變數需要重新賦值時才使用 `let`。
- **一個宣告佔一行**:
  - 每個變數宣告應該獨立成行。
  - *範例*:
    ```javascript
    const count = 1;
    const name = 'John';
    ```

## 3. 分號與引號 (Semicolons & Quotes)

- **結尾必須有分號 `;`**:
  - 每個語句的結尾必須明確加上分號，不依賴 JavaScript 的自動插入分號 (ASI) 機制。
- **使用單引號 `'`**:
  - 一般字串字面量一律使用單引號 `'`。
  - 只有在字串中包含單引號時才可使用雙引號 `"`，或者使用模板字串。
- **模板字串 (Template Literals)**:
  - 當需要動態拼接字串時，一律使用反單引號 (`` ` ``) 與 `${expression}` 機制，避免使用 `+` 串接字串。

## 4. 排版與物件/陣列規範 (Formatting, Objects & Arrays)

- **縮排**:
  - 使用 2 個空格 (Spaces) 進行縮排，不使用 Tab。
- **大括號 `{}`**:
  - 大括號與前方的關鍵字或函數宣告在同一行（OTBS / One True Brace Style）。
  - *範例*:
    ```javascript
    if (isReady) {
      // do something
    } else {
      // do something else
    }
    ```
- **解構賦值 (Destructuring)**:
  - 從物件或陣列中讀取多個屬性時，優先使用解構賦值。
  - *範例*: `const { name, age } = user;`
- **箭頭函數 (Arrow Functions)**:
  - 優先使用箭頭函數定義匿名函數、回呼函數 (Callbacks)。
  - 當箭頭函數只有單一參數時，省略圓括號；沒有參數或複數參數時則必須加上圓括號。

## 5. 程式碼範例 (Code Example)

```javascript
const MAX_TEMPORARY_STORAGE = 100;

class OrderManager {
  constructor(userId) {
    this.userId = userId;
    this._orderList = [];
  }

  addOrder(order) {
    if (!order) {
      throw new Error('Order data is required.');
    }
    this._orderList.push(order);
  }

  getSummaryMessage() {
    const totalOrders = this._orderList.length;
    return `User ${this.userId} has ${totalOrders} active orders.`;
  }
}

// 導出模組
export default OrderManager;
```
