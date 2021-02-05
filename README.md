<a href="https://www.twilio.com">
  <img src="https://static0.twilio.com/marketing/bundles/marketing/img/logos/wordmark-red.svg" alt="Twilio" width="250" />
</a>

# IVR Call Recording and Agent Conference. Level: Intermediate. Powered by Twilio - ASP.NET MVC

An example application implementing an automated phone line using
Twilio and ASP.NET MVC.

[Read the full tutorial here](https://www.twilio.com/docs/tutorials/walkthrough/ivr-screening/csharp/mvc)!

![](https://github.com/TwilioDevEd/ivr-recording-csharp/workflows/NetFx/badge.svg)

## Requirements

- [Visual Studio](https://visualstudio.microsoft.com/downloads/) 2019 or later.
- SQL Server Express 2019 with [LocalDB support](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)

## Local Development

1. Clone the repository and `cd` into it.
   ```
   git clone git@github.com:TwilioDevEd/ivr-recording-csharp.git
   cd ivr-recording-csharp
   ```

1. Build the solution.

1. Run `Update-Database` at [Package Manager Console](https://docs.nuget.org/consume/package-manager-console) to execute the migrations.
 
   *(Be sure to check that your database server name matches the one from the connection string on `Web.config`. For reference, default values where used upon SQLServer installation)*

1. Run the application.

1. Expose the application to the wider Internet using [ngrok](https://ngrok.com/).

   ```
   ngrok http 1078 -host-header="localhost:1078"
   ```

1. Provision a number under the
   [Manage Numbers page](https://www.twilio.com/user/account/phone-numbers/incoming)
   on your account. Set the voice URL for the number to
   `http://<your-ngrok-subdomain>.ngrok.io/ivr/welcome`.

That's it!

## Meta

* No warranty expressed or implied. Software is as is. Diggity.
* [MIT License](http://www.opensource.org/licenses/mit-license.html)
* Lovingly crafted by Twilio Developer Education.
