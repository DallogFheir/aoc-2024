if ($args.Length -eq 1) {
    $day = $args[0]
} else {
    $day = (Get-Date).Day
}

$paddedDay = $day.ToString("D2")
$dayFolderName = "day" + $paddedDay

if (Test-Path -Path $dayFolderName) {
    Write-Output "Folder for day $day already exists."
    return
}

New-Item -ItemType Directory -Path $dayFolderName

$boilerplateFile = "Solution.cs.boilerplate"
$solutionFile = Join-Path -Path $dayFolderName -ChildPath "Solution.cs"
$fileContent = Get-Content -Path $boilerplateFile
$modifiedContent = $fileContent -replace 'X', $paddedDay
$modifiedContent | Set-Content -Path $solutionFile

$inputFile = Join-Path -Path $dayFolderName -ChildPath "input.txt"
New-Item -ItemType File -Path $inputFile -Force
