FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Fireplace/Fireplace.csproj", "Fireplace/"]
RUN dotnet restore "Fireplace/Fireplace.csproj"
COPY . .
WORKDIR "/src/Fireplace"
RUN dotnet build "Fireplace.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fireplace.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fireplace.dll"]