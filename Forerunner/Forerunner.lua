function OnGruntInitial()
	--Forerunner passes the Grunt object to the script and can be accessed like so:
	print ('New Grunt Received with Name: ' .. gruntName)
	
	--Checking Values
	print("[GruntInitial] Grunt Values")
	print('gruntID: ' .. gruntID)
	print('gruntGUID: ' .. gruntGUID)
	print('gruntListener: ' .. gruntListener)
	print('gruntHostname: ' .. gruntHostname)
	print('gruntIntegrity: ' .. gruntIntegrity)
	print('gruntIP: ' .. gruntIP)
	print('gruntOS: ' .. gruntOS)
	print('gruntProcess: ' .. gruntProcess)
	print('gruntDomain: ' .. gruntDomain)
	print('gruntUser: ' .. gruntUser)
	print('gruntLastCheckIn: ' .. gruntLastCheckIn)

	print('[GruntInitial] Grunt Task')
	print('taskID' .. taskID)
	print('taskName' .. taskName)
	print('taskOutput' .. taskOutput)

	print(GruntExec(gruntName,"ls C:\\"))

end

function OnGlobalOutput(commandOutput)
	print(commandOutput)
	print("[GlobalOutput] Grunt Values")
	print('gruntID: ' .. gruntID)
	print('gruntGUID: ' .. gruntGUID)
	print('gruntListener: ' .. gruntListener)
	print('gruntHostname: ' .. gruntHostname)
	print('gruntIntegrity: ' .. gruntIntegrity)
	print('gruntIP: ' .. gruntIP)
	print('gruntOS: ' .. gruntOS)
	print('gruntProcess: ' .. gruntProcess)
	print('gruntDomain: ' .. gruntDomain)
	print('gruntUser: ' .. gruntUser)
	print('gruntLastCheckIn: ' .. gruntLastCheckIn)

	print('[GlobalOutput] Grunt Task')
	print('taskID' .. taskID)
	print('taskName' .. taskName)
	print('taskOutput' .. taskOutput)
end
