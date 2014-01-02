[T4Scaffolding.Scaffolder(Description = "create of SqlRepository for some ModelType")][CmdletBinding()]
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

$outputPath = Join-Path "SqlRepository" $ModelType
$namespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value

$modelTypePluralized = Get-PluralizedWord $foundModelType.Name

$IsLang = $Lang -and $true; 

if($IsLang) {
	$LangModelType = $ModelType + "Lang"
	$foundLangModelType = Get-ProjectType $LangModelType -Project $Project -BlockUi
	if (!$foundLangModelType) { return }
	
	$LangModelNamePlural =  Get-PluralizedWord $foundLangModelType.Name

	Add-ProjectItemViaTemplate $outputPath -Template SqlRepositoryLangTemplate `
		-Model @{ Namespace = $namespace; ModelType = [MarshalByRefObject]$foundModelType; ModelTypePluralized = [string]$modelTypePluralized; LangModelType = $foundLangModelType;	LangModelNamePlural = $LangModelNamePlural } `
		-SuccessMessage "Added SqlRepository output at {0}" `
		-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force
} else {
	Add-ProjectItemViaTemplate $outputPath -Template SqlRepositoryTemplate `
		-Model @{ Namespace = $namespace; ModelType = [MarshalByRefObject]$foundModelType; ModelTypePluralized = [string]$modelTypePluralized; 	} `
		-SuccessMessage "Added SqlRepository output at {0}" `
		-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force
}