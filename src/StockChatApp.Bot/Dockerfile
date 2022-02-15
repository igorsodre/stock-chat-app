FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/StockChatApp.Bot/StockChatApp.Bot.csproj", "StockChatApp.Bot/"]
RUN dotnet restore "StockChatApp.Bot/StockChatApp.Bot.csproj"
COPY ["src/StockChatApp.Bot", "StockChatApp.Bot"]
WORKDIR "/src/StockChatApp.Bot"
RUN dotnet build "StockChatApp.Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StockChatApp.Bot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockChatApp.Bot.dll"]
