FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY . .
RUN dotnet restore
WORKDIR /src/Services/microCommerce.ProductApi
RUN dotnet build --no-restore -o /app

FROM build AS publish
RUN dotnet publish --no-restore -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "microCommerce.ProductApi.dll"]