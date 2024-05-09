FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RtfmAPI.Presentation/RtfmAPI.Presentation.csproj", "RtfmAPI.Presentation/"]
COPY ["RtfmAPI.Application/RtfmAPI.Application.csproj", "RtfmAPI.Application/"]
COPY ["RtfmAPI.Domain/RtfmAPI.Domain.csproj", "RtfmAPI.Domain/"]
COPY ["RtfmAPI.Infrastructure/RtfmAPI.Infrastructure.csproj", "RtfmAPI.Infrastructure/"]
RUN dotnet restore "RtfmAPI.Presentation/RtfmAPI.Presentation.csproj"
COPY . .
WORKDIR "/src/RtfmAPI.Presentation"
RUN dotnet build "RtfmAPI.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RtfmAPI.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RtfmAPI.Presentation.dll"]
