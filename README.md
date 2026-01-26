
[![Build & Test](https://github.com/elminalirzayev/Easy.Tools.DateTimeHelper/actions/workflows/build.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Tools.DateTimeHelper/actions/workflows/build.yml)
[![Build & Release](https://github.com/elminalirzayev/Easy.Tools.DateTimeHelper/actions/workflows/release.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Tools.DateTimeHelper/actions/workflows/release.yml)
[![Build & Nuget Publish](https://github.com/elminalirzayev/Easy.Tools.DateTimeHelper/actions/workflows/nuget.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Tools.DateTimeHelper/actions/workflows/nuget.yml)
[![Release](https://img.shields.io/github/v/release/elminalirzayev/Easy.Tools.DateTimeHelper)](https://github.com/elminalirzayev/Easy.Tools.DateTimeHelper/releases)
[![License](https://img.shields.io/github/license/elminalirzayev/Easy.Tools.DateTimeHelper)](https://github.com/elminalirzayev/Easy.Tools.DateTimeHelper/blob/master/LICENSE.txt)
[![NuGet](https://img.shields.io/nuget/v/Easy.Tools.DateTimeHelper.svg)](https://www.nuget.org/packages/Easy.Tools.DateTimeHelper)


# Easy.Tools.DateTimeHelper

**Easy.Tools.DateTimeHelper** is a lightweight, high-performance .NET library that extends the `DateTime` struct. It simplifies complex date operations such as age calculation, business day arithmetic, Unix timestamp conversion, and ISO 8601 week calculations.

## Features

- **Advanced Age Calculation:** Get age in years or detailed (Years, Months, Days).
- **Business Days:** Add/Subtract workdays (automatically skips weekends).
- **Time Travel:** Easily get Start/End of Day, Week, or Month.
- **ISO 8601 Support:** Correctly calculates `WeeksInYear` (52 or 53 weeks).
- **Unix Integration:** Convert to/from Unix Timestamps (Seconds).
- **Time Zones:** Convert dates between specific Time Zones easily.
- **Fluent API:** Check for `IsWeekend`, `IsLeapYear`, `IsBetween`, etc.

## Installation

Install via NuGet Package Manager:

```bash
Install-Package Easy.Tools.DateTimeHelper
```

Or via .NET CLI:

```bash
dotnet add package Easy.Tools.DateTimeHelper
```

## Usage Example


### 1. Age Calculation

Calculates age correctly, accounting for leap years.

```csharp
using Easy.Tools.DateTimeHelper.Extensions;

var birthDate = new DateTime(1990, 5, 20);

// Simple Age
int age = birthDate.CalculateAge(); // Output: 34

// Detailed Age
var (years, months, days) = birthDate.CalculateAgeDetailed();
Console.WriteLine($"You are {years} years, {months} months, and {days} days old.");
```

### 2. Business Days (Workdays)

Skip weekends when adding days.

```csharp
var friday = new DateTime(2023, 10, 6); // Friday

// Add 3 business days (Skips Sat/Sun) -> Target is Wednesday
var nextWednesday = friday.AddBusinessDays(3);
```

### 3. Start & End of Periods

Useful for reporting and database queries.

```csharp
var now = DateTime.Now;

var startOfDay   = now.StartOfDay();   // 2023-10-06 00:00:00
var endOfDay     = now.EndOfDay();     // 2023-10-06 23:59:59.999

var startOfWeek  = now.StartOfWeek();  // Monday of the current week
var endOfWeek    = now.EndOfWeek();    // Sunday of the current week

var startOfMonth = now.StartOfMonth(); // 1st day of month
var endOfMonth   = now.EndOfMonth();   // Last day of month
```

### 4. Unix & Time Zones

```csharp
// Unix Timestamp
long timestamp = DateTime.UtcNow.ToUnixTimestamp();
DateTime myDate = DateTimeExtensions.FromUnixTimestamp(timestamp);

// Time Zone Conversion
var utcDate = DateTime.UtcNow;
var estDate = utcDate.ConvertToTimeZone("Eastern Standard Time");
```

### 5. ISO 8601 Weeks

Correctly handles years that have 53 weeks.

```csharp
var date = new DateTime(2020, 12, 31);
int weeksInYear = date.WeeksInYear(); // Returns 53 for ISO 8601 compliance
```

### 6. Helper Predicates

Correctly handles years that have 53 weeks.

```csharp
if (DateTime.Now.IsWeekend())
{
    Console.WriteLine("It's party time! ");
}

if (myDate.IsBetween(startDate, endDate))
{
    // Do something...
}

var nextFriday = DateTime.Now.NextWeekday(DayOfWeek.Friday);
```

---

## Contributing

Contributions and suggestions are welcome. Please open an issue or submit a pull request.

---

## License

This project is licensed under the MIT License.

---

© 2025 Elmin Alirzayev / Easy Code Tools