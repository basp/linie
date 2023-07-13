param (
    [string] $OutputDir = ".\src\Linie\bin\Debug"
)
Get-ChildItem -Path $OutputDir -Filter "Linie.*.nupkg" |
Sort-Object -Property LastWriteTime -Descending |
Select-Object -First 1
