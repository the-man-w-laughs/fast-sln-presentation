FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5137

ENV ASPNETCORE_URLS=http://+:5137

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Back-end/FastSlnPresentation.Server/FastSlnPresentation.Server.csproj", "Back-end/FastSlnPresentation.Server/"]
RUN dotnet restore "Back-end/FastSlnPresentation.Server/FastSlnPresentation.Server.csproj"
COPY . .
WORKDIR "/src/Back-end/FastSlnPresentation.Server"
RUN dotnet build "FastSlnPresentation.Server.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "FastSlnPresentation.Server.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FastSlnPresentation.Server.dll"]
