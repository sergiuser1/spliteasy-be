#!/usr/bin/env sh

export DOTNET_WATCH_RESTART_ON_RUDE_EDIT=true
export DOTNET_WATCH_SUPPRESS_BROWSER_REFRESH=true

exec dotnet watch run --project spliteasy.Api/
