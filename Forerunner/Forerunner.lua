function OnCommandOutput(gruntName, command, commandOutput)
	print('New Task Output from: ' .. gruntName)
	print('Command: ' .. command)
	print('Output: ' .. commandOutput)
end

function OnGruntInitial(n,a,u)
	print ('[LUA] Getting Grunts')
	GetGrunts()
	print ('[LUA] New Grunt Received: ' .. n)
	print ('[LUA] Sending Command ProcessList command')
	print (Exec(n,'SeatBelt system'))
end