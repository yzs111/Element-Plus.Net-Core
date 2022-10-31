#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CW_ToyShopping/CW_ToyShopping.csproj", "CW_ToyShopping/"]
RUN dotnet restore "CW_ToyShopping/CW_ToyShopping.csproj"
COPY . .
WORKDIR "/src/CW_ToyShopping"
RUN dotnet build "CW_ToyShopping.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CW_ToyShopping.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CW_ToyShopping.dll"]