function Get-ScriptDirectory
{
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value
    Split-Path $Invocation.MyCommand.Path
}

$sourcePath =  Join-Path (Get-ScriptDirectory) SelfPublish
$result = (Get-ChildItem ($sourcePath) -Recurse | where {$_.Name -eq 'bin' -or $_.Name -eq 'obj' })

foreach($path in $result) {
    Write-Output $path.FullName
    Remove-Item $path.FullName -Force -Recurse
}
