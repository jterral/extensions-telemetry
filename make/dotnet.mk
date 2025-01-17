# Tools
DOTNET ?= "$(shell which dotnet)"


# ******************************************************************
##@ .NET Commands

.PHONY: dotnet-build
dotnet-build:  ## Dotnet build
	@echo -e "$(BCYAN)Building .NET solution...$(NORMAL)"
	@$(DOTNET) restore --configfile nuget.config
	@$(DOTNET) build --no-restore

.PHONY: dotnet-clean
dotnet-clean:  ## Dotnet clean
	@echo -e "$(BCYAN)Cleaning .NET solution...$(NORMAL)"
	@$(DOTNET) clean

.PHONY: dotnet-pack
dotnet-pack:  ## Dotnet pack
	@echo -e "$(BCYAN)Creating NuGet package...$(NORMAL)"
	@$(DOTNET) pack --configuration Release --verbosity detailed --output nupkgs
