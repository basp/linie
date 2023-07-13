param (
    [string] $Source = "local",
    [string] $Project = ".\src\Linie\Linie.csproj"
)
dotnet pack $Project
dotnet nuget push (.\Get-MostRecentPackage.ps1) -s $Source
