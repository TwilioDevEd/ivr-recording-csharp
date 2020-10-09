<a href="https://www.twilio.com">
  <img src="https://static0.twilio.com/marketing/bundles/marketing/img/logos/wordmark-red.svg" alt="Twilio" width="250" />
</a>
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

3. Modify C:\Users\Sam[...] path in `Web.config` to a valid path for `<add name="DefaultConnection" connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=YOUR_PATH_HERE` the file will be created in the next step.

4. Run `Update-Database` at [Package Manager
   Console](https://docs.nuget.org/consume/package-manager-console) to execute the migrations.

5. Expose the application to the wider Internet using [ngrok](https://ngrok.com/)

   ```
   ngrok http 1078 -host-header="localhost:1078"
   ```

6. Provision a number under the
   [Manage Numbers page](https://www.twilio.com/user/account/phone-numbers/incoming)
   on your account. Set the voice URL for the number to
   `http://<your-ngrok-subdomain>.ngrok.io/ivr/welcome`.

That's it!

## Meta

* No warranty expressed or implied. Software is as is. Diggity.
* [MIT License](http://www.opensource.org/licenses/mit-license.html)
* Lovingly crafted by Twilio Developer Education.
