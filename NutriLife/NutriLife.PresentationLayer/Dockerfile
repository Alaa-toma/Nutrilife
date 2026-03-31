FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 10000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY NutriLife.PresentationLayer/ ./NutriLife.PresentationLayer/
COPY Nutrilife.DataAccessLayer/ ./Nutrilife.DataAccessLayer/
COPY Nutrilife.LogicLayer/ ./Nutrilife.LogicLayer/
COPY NutriLife.sln .

RUN dotnet restore "NutriLife.sln" --verbosity normal

RUN dotnet publish "NutriLife.PresentationLayer/NutriLife.PresentationLayer.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NutriLife.PresentationLayer.dll"]