FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["sample/Xerris.Sample.Api/Xerris.Sample.Api.csproj", "Xerris.Sample.Api/"]
RUN dotnet restore "sample/Xerris.Sample.Api/Xerris.Sample.Api.csproj"
COPY . .
WORKDIR "/src/Xerris.Sample.Api"
RUN dotnet build "Xerris.Sample.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Xerris.Sample.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Xerris.Sample.Api.dll"]
