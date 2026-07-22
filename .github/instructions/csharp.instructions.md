---
name: "C# Coding Standards"
description: "Use when creating, editing, refactoring, or reviewing any C# source code (.cs) to ensure alignment with standard Microsoft C# coding conventions and best practices."
applyTo: "**/*.cs"
---
# C# 程式碼規範

此規範旨在定義 C# 的程式碼風格與撰寫慣例，以保充程式碼的可讀性、一致性與可維護性。

## 1. 命名規範 (Naming Conventions)

- **PascalCase (大駝峰命名法)**:
  - 適用於：`Class`, `Record`, `Struct`, `Interface`, `Enum` 類型與成員。
  - 適用於：`Method`, `Property`, `Event`。
  - 專案名稱、Namespace。
  - *範例*: `public class CustomerAccount`, `public void CalculateTotal()`
- **camelCase (小駝峰命名法)**:
  - 適用於：方法參數 (Method arguments)、區域變數 (Local variables)。
  - *範例*: `int itemCount`, `string customerName`
- **_camelCase (底線開頭小駝峰)**:
  - 適用於：私有欄位 (Private fields) 或唯讀欄位 (Read-only fields)。
  - *範例*: `private readonly ICustomerRepository _customerRepository;`
- **I 開頭 (I-Prefix) + PascalCase**:
  - 適用於：介面 (Interface)。
  - *範例*: `public interface IOrderService`
- **T 開頭 (T-Prefix)**:
  - 適用於：泛型參數 (Generic type parameters)。
  - *範例*: `public class List<TElement>`

## 2. 程式碼排版與結構 (Layout & Structure)

- **大括號 `{}`**:
  - 採用 **Allman 風格**（每個大括號單獨佔一行）。
  - 對於單行的 `if`、`foreach`、`for` 語句，**強烈建議**不省略大括號，以提升可維護性並防止 Bug。
- **縮排**:
  - 使用 4 個空格 (Spaces) 進行縮排，不使用 Tab。
- **Namespace**:
  - 推薦使用 C# 10 的**檔案範圍命名空間 (File-scoped namespaces)** 以減少縮排層級。
  - *範例*: `namespace MyProject.Services;`

## 3. C# 語言特性與最佳實踐 (Language Features & Best Practices)

- **隱式類型 `var`**:
  - 當宣告右側能夠明確看出類型時，使用 `var`。
    - *範例*: `var customers = new List<Customer>();`
  - 當類型不明確時，應明確聲明類型以提升可讀性。
    - *範例*: `int count = GetCount();`
- **Null 安全性**:
  - 啟用 **Nullable Reference Types (NRT)**，並妥善處理潛在的 Null 狀態（使用 `?`、`??` 或 `!` 運言符）。
  - 使用模式匹配來檢查 Null，例如 `if (obj is null)` 優先於 `if (obj == null)`。
- **異步編程 (Async/Await)**:
  - 異步方法名稱必須以 `Async` 結尾（除了事件處理常式或進入點方法）。
  - 異步呼叫應一律進行 `await`，不可忽略（除非明確打算進行背景執行不綁定生命週期）。
- **String 與 Exception**:
  - 字串格式化優先使用字串插值 (String interpolation)：`$"Hello, {name}"`。
  - 拋出異常時，請勿拋出泛用的 `Exception`，應實作或使用具體的異常類型（如 `ArgumentNullException` 等）。

## 4. 程式碼範例 (Code Example)

```csharp
using System;
using System.Threading.Tasks;

namespace MyDemo.Demo4.Services;

public interface IOrderProcessor
{
    Task<ProcessResult> ProcessOrderAsync(Order order);
}

public class OrderProcessor : IOrderProcessor
{
    private readonly IPaymentService _paymentService;

    public OrderProcessor(IPaymentService paymentService)
    {
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
    }

    public async Task<ProcessResult> ProcessOrderAsync(Order order)
    {
        if (order is null)
        {
            throw new ArgumentNullException(nameof(order));
        }

        var isPaymentSuccessful = await _paymentService.ChargeAsync(order.Amount);

        if (isPaymentSuccessful)
        {
            return ProcessResult.Success;
        }

        return ProcessResult.Failed;
    }
}
```
