$CoreVersion = "1.2.1"
$FrontEndWpfVersion = "1.2.1.1"
$EraConsoleVersion = "1.2.4"
$XamarinVersion = "2.3.4.224"

$PropertiesArg = "xamarin_version=$XamarinVersion;core_version=$CoreVersion;frontend_wpf_version=$FrontEndWpfVersion;console_version=$EraConsoleVersion" 

nuget pack EraCS.Core.nuspec -Version $CoreVersion -properties $PropertiesArg
nuget pack EraCS.UI.FrontEnd.Wpf.nuspec -Version $FrontEndWpfVersion -properties $PropertiesArg
nuget pack EraCS.UI.EraConsole.nuspec -Version $EraConsoleVersion -properties $PropertiesArg
