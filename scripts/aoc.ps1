#!/usr/bin/env pwsh
# This script is used to run the AoC-2015 project.

# Set default values for the flags
$csharp = $false
$fsharp = $false
$python = $false
$typescript = $false
$rust = $false

$last = $false
$debug = $false
$demo = $false
$first = $false
$second = $false

# Function to display help
function Show-Help {
    Write-Host "--------------------------------------------------------------------------------"
    Get-Content -Path "../HEADER"
    Write-Host ""
    Write-Host "--------------------------------------------------------------------------------"
    Write-Host "Usage: ./aoc.ps1 [options] [arguments]"
    Write-Host "Arguments: "
    Write-Host "  --last   |     Run the last implemented solution"
    Write-Host "  --debug  |     Run the solution in debug mode"
    Write-Host "  --demo   |     Run the solution with demo data"
    Write-Host "  --first  |     Only run the first part of the solution"
    Write-Host "  --second |     Only run the second part of the solution"
    Write-Host "  [nums]   |     numbers of days to run the solution for (e.g. 1 2 3)"
    Write-Host "Options:"
    Write-Host "  -h       |     --help"
    Write-Host "  -cs      |     --csharp"
    Write-Host "  -fs      |     --fsharp"
    Write-Host "  -py      |     --python"
    Write-Host "  -ts      |     --typescript"
    Write-Host "  -r       |     --rust"
    Exit
}

# Parse the flags
while ($args.Count -gt 0) {
    switch ($args[0]) {
        '-h' { Show-Help }
        '--help' { Show-Help }
        '-cs' { $csharp = $true }
        '--csharp' { $csharp = $true }
        '-fs' { $fsharp = $true }
        '--fsharp' { $fsharp = $true }
        '-py' { $python = $true }
        '--python' { $python = $true }
        '-ts' { $typescript = $true }
        '--typescript' { $typescript = $true }
        '-r' { $rust = $true }
        '--rust' { $rust = $true }
        '--last' { $last = $true }
        '--debug' { $debug = $true }
        '--demo' { $demo = $true }
        '--first' { $first = $true }
        '--second' { $second = $true }
        '--' { $args = $args[1..$args.Length]; break }
        default { break }
    }
    $args = $args[1..$args.Length]
}

$exec_command = ""

# Execute the appropriate command based on the input flags
if ($csharp) {
    Set-Location -Path "./src/aoc-csharp/" -ErrorAction Stop
    $exec_command = "dotnet run $args"
}
elseif ($fsharp) {
    Set-Location -Path "./src/aoc-fsharp/" -ErrorAction Stop
    $exec_command = "dotnet run $args"
}
elseif ($typescript) {
    Set-Location -Path "./src/aoc-typescript/" -ErrorAction Stop
    $exec_command = "ts-node $args"
}
elseif ($python) {
    Set-Location -Path "./src/aoc-python/" -ErrorAction Stop
    $exec_command = "python3 $args"
}
elseif ($rust) {
    Set-Location -Path "./src/aoc-rust/" -ErrorAction Stop
    $exec_command = "cargo run $args"
}
else {
    # Default to csharp solution if no flags are given
    Set-Location -Path "./src/aoc-csharp/" -ErrorAction Stop
    Write-Host "Running csharp solution as a fallback because no flags were given."
    $exec_command = "dotnet run $args"
}

# Add the universal flags like --last to the command
if ($last) { $exec_command += " --last" }
if ($debug) { $exec_command += " --debug" }
if ($demo) { $exec_command += " --demo" }
if ($first) { $exec_command += " --first" }
if ($second) { $exec_command += " --second" }

# Execute the command
Write-Host "Executing command: $exec_command in $PWD"
Invoke-Expression $exec_command
