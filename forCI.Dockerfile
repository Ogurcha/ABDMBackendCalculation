FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /WebAPI

RUN sed -i'.bak' 's/Components: main$/Components: main contrib/' \
    /etc/apt/sources.list.d/debian.sources

RUN apt-get update; apt-get install -y ttf-mscorefonts-installer fontconfig
RUN apt-get update && \
    apt-get install -y \
        fontconfig libharfbuzz0b libfreetype6 libgdiplus libc6-dev

COPY ./WebAPI .
ENTRYPOINT ["dotnet", "Abdm.Calculation.WebApiCore.dll"]
