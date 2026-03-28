FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 10000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy all layers so dependencies can be resolved
COPY ../Nutrilife.DataAccessLayer ./Nutrilife.DataAccessLayer
COPY ../Nutrilife.LogicLayer ./Nutrilife.LogicLayer
COPY . ./NutriLife.PresentationLayer

WORKDIR /src/NutriLife.PresentationLayer
RUN dotnet restore "NutriLife.PresentationLayer.csproj"
RUN dotnet publish "NutriLife.PresentationLayer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NutriLife.PresentationLayer.dll"]