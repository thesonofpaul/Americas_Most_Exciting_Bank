# Americas_Most_Exciting_Bank
Bank Ledger program. Consists of three separate applications: WebAPI, Console App, and MVC Web Application

Prerequisites:
 - Visual Studio 
 - .NET Framework (download link: https://www.microsoft.com/net/download/framework)
 - Windows
 
Setup:
1.  Clone this repository ("git clone https://github.com/thesonofpaul/Americas_Most_Exciting_Bank.git")
2.  cd into "Americas_Most_Exciting_Bank"
3.  cd into each of the 3 directories building the projects using the command "dotnet build" in each

To Run Web API (required to be running to successfully run console or web applications):
1.  cd into "BankLedger" directory
2.  run command "./BankLedger/bin/Debug/BankLedger.exe"

To Run Console Application:
1.  cd into "BankLedgerClient" directory
2.  run command "./BankLedgerClient/bin/Debug/BankLedgerClient.exe"
3.  prompt should appear in console window to "Enter command: "
4.  for help running console commands, type "help" and hit enter

To Run Web Application (Not hosted on server):
1.  open Visual Studio
2.  open "BankLedgerMVC/BankLedgerMVC.sln"
3.  run project
4.  browser window should open automatically. if not, open browser window and enter "http://localhost:50368/"
5.  Home/Login window should open first with tabs for the other pages.
