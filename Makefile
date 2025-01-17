define HEADER
  _                                ___
 |_   _|_  _  ._   _ o  _  ._   _   |  _  |  _  ._ _   _ _|_ ._
 |_ >< |_ (/_ | | _> | (_) | | _> o | (/_ | (/_ | | | (/_ |_ | \/
                                                               /
endef
export HEADER


# Colors
NORMAL	:= \033[0m
BCYAN	:= \033[1;36m
RED		:= \033[0;31m
GREEN	:= \033[0;32m
YELLOW	:= \033[0;33m

.DEFAULT_GOAL := help


# ******************************************************************
##@ General

.PHONY: help
help:  ## Display this help
	@clear
	@echo "$$HEADER"
	@awk 'BEGIN {FS = ":.*##"; printf "\nUsage:\n  make \033[36m\033[3mcommand \033[0m \n"} /^[a-zA-Z_-]+:.*?##/ { printf "  \033[36m%-15s\033[0m %s\n", $$1, $$2 } /^##@/ { printf "\n\033[1m%s\033[0m\n", substr($$0, 5) } ' $(MAKEFILE_LIST)


# Shared components
include make/dotnet.mk
include make/precommit.mk
