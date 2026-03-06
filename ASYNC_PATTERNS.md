# Async Patterns Guide

This guide demonstrates the comprehensive async support in StrongResult, including chaining async operations and working with `Task<Result<T>>` and `ValueTask<Result<T>>`.

## Overview

StrongResult provides full async support for all extension methods with multiple overload patterns:
- **Sync result → Async function**: Execute async logic on sync results
- **Async result → Sync function**: Apply sync transformations to async results
- **Async result → Async function**: Full async pipeline
- **Task/ValueTask support**: Works with both `Task` and `ValueTask`

## MapAsync Patterns

### Sync Result → Async Function
```csharp
var result = Result<int>.Ok(5);
var mapped = await result.MapAsync(async x => await GetMultiplierAsync(x));
```

### Async Result → Sync Function
```csharp
Task<Result<int>> resultTask = GetValueAsync();
var mapped = await resultTask.MapAsync(x => x * 2);
```

### Async Result → Async Function
```csharp
Task<Result<int>> resultTask = GetValueAsync();
var mapped = await resultTask.MapAsync(async x => await CalculateAsync(x));
```

### Full Example with Error Handling
```csharp
var result = await GetUserIdAsync()
    .MapAsync(id => $"User_{id}")
    .MapAsync(async name => await FetchUserDataAsync(name));

if (result.IsSuccess)
{
    Console.WriteLine($"User: {result.Value}");
}
```

## BindAsync Patterns

### Sync Result → Async Bind
```csharp
var result = Result<string>.Ok("123");
var bound = await result.BindAsync(async id => await ValidateUserAsync(id));
```

### Async Result → Sync Bind
```csharp
ValueTask<Result<int>> resultTask = ParseInputAsync();
var bound = await resultTask.BindAsync(x => 
    x > 0 ? Result<string>.Ok("positive") : Result<string>.Fail(Error.Create("E", "negative")));
```

### Async Result → Async Bind
```csharp
Task<Result<string>> resultTask = GetTokenAsync();
var bound = await resultTask.BindAsync(async token => await AuthenticateAsync(token));
```

### Complex Chaining
```csharp
var result = await GetUserInputAsync()
    .BindAsync(async input => await ValidateAsync(input))
    .BindAsync(async validated => await ProcessAsync(validated))
    .BindAsync(processed => Result<string>.Ok(processed.ToString()));
```

## MatchAsync Patterns

### Sync Result → Async Match
```csharp
var result = Result<int>.Ok(42);
var output = await result.MatchAsync(
    async value => await FormatSuccessAsync(value),
    async error => await LogErrorAsync(error));
```

### Async Result → Sync Match
```csharp
var resultTask = ComputeAsync();
var output = await resultTask.MatchAsync(
    value => $"Success: {value}",
    error => $"Error: {error.Message}");
```

### Async Result → Async Match
```csharp
var resultTask = FetchDataAsync();
var output = await resultTask.MatchAsync(
    async data => await ProcessSuccessAsync(data),
    async error => await HandleErrorAsync(error));
```

## OnSuccessAsync / OnFailureAsync Patterns

### Async Side Effects on Sync Results
```csharp
var result = Result<string>.Ok("data");
await result
    .OnSuccessAsync(async value => await SaveToDbAsync(value))
    .OnFailureAsync(async error => await LogErrorAsync(error));
```

### Sync Side Effects on Async Results
```csharp
var resultTask = LoadDataAsync();
await resultTask
    .OnSuccessAsync(data => Console.WriteLine($"Loaded: {data}"))
    .OnFailureAsync(error => Console.WriteLine($"Failed: {error.Message}"));
```

### Full Async Pipeline
```csharp
await FetchUserAsync()
    .OnSuccessAsync(async user => await SendWelcomeEmailAsync(user))
    .OnFailureAsync(async error => await NotifyAdminAsync(error));
```

## OnWarningsAsync / ForEachWarningAsync Patterns

### Processing Warnings Asynchronously
```csharp
var warning1 = Warning.Create("W1", "Low disk space");
var warning2 = Warning.Create("W2", "High memory usage");
var result = Result<string>.PartialSuccess("data", warning1, warning2);

await result.ForEachWarningAsync(async warning =>
{
    await LogWarningToMonitoringAsync(warning);
});
```

### Async Result with Warning Processing
```csharp
await ProcessDataAsync()
    .OnWarningsAsync(async warnings =>
    {
        foreach (var w in warnings)
            await SendAlertAsync(w);
    });
```

## Complex Real-World Examples

### Example 1: User Registration Flow
```csharp
public async Task<Result<UserDto>> RegisterUserAsync(RegisterRequest request)
{
    return await ValidateRequestAsync(request)
        .BindAsync(async req => await CheckUserExistsAsync(req.Email))
        .BindAsync(async email => await CreateUserAsync(email, request))
        .MapAsync(async user => await GenerateUserDtoAsync(user))
        .OnSuccessAsync(async dto => await SendConfirmationEmailAsync(dto.Email))
        .OnFailureAsync(async error => await LogRegistrationErrorAsync(error))
        .ForEachWarningAsync(async warning => await LogWarningAsync(warning));
}
```

### Example 2: Data Pipeline
```csharp
public async Task<Result<ProcessedData>> ProcessDataPipelineAsync(string input)
{
    var result = await ParseInputAsync(input)
        .MapAsync(async parsed => await EnrichDataAsync(parsed))
        .BindAsync(async enriched => await ValidateBusinessRulesAsync(enriched))
        .MapAsync(async validated => await TransformAsync(validated))
        .OnWarningsAsync(warnings => 
            Console.WriteLine($"Pipeline warnings: {warnings.Count}"));

    return await result.MatchAsync(
        async data => Result<ProcessedData>.Ok(data),
        async error => await HandlePipelineErrorAsync(error));
}
```

### Example 3: Parallel Operations with Results
```csharp
public async Task<Result<Summary>> ProcessMultipleAsync(List<string> inputs)
{
    var tasks = inputs.Select(input => ProcessSingleAsync(input)).ToList();
    var results = await Task.WhenAll(tasks);

    // Combine results
    var errors = results.Where(r => r.IsFailure).Select(r => r.Error!).ToList();
    if (errors.Any())
    {
        return Result<Summary>.Fail(Error.Create("BATCH_ERROR", $"{errors.Count} items failed"));
    }

    var values = results.Select(r => r.Value!).ToList();
    return Result<Summary>.Ok(new Summary(values));
}

private async Task<Result<ProcessedItem>> ProcessSingleAsync(string input)
{
    return await ValidateAsync(input)
        .BindAsync(async valid => await TransformAsync(valid))
        .OnFailureAsync(async error => await LogAsync(error));
}
```

## Best Practices

### 1. Use ConfigureAwait(false)
All built-in async methods use `ConfigureAwait(false)` for better performance in library code.

### 2. Choose ValueTask for Performance
StrongResult uses `ValueTask` for better allocation performance when results are often synchronously completed.

### 3. Chain Operations Fluently
```csharp
var result = await GetDataAsync()
    .MapAsync(Transform)
    .BindAsync(ValidateAsync)
    .OnSuccessAsync(ProcessAsync);
```

### 4. Error Handling in Async Pipelines
```csharp
try
{
    var result = await RiskyOperationAsync()
        .OnFailureAsync(async error => await LogAsync(error));
    
    return result.IsSuccess ? result.Value : DefaultValue;
}
catch (Exception ex)
{
    return Result<T>.Fail(Error.FromException(ex));
}
```

### 5. Warning Collection in Async Flows
```csharp
var warnings = new List<IWarning>();

var result = await ProcessAsync()
    .ForEachWarningAsync(async w =>
    {
        warnings.Add(w);
        await NotifyAsync(w);
    });
```

## Performance Considerations

- **ValueTask**: Preferred for sync-completed results (reduces allocations)
- **Task**: Use when result is always asynchronous
- **ConfigureAwait(false)**: Already applied internally for library code
- **Avoid unnecessary awaits**: The library optimizes by checking `IsFailure` before executing functions

## Migration from Sync to Async

### Before (Sync)
```csharp
var result = GetData()
    .Map(x => Transform(x))
    .Bind(x => Validate(x))
    .OnSuccess(x => Process(x));
```

### After (Async)
```csharp
var result = await GetDataAsync()
    .MapAsync(async x => await TransformAsync(x))
    .BindAsync(async x => await ValidateAsync(x))
    .OnSuccessAsync(async x => await ProcessAsync(x));
```

## Testing Async Code

```csharp
[Fact]
public async Task MyAsyncTest()
{
    // Arrange
    var resultTask = GetResultAsync();

    // Act
    var output = await resultTask
        .MapAsync(x => x * 2)
        .OnSuccessAsync(x => Assert.True(x > 0));

    // Assert
    Assert.True(output.IsSuccess);
}
```

---

For more information, see the test files:
- `ResultT.AsyncPatternsTests.cs` - Generic async patterns
- `Result.AsyncPatternsTests.cs` - Non-generic async patterns
