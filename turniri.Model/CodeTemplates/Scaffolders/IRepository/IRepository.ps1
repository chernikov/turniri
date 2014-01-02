[T4Scaffolding.Scaffolder(Description = "Create IRepository interface")][CmdletBinding()]
param(    
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ModelType,    
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

$foundModelType = Get-ProjectType $ModelType -Project $Project -BlockUi
if (!$foundModelType) { return }

# Find the IRepository interface, or create it via a template if not already present
$foundIRepositoryType = Get-ProjectType IRepository -Project $Project -AllowMultiple
if(!$foundIRepositoryType) {
	#Create IRepository
	$outputPath = "IRepository"
	$defaultNamespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value
	
Add-ProjectItemViaTemplate $outputPath -Template IRepositoryTemplate `
	-Model @{ Namespace = $defaultNamespace } `
	-SuccessMessage "Added IRepository at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force
	
	$foundIRepositoryType = Get-ProjectType IRepository -Project $Project
}

# Add a new property on the DbContext class
if ($foundIRepositoryType) {
	$propertyName = $foundModelType.Name
	$propertyNames = Get-PluralizedWord $propertyName
	# This *is* a DbContext, so we can freely add a new property if there isn't already one for this model
	Add-ClassMemberViaTemplate -Name $propertyName -CodeClass $foundIRepositoryType -Template IRepositoryItemTemplate -Model @{
		EntityType = $foundModelType;
		EntityTypeNamePluralized = $propertyNames;
	} -SuccessMessage "Added '$propertyName' to interface '$($foundIRepositoryType.FullName)'" -TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage
	
}

return @{
	DbContextType = $foundDbContextType
}