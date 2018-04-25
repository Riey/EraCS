$CoreVersion = "1.3.3.0"
$EraConsoleVersion = "1.4.1.0"
$FrontEndWpfVersion = "1.3.1.0"

$PropertiesArg = "core_version=$CoreVersion;frontend_wpf_version=$FrontEndWpfVersion;console_version=$EraConsoleVersion" 

nuget pack EraCS.UI.FrontEnd.Wpf.nuspec -Version $FrontEndWpfVersion -properties $PropertiesArg
copy ..\EraCS.Core\bin\Release\Riey.EraCS.Core.1.3.3.nupkg .
copy ..\EraCS.UI.EraConsole\bin\Release\Riey.EraCS.UI.EraConsole.1.4.1.nupkg .
