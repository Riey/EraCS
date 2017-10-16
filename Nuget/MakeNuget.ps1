$CoreVersion = "1.2.4.0"
$FrontEndWpfVersion = "1.2.2.0"
$EraConsoleVersion = "1.3.0.0"

$PropertiesArg = "core_version=$CoreVersion;frontend_wpf_version=$FrontEndWpfVersion;console_version=$EraConsoleVersion" 

nuget pack EraCS.UI.FrontEnd.Wpf.nuspec -Version $FrontEndWpfVersion -properties $PropertiesArg
