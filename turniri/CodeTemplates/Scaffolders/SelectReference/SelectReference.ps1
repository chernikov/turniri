[T4Scaffolding.Scaffolder(Description = "Enter a description of SelectReference here")][CmdletBinding()]
param(        
	[parameter(Position = 0,Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ModelType,
	[parameter(Position = 1,Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ReferenceType,
    [string]$Project,
	[string[]]$TemplateFolders,
	[switch]$Lang = $false
)

$foundModelType = Get-ProjectType $ModelType -Project $Project -BlockUi -ErrorAction SilentlyContinue
if (!$foundModelType) { return; }

$ViewType = $ModelType + "View" 
$foundViewType = Get-ProjectType $ViewType -Project $Project -BlockUi -ErrorAction SilentlyContinue
if (!$foundViewType) { return; }

$foundReferenceType = Get-ProjectType $ReferenceType -Project $Project -BlockUi -ErrorAction SilentlyContinue
if (!$foundReferenceType) { return; }

$propertyName = $foundReferenceType.Name
$propertyNames = Get-PluralizedWord $propertyName
$languageMode = $Lang -and $true

# This *is* a DbContext, so we can freely add a new property if there isn't already one for this model
Add-ClassMemberViaTemplate -Name $propertyName -CodeClass $foundViewType -Template SelectReferenceTemplate -Model @{
	EntityType = $foundReferenceType;
	EntityTypeNamePluralized = $propertyNames;
	Lang = $languageMode;
} -SuccessMessage "Added reference '$propertyName' to  '$($foundViewType.FullName)'" -TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage
	