# Tools
PIP ?= "$(shell which pip)"
PRECOMMIT ?= "$(shell which pre-commit)"


# ******************************************************************
##@ Precommit Tools

.PHONY: precommit-install
precommit-install:  ## Install precommit
	@echo -e "$(BCYAN)Initializing pre-commit installation...$(NORMAL)"
	@$(PIP) install pre-commit --break-system-packages

.PHONY: precommit-configure
precommit-configure:  ## Configure precommit hooks
	@echo -e "$(BCYAN)Starting pre-commit configuration...$(NORMAL)"
	@$(PRECOMMIT) install --install-hooks
	@$(PRECOMMIT) install --hook-type commit-msg
	@$(PRECOMMIT) install --hook-type pre-push

.PHONY: precommit-update
precommit-update:  ## Update precommit hooks
	@echo -e "$(BCYAN)Updating pre-commit hooks...$(NORMAL)"
	@$(PRECOMMIT) autoupdate

.PHONY: precommit-uninstall
precommit-uninstall:  ## Uninstall precommit hooks
	@echo -e "$(BCYAN)Uninstalling pre-commit hooks...$(NORMAL)"
	@$(PRECOMMIT) uninstall -t pre-commit -t pre-merge-commit -t pre-push -t prepare-commit-msg -t commit-msg -t post-commit -t post-checkout -t post-merge -t post-rewrite
