$exePath = Resolve-Path ".\WindowedStoryPlanner\bin\Debug\net10.0-windows\WindowedStoryPlanner.exe"

$classesRoot = "HKCU:\Software\Classes"

New-Item -Force -Path "$classesRoot\.storyplan" | Set-ItemProperty -Name "(default)" -Value "StoryPlannerFile"
New-Item -Force -Path "$classesRoot\StoryPlannerFile" | Set-ItemProperty -Name "(default)" -Value "Story Planner Project"
New-Item -Force -Path "$classesRoot\StoryPlannerFile\DefaultIcon" | Set-ItemProperty -Name "(default)" -Value "`"$exePath`",0"
New-Item -Force -Path "$classesRoot\StoryPlannerFile\shell\open\command" | Set-ItemProperty -Name "(default)" -Value "`"$exePath`" `"%1`""

# Tell Explorer to pick up the change immediately
& ie4uinit.exe -show
Write-Host "Registered .storyplan -> $exePath"