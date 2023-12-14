FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY AspNetCore.RealTime.SignalR.csproj ./
RUN dotnet restore "AspNetCore.RealTime.SignalR.csproj"
COPY . .
RUN dotnet build "AspNetCore.RealTime.SignalR.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspNetCore.RealTime.SignalR.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspNetCore.RealTime.SignalR.dll"]