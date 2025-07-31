
# Easy.Tools.DateTimeHelpers

## Overview
`Easy.Tools.DateTimeHelpers` provides a set of extension methods for common and advanced `DateTime` operations in .NET. It simplifies working with dates by offering methods for age calculation, Unix timestamp conversions, date range checks, start/end of day/week/month retrieval, business day arithmetic, and more.

## Features
- Calculate exact age with years, months, and days
- Calculate age in years with leap year (Feb 29) support
- Convert `DateTime` to/from Unix timestamp
- Check if a date is weekend or weekday
- Get start and end of day, week, and month
- Calculate total weeks between two dates
- Add or subtract business days
- Determine if a year is leap year
- Compare dates ignoring time
- Convert `DateTime` to specific time zones
- Days until a target date

## Installation
Install via NuGet Package Manager:

```bash
Install-Package Easy.Tools.DateTimeHelpers
```

Or via .NET CLI:

```bash
dotnet add package Easy.Tools.DateTimeHelpers
```

## Usage Example

```csharp
using Easy.Tools.DateTimeHelpers.Extensions;

var birthDate = new DateTime(1990, 5, 15);
var age = birthDate.CalculateAge(); // returns age in years

var detailedAge = birthDate.CalculateAgeDetailed(); // returns (years, months, days)

var now = DateTime.Now;
var startOfWeek = now.StartOfWeek();
var endOfWeek = now.EndOfWeek();

var unixTimestamp = now.ToUnixTimestamp();
var dateFromUnix = DateTimeExtensions.FromUnixTimestamp(unixTimestamp);

bool isWeekend = now.IsWeekend();
bool isLeapYear = now.IsLeapYear();

var nextMonday = now.NextWeekday(DayOfWeek.Monday);
```

## Contributing
Contributions and suggestions are welcome. Please open an issue or submit a pull request.

## License
MIT License

---

© 2025 Elmin Alirzayev / Easy Code Tools
