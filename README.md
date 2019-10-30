# Forerunner
An automation engine using the Covenant API and lua scripting

## Set-Up
Step 1.) Install .NET Core from [here](https://dotnet.microsoft.com/download/dotnet-core/2.2)

Step 2.) Download the most recent version from [releases](https://github.com/checkymander/Forerunner/releases)

Step 3.) Modify the forerunner.lua file as needed and it in the same directory as the forerunner dll

## Usage

For basic execution run the following command 

```dotnet Forerunner.dll https://localhost:7443```

The console will then ask for your covenant credentials, enter the credentials you want to use to connect to the Covenant server

```[+] Testing connection to Covenant Server.
[+] Connection Established!
[!] No Access Token Provided. Getting Token.
[!] Enter your Username: checkymander
[!] Enter your password: ********
```

Assuming you've entered the correct credentials you should see the following:

```[+] Connecting to EventHub
[+] Connected to EventHub
[+] Joining Group
[+] Joined EventHub group with ID:NJDNTGErfEVuZ8LIzgNsBQ
[+] Connecting to GruntHub
[+] Connected to GruntHub
[+] Joining Group
[+] Joined GruntHub group with ID:Ffv5_-fUfJXIYpbXEUCSCQ
```

From here, you should be good to go. Forerunner will listen for new grunts, and perform the requested functions when the specific events occur.

Alternatively you can provide the AccessToken directly if you don't feel comfortable providing a password

```forerunner.exe https://localhost:7443 <covenantToken>```

## Supported Functionality
The following commands are built-in functions supported by Forerunner. More will be added as I find use, and receive requests.

1. SendSlackNotification
	- Sends a slack notification to your specified channel.
	- Params
		- Message
		- AccessToken
		- Channel
	- Example
      - `SendSlackNotification("This is a test","slacksdfds/tokenwers","#general")`

2. WriteToLog
	- Writes output to a log file in the same directory as Forerunner.exe
	- Params
		- Message
	- Example
      - `WriteToLog("Test")`
	
3. GruntExec
	- Tells a grunt to run a task and return the results
	- Params
		- GruntName
		- Command
    - Example
	    - `GruntExec(gruntName,"ls C:\\")`
    - Returns
      - The result of the command.
		
4. LaunchJob
	- Tasks grunt to run a task, doesn't return output
	- Params
		- GruntName
		- Command
    - Example
	    - `LaunchJob(gruntName,"ls")`
      
## Script Variables
forerunner.lua supports placeholder for certain specific variables
1. gruntName
2. gruntID
3. gruntGUID
4. gruntListener

## Event Hooks
1. OnGruntInitial
	- Code is run when a new grunt is initialized
	
2. OnGlobalOutput
	- Code runs anytime output is returned from any grunt
