FROM ghcr.io/mazharenko/dotnet-interactive-docker:1.0.340501

COPY --chown=1000 ./docs ${HOME}/docs/

WORKDIR ${HOME}/docs/