# IVR Call Recording and Agent Conference. Level: Intermediate. Powered by Twilio - ASP.NET MVC

An example application implementing an automated phone line using
Twilio and ASP.NET MVC.

[Read the full tutorial here](https://www.twilio.com/docs/tutorials/walkthrough/ivr-screening/csharp/mvc)!

[![Build status](https://ci.appveyor.com/api/projects/status/w3s6prc1gqfmngaw?svg=true)](https://ci.appveyor.com/project/TwilioDevEd/ivr-recording-csharp)

## Run the application

1. Clone the repository and `cd` into it.
   ```
   git clone git@github.com:TwilioDevEd/ivr-recording-csharp.git
   cd ivr-recording-csharp
   ```

2. Build the solution.

3. Run `Update-Database` at [Package Manager
   Console](https://docs.nuget.org/consume/package-manager-console) to execute the migrations.

4. Expose the application to the wider Internet using [ngrok](https://ngrok.com/)

   ```
   ngrok http 1078 -host-header="localhost:1078"
   ```

5. Provision a number under the
   [Manage Numbers page](https://www.twilio.com/user/account/phone-numbers/incoming)
   on your account. Set the voice URL for the number to
   `http://<your-ngrok-subdomain>.ngrok.io/ivr/welcome`.

That's it!

## Meta

* No warranty expressed or implied. Software is as is. Diggity.
* [MIT License](http://www.opensource.org/licenses/mit-license.html)
* Lovingly crafted by Twilio Developer Education.
