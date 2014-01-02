[T4Scaffolding.Scaffolder(Description = "Create Mapper Collection and add class inside")][CmdletBinding()]
param(        
	[parameter(Position = 0, Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ModelType,
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $true
)

# Find the MapperCollection class, or create it via a template if not already present
$foundMapperCollectionType = Get-ProjectType MapperCollection -Project $Project -AllowMultiple
if(!$foundMapperCollectionType) {
	#Create MapperCollection
	$outputPath = Join-Path "Mappers" "MapperCollection"
	$defaultNamespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value
	
Add-ProjectItemViaTemplate $outputPath -Template MapperCollectionTemplate `
	-Model @{ Namespace = $defaultNamespace } `
	-SuccessMessage "Added MapperCollection at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force
	
	$foundMapperCollectionType = Get-ProjectType MapperCollection -Project $Project
}

# Add a new property on the DbContext class
if ($foundMapperCollectionType) {
	$foundModelType = Get-ProjectType $ModelType -Project $Project -BlockUi -ErrorAction SilentlyContinue
	if (!$foundModelType) { return }
	$propertyName = $foundModelType.Name;
	# This *is* a DbContext, so we can freely add a new property if there isn't already one for this model
	Add-ClassMemberViaTemplate -Name $propertyName -CodeClass $foundMapperCollectionType -Template MapperCollectionItemTemplate -Model @{
		ModelName = $propertyName;
	} -SuccessMessage "Added '$propertyName' to class '$($foundMapperCollectionType.FullName)'" -TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage
}