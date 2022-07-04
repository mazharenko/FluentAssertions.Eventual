FROM ghcr.io/mazharenko/dotnet-interactive-docker:main

COPY --chown=1000 ./docs ${HOME}/docs/

WORKDIR ${HOME}/docs/