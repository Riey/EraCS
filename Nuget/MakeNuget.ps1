$CoreVersion = "1.2.3.0"
$FrontEndWpfVersion = "1.2.1.2"
$EraConsoleVersion = "1.2.6.0"

$PropertiesArg = "core_version=$CoreVersion;frontend_wpf_version=$FrontEndWpfVersion;console_version=$EraConsoleVersion" 

nuget pack EraCS.Core.nuspec -Version $CoreVersion -properties $PropertiesArg
nuget pack EraCS.UI.FrontEnd.Wpf.nuspec -Version $FrontEndWpfVersion -properties $PropertiesArg
nuget pack EraCS.UI.EraConsole.nuspec -Version $EraConsoleVersion -properties $PropertiesArg
