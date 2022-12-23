FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/PicCard.Subscription.Api/PicCard.Subscription.Api.csproj", "src/PicCard.Subscription.Api/"]
RUN dotnet restore "src/PicCard.Subscription.Api/PicCard.Subscription.Api.csproj"
COPY . .
WORKDIR "/src/src/PicCard.Subscription.Api"
RUN dotnet build "PicCard.Subscription.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PicCard.Subscription.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PicCard.Subscription.Api.dll"]