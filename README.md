# StrongResult

StrongResult is a C# library for representing operation results with explicit success, failure, error, and warning states. It provides both generic (`Result<T>`) and non-generic (`Result`) result types for clear, type-safe, and expressive error handling.

## Features

- **Result Types**: Use `Result` for operations without a return value, and `Result<T>` for operations that return a value.
- **Success & Failure**: Distinguish between hard success, partial success (with warnings), controlled errors, and hard failures.
- **Warnings & Errors**: Attach warnings and errors to results for detailed diagnostics.
- **Immutability**: All result types are immutable and thread-safe.
- **Fluent API**: Easily create, map, bind, and match results.

## Quick Start

### Installation

Add StrongResult to your project via NuGet:

```
dotnet add package StrongResult
```

### Usage

#### Non-Generic Result

```
using StrongResult.NonGeneric;

// Hard success
var result = Result.Ok();

// Partial success with warnings
var warning = Warning.Create("W1", "Minor issue");
var partial = Result.PartialSuccess(warning);

// Controlled error
var error = Error.Create("E1", "Validation failed");
var controlled = Result.ControlledError(error, warning);

// Hard failure
var failure = Result.Fail(error);
```

#### Generic Result

```
using StrongResult.Generic;

// Hard success with value
var result = Result<string>.Ok("value");

// Partial success with value and warnings
var partial = Result<string>.PartialSuccess("value", warning);

// Controlled error with value type
var controlled = Result<string>.ControlledError(error, warning);

// Hard failure
var failure = Result<string>.Fail(error);
```

### Handling Results

```
if (result.IsSuccess)
{
    // Access result.Value for generic results
}
else
{
    // Handle result.Error and result.Warnings
}
```

#### Implicit Conversion

```
string value = Result<string>.Ok("abc"); // Implicit conversion
```

#### Mapping and Binding

```
var mapped = result.Map(s => s.Length);
var bound = result.Bind(s => Result<int>.Ok(s.Length));
```

### Fluent Result Actions

#### OnSuccess

```
result.OnSuccess(() => Console.WriteLine("Success!"));
```

#### OnFailure

```
result.OnFailure(error => Console.WriteLine($"Failed: {error.Message}"));
```

#### OnWarnings

```
result.OnWarnings(warnings =>
{
    foreach (var w in warnings)
        Console.WriteLine($"Warning: {w.Message}");
});
```

#### ForEachWarning

```
result.ForEachWarning(warning => Console.WriteLine($"Warning: {warning.Code} - {warning.Message}"));
```

## Result Kinds

- `HardSuccess`: Operation completed successfully.
- `PartialSuccess`: Operation succeeded with warnings.
- `ControlledError`: Operation failed with expected/controlled error.
- `HardFailure`: Operation failed with unrecoverable error.

## API Reference

- `Result` and `Result<T>`: Main result types.
- `Error` and `Warning`: Attach error and warning information.
- Extension methods: `Map`, `Bind`, `Match`, `OnSuccess`, `OnFailure`,`OnWarnings`, `ForEachWarning` etc.

## Testing

Unit tests are provided for all core scenarios. See the `StrongResult.Test` project for examples.

## Contributing

Contributions and feedback are welcome! Please open issues or submit pull requests on [GitHub](https://github.com/saulhdzga/StrongResult).

---

**.NET 8 / C# 12 compatible**