FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env

WORKDIR /app

COPY ./DietPlanner.Api/*.csproj ./
RUN dotnet restore

COPY ./DietPlanner.Api/ ./
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /publish
COPY --from=build-env /publish .

COPY ./Identity.db ../
COPY ./Products.db ../

EXPOSE 80

ENTRYPOINT ["dotnet", "DietPlanner.Api.dll"]