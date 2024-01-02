# Prerequisites

The trading api required couple of prerequisites or dependencies to kick off. Below are the list of prerequisites:

- .NET 8 for Wep Api
- Visual Studio 2022 or Above
- Postman (Optional) for Api testing
  

# Trading Api

This solution contains the serveral project to perform the various functionality and below the core api, functions app and the email notification service. 

1. EmailContractNote
2. TradingPOC.ContractNote
3. TradingPOC.Data.API
4. TradingPOC.EmailService
5. TradingPOC.LoginAPI
6. TradingPOC.TradingAPI

## EmailContractNote (Function App)

This function app will responsible to send the contract note information with the data into specific tabular format. Tabular information follow the below details:

- Order Id
- Trade Timestamp
- Script
- Trade Type
- Quantity
- Price
- Buy Amount
- Sell Amount

`ContractNote.cs` class contains the function app functionality and handle the request for generating the `TradeInfo` and passes the information to the `SendEmail` function to trigger the email to the user.

## TradingPOC.ContractNote

This function app is tried generate the function app as POC and it's contains the similar functionality as `EmailContractNote (Function App)`.

## TradingPOC.Data.API

This purpose of this project to define the core data api for e.g. `LivePricing`. Currently this is in development and their will more api and functionality to be included. Below endpoint will be used for various purposes.

- `api/Data/GetPrice`: This api will fetch the live price data.

## TradingPOC.EmailService

This service is responsible to perform all the email related tasks, notification etc. `RequestService` class defines the functionality of retrieving the mail template and construct the body and trigger the mail to the recipients. `IRequestService` interface exposes the below members.

`IRequestService`:
```
bool HasError { get; }
string ErrorDescription { get; }
void ProcessNotificationEmailRequest(RequestModel? request);
```

## TradingPOC.LoginAPI

This project mainly responsible to handle the login process either using different OAuth api like Facebook, Google and User Account. Respective api defined to fullfil the purpose of each. Below core controller defined as below:

- AccountController
- FacebookAccountController
- GoogleAccountController

`LoginRequestModel` and `LoginResponseModel` classes to hold the data of request/response. 


## TradingPOC.TradingAPI

This is the core project which is responsible to handle all the trading related api and various functionality or api defined in the project. As of now, two core controllers defined `Report` and `Trading`. 

All reporting api will be defined inside `ReportController`. Below are the endpoint mentioned below:

- `/api/report/ProfitAndLoss/{userId}/{from}/{to}`: This will fetch the Profit and Loss report data based on set off parameter `from` data and `to` data for the logged in user.
