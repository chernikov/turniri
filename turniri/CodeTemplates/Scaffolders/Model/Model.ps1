[T4Scaffolding.Scaffolder(Description = "Create View To Model class")][CmdletBinding()]
param(        
	[parameter(Position = 0, Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ModelType,
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $true,
	[switch]$Related = $false,
	[switch]$Lang = $false
)

$foundModelType = Get-ProjectType $ModelType -Project $Project -BlockUi -ErrorAction SilentlyContinue
if (!$foundModelType) { return }

$outputFile = $ModelType + "View"
$outputPath = Join-Path "Models/ViewModels" $outputFile
Write-Host "Scaffolding Model $outputFile..."

$namespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value
$modelTypePluralized = Get-PluralizedWord $foundModelType.Name
$relatedEntities = $Related -and $true;
$languageMode = $Lang -and $true;

Add-ProjectItemViaTemplate $outputPath -Template ModelTemplate `
	-Model @{ Namespace = $namespace; ModelType = $foundModelType; ModelTypePluralized = $modelTypePluralized; Related = $relatedEntities; Lang = $languageMode } `
	-SuccessMessage "Added $ModelType output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force

Scaffold MapperCollection $ModelType

Scaffold CommonMapper