FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
COPY . ./


RUN dotnet restore APIFCG/APIFCG.csproj
RUN dotnet publish APIFCG/APIFCG.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "APIFCG.dll"]
# This Dockerfile builds a Blazor WebAssembly application using .NET 8.0 SDK