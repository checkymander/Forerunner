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
	    
5. SendMatterMostNotification
	- Sends a mattermost notification to your specified channel.
	- Params
		- Mattermost Host
		- Message
		- AccessToken
		- Channel
	- Example
      - `SendMattermostNotification("mattermost.company.corp", "This is a test","mattersdfdsmost/tokenwers","general")`
      
## Script Variables
forerunner.lua supports the following variables to access information about the grunt
1. gruntName - the name of the grunt
2. gruntID - the ID oo the grunt
3. gruntGUID - the GUID of the grunt
4. gruntHostname - the hostname of the grunt host
5. gruntIntegrity - the integrity level the grunt is running in (Low/Medium/High)
6. gruntIP - the IP address of the grunt host
7. gruntOS - the OS the grunt is running on
8. gruntProcess - the process the grunt is running as
9. gruntDomain - the domain the grunt is connected to
10. gruntUser - the username the grunt returned as
11. gruntLastCheckIn - last checkin time of the grunt

The following variables are used to gain access to information about the task returned.(Note: these will only be set once, so if you run a new command with GruntExec() these variables will not represent the most recent call)
1. taskID - the ID of the task
2. taskName - the name of the task executed by the grunt
3. taskOutput - the output of the task returned


## Event Hooks
1. OnGruntInitial
	- Code is run when a new grunt is initialized
	
2. OnGlobalOutput
	- Code runs anytime output is returned from any grunt
