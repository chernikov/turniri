[T4Scaffolding.Scaffolder(Description = "Enter a description of Controller here")][CmdletBinding()]
param(        
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ControllerName,
	[string]$ModelType,
    [string]$Project,
	[switch]$Paging = $false,
	[switch]$Lang = $false,
	[string]$CodeLanguage,
	[string]$Area = "Default",
	[string]$ViewScaffolder = "View",
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

# If you haven't specified a model type, we'll guess from the controller name
if (!$ModelType) {
	if ($ControllerName.EndsWith("Controller", [StringComparison]::OrdinalIgnoreCase)) {
		# If you've given "PeopleController" as the full controller name, we're looking for a model called People or Person
		$ModelType = [System.Text.RegularExpressions.Regex]::Replace($ControllerName, "Controller$", "", [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
		$foundModelType = Get-ProjectType $ModelType -Project $Project -BlockUi -ErrorAction SilentlyContinue
		if (!$foundModelType) {
			$ModelType = [string](Get-SingularizedWord $ModelType)
			$foundModelType = Get-ProjectType $ModelType -Project $Project -BlockUi -ErrorAction SilentlyContinue
		}
	} else {
		# If you've given "people" as the controller name, we're looking for a model called People or Person, and the controller will be PeopleController
		$ModelType = $ControllerName
		$foundModelType = Get-ProjectType $ModelType -Project $Project -BlockUi -ErrorAction SilentlyContinue
		if (!$foundModelType) {
			$ModelType = [string](Get-SingularizedWord $ModelType)
			$foundModelType = Get-ProjectType $ModelType -Project $Project -BlockUi -ErrorAction SilentlyContinue
		}
		if ($foundModelType) {
			$ControllerName = $foundModelType.Name + "Controller"
		}
	}
	if (!$foundModelType) { throw "Cannot find a model type corresponding to a controller called '$ControllerName'. Try supplying a -ModelType parameter value." }
} else {
	# If you have specified a model type
	$foundModelType = Get-ProjectType $ModelType -Project $Project -BlockUi
	if (!$foundModelType) { return }
	if (!$ControllerName.EndsWith("Controller", [StringComparison]::OrdinalIgnoreCase)) {
		$ControllerName = $ControllerName + "Controller"
	}
}
Write-Host "Scaffolding $ControllerName..."

$outputPath = Join-Path Controllers $ControllerName
# We don't create areas here, so just ensure that if you specify one, it already exists
if ($Area) {
	$areaPath = Join-Path Areas $Area
	if (-not (Get-ProjectItem $areaPath -Project $Project)) {
		Write-Error "Cannot find area '$Area'. Make sure it exists already."
		return
	}
	$outputPath = Join-Path $areaPath $outputPath
}

$viewModel = $ModelType + "View"
$foundViewType = Get-ProjectType $viewModel -Project $Project -BlockUi -ErrorAction SilentlyContinue
if (!$foundViewType) { 
Write-Host "Not Found Model View"
return; }

$indexPaging = $Paging -and $true; 
$languageMode = $Lang -and $true;
$defaultNamespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value
$modelTypePluralized =  Get-PluralizedWord $foundModelType.Name

Add-ProjectItemViaTemplate $outputPath -Template ControllerTemplate `
	-Model @{ Namespace = $defaultNamespace; ControllerName = $ControllerName; ModelType = $foundModelType;  ModelTypePluralized = $modelTypePluralized; Paging = $indexPaging; Area = $Area; Lang = $languageMode } `
	-SuccessMessage "Added Controller output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force

Scaffold IndexView $ModelType -Area:$Area -Paging:$Paging -Lang:$Lang 
Scaffold EditView $ModelType -Area:$Area -Lang:$Lang 