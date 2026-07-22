---
name: create-xunit-test
description: '使用 dotnet cli 在 .NET 9 (或以上) 環境中建立 xUnit 測試專案，並撰寫遵循 AAA 模式的 C# 單元測試。'
argument-hint: '請提供要建立的測試專案名稱、目標專案路徑，以及要測試的類別或方法。'
user-invocable: true
---
# Create xUnit Test Skill

本 Skill 專門用於在 .NET 9+ 環境下，透過 `dotnet cli` 建立全新的 xUnit 測試專案，與目標專案（待測專案，System Under Test - SUT）進行關聯，並撰寫符合 **AAA (Arrange, Act, Assert)** 慣例的單元測試程式碼。同時確保符合工作區的 [csharp.instructions.md](../../instructions/csharp.instructions.md) 規範。

## Triggers
- 用戶要求「為某個類別/方法建立單元測試」、「新增 C# 測試專案」、「使用 xUnit 撰寫 AAA 測試」。
- 需要針對現有 C# 專案實作測試結構。

## Procedure

### 1. 建立測試專案與設定關聯
使用 `dotnet cli` 執行下列步驟以建立 .NET 9 的 xUnit 專案：
1. **建立專案**：在合適的目錄下執行以下指令（預設指定為 .NET 9 架構）：
   ```powershell
   dotnet new xunit -n [TestProjectName] -f net9.0
   ```
2. **加入專案參照 (Reference)**：將待測專案 (SUT) 的專案參照加入至測試專案中：
   ```powershell
   dotnet add [TestProjectName]/[TestProjectName].csproj reference [TargetProject]/[TargetProject].csproj
   ```
3. **加入方案 (Solution)**：如果工作區根目錄存在 `.sln` 檔案，請將新專案加入方案：
   ```powershell
   dotnet sln [SolutionName].sln add [TestProjectName]/[TestProjectName].csproj
   ```

### 2. 撰寫 AAA 單元測試程式碼
在新建的測試專案中建立測試類別（例如 `[ClassName]Tests.cs`），並遵循 [csharp.instructions.md](../../instructions/csharp.instructions.md) 規範與 AAA 結構：

- **類別命名**：`[ClassName]Tests`
- **測試方法命名**：採取 `MethodUnderTest_Scenario_ExpectedBehavior` 格式，例如 `CalculateTotal_ValidItems_ReturnsCorrectSum`。
- **AAA 結構區塊**：在測試方法中明確標示 `Arrange`、`Act`、`Assert` 註解：
  - **Arrange (準備)**：初始化物件、建立 Mock/Stub、設定輸入參數，建構出測試的前置環境。
  - **Act (執行)**：呼叫要測試的目標方法並獲取返回值或觸發行為。
  - **Assert (驗證)**：使用 `Assert.Equal`、`Assert.NotNull` 等方法驗證執行結果與預期是否相符。

### 3. C# 程式碼品質與樣式檢查
- 測試類別或檔案應使用 **File-scoped Namespaces**（例如 `namespace MyProject.Tests;`）。
- 括號應採用 **Allman 風格**（大括號單獨占一行）。
- 變數宣告在右側可明確看出類型時，應使用 `var`。

## Code Template

```csharp
using Xunit;
using MyProject.Services; // 待測 Namespace

namespace MyProject.Tests;

public class OrderProcessorTests
{
    [Fact]
    public async Task ProcessOrderAsync_ValidOrder_ReturnsSuccessResult()
    {
        // Arrange
        var mockPaymentService = new MockPaymentService(true);
        var processor = new OrderProcessor(mockPaymentService);
        var order = new Order { Id = 1, Amount = 100 };

        // Act
        var result = await processor.ProcessOrderAsync(order);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ProcessResult.Success, result);
    }
}
```

## Completion Verification
1. **編譯驗證**：在終端機中執行 `dotnet build` 確保專案不含編譯錯誤。
2. **執行測試**：執行 `dotnet test` 確保所有測試案例皆成功通過。
3. **回報結構**：告知使用者建立了哪些檔案、加入了哪些關聯，並提供測試執行結果的摘要。
