[T4Scaffolding.Scaffolder(Description = "Enter a description of ProviderRepository here")][CmdletBinding()]
param(
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ModelType,    
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false,
	[switch]$Lang = $false
)

Scaffold IRepository $ModelType -Project $Project -CodeLanguage $CodeLanguage -TemplateFolders $TemplateFolders -Force:$Force 
Scaffold Proxy $ModelType -Project $Project -CodeLanguage $CodeLanguage -TemplateFolders $TemplateFolders -Force:$Force -Lang:$Lang
Scaffold SqlRepository $ModelType -Project $Project -CodeLanguage $CodeLanguage -TemplateFolders $TemplateFolders -Force:$Force -Lang:$Lang