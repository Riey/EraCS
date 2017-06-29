$CoreVersion = "1.1.1.1"
$FrontEndBasicVersion = "1.1.1.1"
$EraConsoleVersion = "1.0.0.1"
$UWPRendererVersion = "1.0.0.1"
$XamarinVersion = "2.3.4.224"

$PropertiesArg = "xamarin_version=$XamarinVersion;core_version=$CoreVersion;frontend_version=$FrontEndBasicVersion;console_version=$EraConsoleVersion;uwp_renderer_version=$UWPRendererVersion" 

nuget pack EraCS.Core.nuspec -Version $CoreVersion -properties $PropertiesArg
nuget pack EraCS.UI.FrontEnd.Basic.nuspec -Version $FrontEndBasicVersion -properties $PropertiesArg
nuget pack EraCS.UI.EraConsole.nuspec -Version $EraConsoleVersion -properties $PropertiesArg
nuget pack EraCS.UI.EraConsole.ViewRenderer.UWP.nuspec -Version $UWPRendererVersion -properties $PropertiesArg -properties platform=x64
nuget pack EraCS.UI.EraConsole.ViewRenderer.UWP.nuspec -Version $UWPRendererVersion -properties $PropertiesArg -properties platform=x86
nuget pack EraCS.UI.EraConsole.ViewRenderer.UWP.nuspec -Version $UWPRendererVersion -properties $PropertiesArg -properties platform=ARM
