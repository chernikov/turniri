[T4Scaffolding.Scaffolder(Description = "Enter a description of IndexView here")][CmdletBinding()]
param(        
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ModelType,
    [string]$Project,
	[string]$Area,
	[switch]$Paging = $false,
	[switch]$Lang = $false,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)
$foundModelType = Get-ProjectType $ModelType -Project $Project -BlockUi -ErrorAction SilentlyContinue
if (!$foundModelType) { return; }

$viewModel = $ModelType + "View"
$foundViewType = Get-ProjectType $viewModel -Project $Project -BlockUi -ErrorAction SilentlyContinue
if (!$foundViewType) { 
Write-Host "Not Found Model View"
return; }
Write-Host "View: $viewModel"

$outputPath = Join-Path $ModelType Index
$outputPath = Join-Path Views $outputPath

# We don't create areas here, so just ensure that if you specify one, it already exists
if ($Area) {
	$areaPath = Join-Path Areas $Area
	if (-not (Get-ProjectItem $areaPath -Project $Project)) {
		Write-Error "Cannot find area '$Area'. Make sure it exists already."
		return
	}
	$outputPath = Join-Path $areaPath $outputPath
}
Write-Host "Output: $outputPath"
$defaultNamespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value
$indexPaging = $Paging -and $true; 
$languageMode = $Lang -and $true;

Add-ProjectItemViaTemplate $outputPath -Template IndexViewTemplate `
	-Model @{ Namespace = $defaultNamespace; ModelType = $foundModelType; ViewType = $foundViewType; Paging = $indexPaging; Area = $Area; Lang = $languageMode } `
	-SuccessMessage "Added IndexView output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force