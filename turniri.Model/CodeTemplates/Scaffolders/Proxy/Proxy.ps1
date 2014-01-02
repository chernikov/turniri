[T4Scaffolding.Scaffolder(Description = "Enter a description of Proxy here")][CmdletBinding()]
param(        
	[parameter(Position = 0, Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ModelType,
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Lang = $false,
	[switch]$Force = $false
)

$foundModelType = Get-ProjectType $ModelType -Project $Project -BlockUi
if (!$foundModelType) { return }


$outputPath = Join-Path "Proxy" $ModelType
$namespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value

$IsLang = $Lang -and $true; 

if($IsLang) {
	$LangModelType = $ModelType + "Lang"
	$foundLangModelType = Get-ProjectType $LangModelType -Project $Project -BlockUi
	if (!$foundLangModelType) { return }
	
	$LangModelNamePlural =  Get-PluralizedWord $foundLangModelType.Name
	
	Add-ProjectItemViaTemplate $outputPath -Template ProxyLangTemplate `
		-Model @{ Namespace = $namespace; ModelType = $ModelType; LangModelType = $foundLangModelType; LangModelNamePlural = $LangModelNamePlural } `
		-SuccessMessage "Added Proxy lang output at {0}" `
		-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force

} else {
	Add-ProjectItemViaTemplate $outputPath -Template ProxyTemplate `
		-Model @{ Namespace = $namespace; ModelType = $ModelType } `
		-SuccessMessage "Added Proxy output at {0}" `
		-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force
}