[![build-and-test](https://github.com/abhiroop43/Expense-Tracker-Backend-API/actions/workflows/build+test.yml/badge.svg)](https://github.com/abhiroop43/Expense-Tracker-Backend-API/actions/workflows/build+test.yml)
[![deployment](https://github.com/abhiroop43/Expense-Tracker-Backend-API/actions/workflows/deploy.yml/badge.svg)](https://github.com/abhiroop43/Expense-Tracker-Backend-API/actions/workflows/deploy.yml)

# Expense Tracker Backend API 🙄💸

Welcome to the **ExpenseTracker** backend API – because who _doesn't_ want to track every penny they spend?
Built with .NET (because, obviously), using Clean Architecture, DDD, and CQRS – because we like our code as complicated
as our finances.
MongoDB is our database of choice, because SQL is just too mainstream. 😏

## Live Swagger (because you want to see if it actually works)

[Check out the API docs in all their glory](https://abhiroop43-expensetracker-api-eea4gshuauawc0hq.uaenorth-01.azurewebsites.net/swagger/index.html) – because nothing says "production ready" like a public Swagger page.

## Features

- 🚀 Blazing fast API (if you don't count cold starts)
- 🧼 Clean Architecture (so clean, you’ll need sunglasses)
- 🧩 DDD & CQRS (because acronyms make everything better)
- 🍃 MongoDB (NoSQL, no problem, no relations, no worries)
- 🛡️ Enterprise-level overengineering

## Getting Started

1. Clone this repo. Or don’t. I’m not your boss.
2. Install .NET. If you don’t have it, what are you even doing here?
3. Make sure MongoDB is running. Or just hope for the best.
4. Manually create these collections in MongoDB, because EF Core migrations are apparently too cool for NoSQL (for now):
   - lookups
   - wallets
   - transactions
   - applicationUsers
   - applicationRoles
5. Import the data for `lookups`, `applicationRoles`, and `applicationUsers` from the `seed` directory. Yes, by hand. Automation is overrated anyway.
6. Restore the dependencies:

   ```bash
   dotnet restore
   ```

7. Run the API:

   ```bash
   dotnet run
   ```

8. Watch as your expenses are tracked with the power of modern software architecture. Or at least, pretend they are.

## Why Clean Architecture?

Because messy architecture is so last season.
A tangled up codebase that nobody understands. 🍝

## Why DDD & CQRS?

Because we like to separate our concerns. And our code. And our hopes and dreams.

## Why MongoDB?

Because sometimes you just want to store JSON and call it a day.

## Contributing

Feel free to open a PR. Or don’t. We’ll survive either way.

## License

MIT – because sharing is caring. Or something like that.
