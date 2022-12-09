$year = 2022
$maxDay = 0
Get-ChildItem . -directory -Filter Day* |
Foreach-Object {
	$match = Select-String "Day(\d+)" -inputobject $_.FullName
	$thisDay = [int]$match.Matches.groups[1].Value
	if ($thisDay -gt $maxDay) {
		$maxDay = $thisDay
	}
}
$nextDay = $maxDay + 1
$nextFolder = ".\Day${nextDay}" 
New-Item $nextFolder -Type Directory

$newDayFile = "${nextFolder}\Day${nextDay}.cs"
Copy-Item .\DayTemplate.cs  $newDayFile

((Get-Content -path $newDayFile -Raw) -replace 'Template', $nextDay) |
Set-Content -Path $newDayFile

New-Item "${nextFolder}\inputs" -Type Directory
$inputFile = "${nextFolder}\inputs\input.txt"

((Get-Content -path ".\Program.cs" -Raw) -replace "Day${maxDay}", "Day${nextDay}") |
Set-Content -Path ".\Program.cs"

[System.Uri]$Uri = "https://adventofcode.com/${year}/day/${nextDay}/input"
$ContentType = "text/html" # Add the content type
$Method = 'GET' # Add the method type

$SessionCookie = New-Object System.Net.Cookie
$SessionCookie.Name = "session" # Add the name of the cookie
$SessionCookie.Value = $Env:AocToken  # Add the value of the cookie
$SessionCookie.Domain = $uri.DnsSafeHost

$GaCookie = New-Object System.Net.Cookie
$GaCookie.Name = "_ga" # Add the name of the cookie
$GaCookie.Value = $Env:AocGa # Add the value of the cookie
$GaCookie.Domain = $uri.DnsSafeHost

$GidCookie = New-Object System.Net.Cookie
$GidCookie.Name = "_gid" # Add the name of the cookie
$GidCookie.Value = $Env:AocGid # Add the value of the cookie
$GidCookie.Domain = $uri.DnsSafeHost

$WebSession = New-Object Microsoft.PowerShell.Commands.WebRequestSession
$WebSession.Cookies.Add($SessionCookie)
$WebSession.Cookies.Add($GaCookie)
$WebSession.Cookies.Add($GidCookie)


# Splat the parameters
$props = @{
	Uri         = $uri.AbsoluteUri
	ContentType = $ContentType
	Method      = $Method
	WebSession  = $WebSession
}

Invoke-RestMethod @props > $inputFile
#[System.IO.File]::WriteAllText($FilePath,"TestTest",[System.Text.Encoding]::ASCII)

$WebResponse = Invoke-WebRequest "https://adventofcode.com/${year}/day/${nextDay}"

$WebResponse.AllElements | Where { $_.TagName -eq "pre" } | Foreach-Object -Begin { $counter = 1 } {
	$_ | Select -ExpandProperty "InnerText" > ".\${nextFolder}\inputs\demo${counter}.txt"
	$counter++
}

Start-Process "https://adventofcode.com/2022/day/${nextDay}"
